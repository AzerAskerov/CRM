using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CRM.Data.Database;
using CRM.Operation.Enums;
using CRM.Operation.Models.RequestModels;
using Zircon.Core.OperationModel;
using CRM.Operation.Helpers;
using CRM.Operation.Models.Terror;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Zircon.Core.Enums;
using Zircon.Core.Extensions;
using Zircon.Core.Config;
using Microsoft.Extensions.Caching.Memory;

namespace CRM.Operation.Terror
{
    public class CounterTerrorismOperation : BusinessOperation<ClientDataPayload>, ISendEmailOperation
    {
        private readonly CRMDbContext _context;
        private static List<TerrorPersonModel> _terroristList;
        private static TerrorConfig _terrorConfigSettings;
        private static List<FieldsConfig> _fieldsFromConfig;
        private static IList<int> _clientTagsFromConfig;
        private static decimal _totalThresholdFromConfig;
        private string _notificationEmail;
        private TerrorPersonModel incomingPerson;
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        //main result
        public readonly IList<TerrorScoreResultModel> Terrorists;

        public CounterTerrorismOperation(CRMDbContext db) : base(db)
        {
            _context = db;
            _terroristList = new List<TerrorPersonModel>();
            Terrorists = new List<TerrorScoreResultModel>();
            incomingPerson = new TerrorPersonModel();
        }

        protected override void Prepare()
        {
            LoadTerrorConfig();
            if (!RunTerrorOperation())
                return;

            LoadTerroristByTags(_clientTagsFromConfig);
            Maps.MapHelper.MapModeToTerrorPersonModel(Parameters, incomingPerson);
        }


        protected override void DoExecute()
        {
            foreach (var terrorist in _terroristList)
            {
                List<fieldModel> fieldScores = CalculateLevenshteinScorePerField(terrorist);
                FindPotentialTerrorist(terrorist, fieldScores, _totalThresholdFromConfig);
            }

            if (Terrorists.Any())
                SaveTerrorists();
        }

        private void SaveTerrorists()
        {
            var log = new TerrorLog()
            {
                Inn = Parameters.INN,
                OperationType = Parameters.OperationType,
                OperationDescription = Parameters.OperationDescription,
                JsonResult = JsonConvert.SerializeObject(Terrorists),
                RegisteredDateTime = DateTime.Now
            };

            _context.TerrorLog.AddRange(log);
            _context.SaveChanges();

            var emailModel = new EmailModel
            {
                Body = string.Format($"Found Operation made by person marked as Terrorist. Operation description: {Parameters.OperationDescription}; Person ID in CRM: {Parameters.INN}"),
                From = Settings.GetString("MAILSERVER_MAIL_FROM"),
                To = _notificationEmail,
                Subject = "Found suspicious activity".Translate(),
                SystemOid = 8,
                SubType = QueueItemSubtype.EmailSuspeciousActivity
            };

            Result.Merge((this as ISendEmailOperation).SendEmail(emailModel));

        }

        private List<fieldModel> CalculateLevenshteinScorePerField(TerrorPersonModel terrorist)
        {
            var terrorResult = new List<fieldModel>();

            foreach (var field in _fieldsFromConfig)
            {

                var score = CalculateLevenshteinScore(terrorist, incomingPerson, field);
                if (score == -1)
                    continue;

                terrorResult.Add(new fieldModel() { FieldName = field.fieldName, LvScore = score, FieldWeight = field.fieldWeight });
            }

            return terrorResult;
        }

        private void FindPotentialTerrorist(TerrorPersonModel terrorist, List<fieldModel> fieldScores, decimal totalThresholdScore)
        {
            var clientTotalLvScore = fieldScores.Sum(x => x.LvScore);
            var totalFieldsWeight = fieldScores.Sum(x => x.FieldWeight);

            if (clientTotalLvScore >= totalThresholdScore * 100)
            {
                var terrorScoreResult = new TerrorScoreResultModel()
                {
                    Inn = terrorist.Inn.Value,
                    TotalLvScore = clientTotalLvScore,
                    fieldsScore = new List<fieldModel>(fieldScores)
                };

                Terrorists.Add(terrorScoreResult);
            }
        }


        private void LoadTerrorConfig()
        {
            if (_terrorConfigSettings != null) return;

            _terrorConfigSettings = AppSettings.TerrorConfiguration;
            _fieldsFromConfig = _terrorConfigSettings.fieldsConfig;
            _totalThresholdFromConfig = AppSettings.TerrorConfiguration.thresholdScore;
            _notificationEmail = AppSettings.TerrorConfiguration.notificationEmail;
            _clientTagsFromConfig = AppSettings.TerrorConfiguration.clientsWithTagId;
        }

        private static bool RunTerrorOperation()
        {
            var disabled = AppSettings.GetSettingValue("terrorConfig", "disabled");
            return disabled != null && disabled.ToLower() == "false";
        }



        private void LoadTerroristByTags(IEnumerable<int> clientTags)
        {
            if (!_terroristList.Any())
            {
                // Step 1: Get the blacklist of VOENs (INNs) and cache it.
                var allTaggedInns = GetCachedBlacklistedVOENs(clientTags);

                // Step 2: Preload all data into memory and structure it as dictionaries for fast lookup by Inn.
                var getPhysicalPerson = _context.PhysicalPeople
                    .Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && allTaggedInns.Contains(x.Inn))
                    .ToDictionary(x => x.Inn); // Use Dictionary for fast lookups

                var clientInfoComps = _context.ClientContactInfoComps
                    .Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && allTaggedInns.Contains(x.Inn))
                    .Include(x => x.ContactInfo)
                    .ToList();

                // Group Client Info by Inn for easy lookup
                var groupedClientInfoComps = clientInfoComps
                    .GroupBy(x => x.Inn)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var docs = GetDocuments(allTaggedInns);

                var addresses = GetAddresses(allTaggedInns);

                // Step 3: Loop through each VOEN and populate _terroristList using preloaded data.
                foreach (var inn in allTaggedInns)
                {
                    // Get the physical person
                    if (!getPhysicalPerson.TryGetValue(inn, out var ph))
                    {
                        continue; // Skip if no physical person found
                    }

                    // Get phones and emails from grouped client info
                    var ciPhone = groupedClientInfoComps.TryGetValue(inn, out var infoComps)
                        ? infoComps.Where(ci => ci.ContactInfo.Type.In((int)ContactInfoType.MobilePhone, (int)ContactInfoType.MainPhone))
                            .Select(ci => ci.ContactInfo.Value)
                            .ToList()
                        : null;

                    var ciEmail = groupedClientInfoComps.TryGetValue(inn, out infoComps)
                        ? infoComps.Where(ci => ci.ContactInfo.Type == (int)ContactInfoType.Email)
                            .Select(ci => ci.ContactInfo.Value)
                            .ToList()
                        : null;

                    // Get passport information
                    var passport = docs.TryGetValue(inn.ToString(), out var personDocs)
                        ? personDocs.FirstOrDefault(d => d.DocumentType == (int)DocumentTypeEnum.Passport)?.DocumentNumber
                        : null;

                    // Get addresses
                    var addressList = addresses.TryGetValue(inn, out var addrList)
                        ? addrList.Select(a => a.AdditionalInfo).ToList()
                        : null;

                    // Add to terrorist list
                    _terroristList.Add(new TerrorPersonModel()
                    {
                        Inn = inn,
                        FirstName = ph.FirstName,
                        LastName = ph.LastName,
                        FullName = ph.FullName,
                        Pin = ph.Pin,
                        Email = ciEmail,
                        PhoneNumber = ciPhone,
                        Birthdate = ph.BirthDate,
                        PassportNr = passport,
                        Addresses = addressList
                    });
                }
            }
        }

        private List<int> GetCachedBlacklistedVOENs(IEnumerable<int> clientTags)
        {
            string cacheKey = "blacklist_voens";

            // Check if blacklist is in cache
            if (!_cache.TryGetValue(cacheKey, out List<int> blacklistedVOENs))
            {
                // Cache miss, so fetch from the database
                blacklistedVOENs = _context.TagComps
                    .Where(x => clientTags.Contains(x.TagId) && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now)
                    .Select(x => x.Inn)
                    .ToList();

                _cache.Set(cacheKey, blacklistedVOENs, TimeSpan.FromHours(1));
            }

            return blacklistedVOENs;
        }


        public Dictionary<int, List<Address>> GetAddresses(List<int> allTaggedInns)
        {
            // Define a cache key
            var cacheKey = "AddressesGroupedByInn";

            // Try to get the cached data
            if (!_cache.TryGetValue(cacheKey, out Dictionary<int, List<Address>> addresses))
            {
                // If data is not cached, query from the database
                addresses = _context.Addresses
                    .Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && allTaggedInns.Contains(x.Inn))
                    .AsEnumerable() // Move the data to memory
                    .GroupBy(x => x.Inn) // Group by Inn
                    .ToDictionary(g => g.Key, g => g.ToList()); // Create Dictionary<int, List<Address>>

                // Cache the data for 10 minutes
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                _cache.Set(cacheKey, addresses, cacheOptions);
            }

            return addresses;
        }


        public Dictionary<string, List<Document>> GetDocuments(List<int> allTaggedInns)
        {
            // Define cache key
            var cacheKey = "DocsGroupedByInn";

            // Try to get the cached data
            if (!_cache.TryGetValue(cacheKey, out Dictionary<string, List<Document>> docs))
            {
                // If data is not cached, query from the database
                docs = _context.Documents
                        .Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && allTaggedInns.Contains(x.Inn))
                        .AsEnumerable() // Fetch data into memory
                        .GroupBy(x => x.Inn)
                        .ToDictionary(g => g.Key.ToString(), g => g.ToList()); // Convert Inn to string

                // Cache the data
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                _cache.Set(cacheKey, docs, cacheOptions);
            }

            return docs;
        }



        private decimal CalculateLevenshteinScore(object x, object y, FieldsConfig field)
        {
            var terroristPropertyValue = GetPropertyValue(x, field.fieldName);
            var incomingPropertyValue = GetPropertyValue(y, field.fieldName);

            if (incomingPropertyValue.isnull)
                return -1;

            if (terroristPropertyValue.isnull)
                return -1;

            var retValue = 0m;

            if (incomingPropertyValue.obj.GetType().IsGenericType)
                foreach (var incomingField in (List<string>)incomingPropertyValue.obj)
                {
                    foreach (var terrorField in (List<string>)terroristPropertyValue.obj)
                    {
                        retValue = FieldLvPercentageScore(field, terrorField, incomingField);
                        if (retValue > 0) break;
                    }
                }
            else
                retValue = FieldLvPercentageScore(field, terroristPropertyValue, incomingPropertyValue);

            return retValue;
        }

        private static decimal FieldLvPercentageScore(FieldsConfig field, object terroristPropertyValue, object incomingPropertyValue)
        {
            if (terroristPropertyValue == null || incomingPropertyValue == null)
                return 0m;

            var levenshteinDistanceScore = LevenshteinDistance.Compute(terroristPropertyValue.ToString(), incomingPropertyValue.ToString());
            var longerStringLength = LongerStringLength(terroristPropertyValue.ToString(), incomingPropertyValue.ToString());

            var fieldLvPercentageScore = 0m;

            if (longerStringLength == 0) //preventing divide by zero
                return fieldLvPercentageScore;

            var levPercentage = 1 - (decimal)levenshteinDistanceScore / longerStringLength;

            if (levPercentage >= field.fieldThreshold)
                fieldLvPercentageScore = field.fieldWeight * levPercentage;

            return fieldLvPercentageScore;
        }

        private static (object obj, bool isnull) GetPropertyValue(object source, string propertyName)
        {
            var property = source.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var ret = property == null ? string.Empty : property.GetValue(source, null) == null ? string.Empty : property.GetValue(source, null);
            return (ret, property.GetValue(source, null) == null);
        }

        private static int LongerStringLength(string x, string y)
        {
            return x.Length > y.Length ? x.Length : y.Length;
        }
    }
}