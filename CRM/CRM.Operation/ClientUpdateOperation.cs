using CRM.Data.Database;
using CRM.Operation.Interfaces;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Zircon.Core.Extensions;

namespace CRM.Operation
{
    public class ClientUpdateOperation : ClientOperationBase, ICreateOrUpdateClient
    {
        private readonly CRMDbContext _db;

        public ClientUpdateOperation(CRMDbContext db) : base(db)
        {
            _db = db;
        }


        protected override void DoExecute()
        {

            ClientRef clientIncoming = base.ClientRef;

            if (clientIncoming == null) return;

            ClientRef clientFound = Parameters.FoundClient;


            if (Parameters.IncomingClient.INN.HasValue && Parameters.FoundClient.Inn != Parameters.IncomingClient.INN.Value)
            {
                Client incomingClient = _db.Clients.FirstOrDefault(x => x.Inn == Parameters.IncomingClient.INN.Value
                && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);

                if (incomingClient == null)
                {
                    Result.AddInformation("Active client not found");
                }
                else
                {

                    var query = _db.ClientRefs.Where(x => x.Inn == Parameters.IncomingClient.INN.Value);
                    query.Include(x => x.Clients.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == Parameters.IncomingClient.INN.Value)).Load();
                    query.Include(x => x.PhysicalPeople.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == Parameters.IncomingClient.INN.Value)).Load();
                    query.Include(x => x.Companies.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == Parameters.IncomingClient.INN.Value)).Load();
                    query.Include(x => x.Documents.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == Parameters.IncomingClient.INN.Value)).Load();
                    query.Include(x => x.ClientContactInfoComps.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == Parameters.IncomingClient.INN.Value))
                        .ThenInclude(x => x.ContactInfo).Load();
                    query.Include(x => x.Addresses.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == Parameters.IncomingClient.INN.Value)).Load();
                    query.Include(x => x.LeadObjects.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == Parameters.IncomingClient.INN.Value)).Load();
                    //check later
                    query.Include(x => x.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.Inn == Parameters.IncomingClient.INN.Value))
                        .ThenInclude(x => x.Tag).Load();
                    ClientRef incomingClientRef = query.Include(x => x.ClientRelationCompClient1Navigations.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.Client1 == Parameters.IncomingClient.INN.Value))
                 .First();

                    clientFound = DeactivateDuplicates(incomingClientRef, clientFound);
                }
            }

            ClientRef endClient = Merge(clientIncoming, clientFound);

        }

        /// <summary>
        /// find which one should be expired based on sum of quality levels
        /// </summary>
        /// <param name="incoming"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        private ClientRef DeactivateDuplicates(ClientRef incoming, ClientRef found)
        {
            //find active client based on sum of ql's

            int res = ComparePhysicalPerson(incoming.PhysicalPeople.FirstOrDefault(), found.PhysicalPeople.FirstOrDefault()) +
                CompareCompay(incoming.Companies.FirstOrDefault(), found.Companies.FirstOrDefault()) +
                CompareAddress(incoming.Addresses.FirstOrDefault(), found.Addresses.FirstOrDefault());

            //take  docs of weak client and to won's Documents

            //won found client
            if (res >= 0)
            {


                foreach (var item in incoming.Documents.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now))
                {
                    if (item != null && !found.Documents.Any
                     (x => x.DocumentNumber == item.DocumentNumber && x.DocumentType == item.DocumentType
                     && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now))
                    {
                        found.Documents.Add(
                        new Document
                        {
                            DocumentNumber = item.DocumentNumber,
                            DocumentType = item.DocumentType,
                            DocumentExpireDate = item.DocumentExpireDate,
                            Inn = found.Inn, //pay atention !!!
                            ValidFrom = DateTime.Now,
                            ValidTo = item.ValidTo
                        });
                    }
                }



                ExpireClient(incoming);
                return found;
            }


            //won incoming client
            foreach (var item in found.Documents.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now))
            {
                if (item != null && !incoming.Documents.Any
                 (x => x.DocumentNumber == item.DocumentNumber && x.DocumentType == item.DocumentType
                 && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now))
                {
                    incoming.Documents.Add(
                    new Document
                    {
                        DocumentNumber = item.DocumentNumber,
                        DocumentType = item.DocumentType,
                        DocumentExpireDate = item.DocumentExpireDate,
                        Inn = found.Inn, //pay atention !!!
                        ValidFrom = DateTime.Now,
                        ValidTo = item.ValidTo
                    });
                }
            }




            ExpireClient(found);
            return incoming;
        }


        /// <summary>
        /// deactivate loser client and add different docs to winning client's Documents
        /// </summary>
        /// <param name="inActiveClient">client that will be expired after method</param>
        /// <param name="activeClient">client that will remain active after method</param>
        private void ExpireClient(ClientRef inActiveClient)
        {
            inActiveClient.Clients.First().ValidTo = DateTime.Now.AddMilliseconds(-4);


            if (inActiveClient.Clients?.First().ClientType == (int)ClientType.Pyhsical)
            {

                inActiveClient.PhysicalPeople?.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList().ForEach(x => x.ValidTo = DateTime.Now.AddMilliseconds(-4));
                //   inActiveClient.PhysicalPeople.First().ValidTo = DateTime.Now.AddMilliseconds(-4);
            }
            else
            {
                //  inActiveClient.Companies.FirstOrDefault().ValidTo = DateTime.Now.AddMilliseconds(-4);
                inActiveClient.Companies?.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList().ForEach(x => x.ValidTo = DateTime.Now.AddMilliseconds(-4));

            }


            inActiveClient.ClientContactInfoComps?.ToList().ForEach(x => x.ValidTo = DateTime.Now.AddMilliseconds(-4));

            inActiveClient.Documents?.ToList().ForEach(x => x.ValidTo = DateTime.Now.AddMilliseconds(-4));

            inActiveClient.Addresses?.ToList().ForEach(x => x.ValidTo = DateTime.Now.AddMilliseconds(-4));
            inActiveClient.LeadObjects?.ToList().ForEach(x => x.ValidTo = DateTime.Now.AddMilliseconds(-4));

            inActiveClient.Assets?.ToList().ForEach(x => x.ValidTo = DateTime.Now.AddMilliseconds(-4));
            inActiveClient.UserClientComps?.ToList();

            inActiveClient.ClientRelationCompClient1Navigations.ToList().ForEach(x => x.ValidTo = DateTime.Now.AddMilliseconds(-4));

            inActiveClient.TagComps.ToList().ForEach(x => x.ValidTo = DateTime.Now.AddMilliseconds(-4));
        }


        private ClientRef Merge(ClientRef clientIncoming, ClientRef clientFound)
        {
            ClientRef resultClient = clientFound;

            if (clientIncoming.Clients.First().ClientType == 0)
                clientIncoming.Clients.First().ClientType = (int)ClientType.Pyhsical;

            //Client
            if (clientIncoming.Clients.First().ClientType == (int)ClientType.Pyhsical && clientIncoming.Clients.First().ClientType == clientFound.Clients.First().ClientType)
            {
                var personIncoming = clientIncoming.PhysicalPeople.OrderByDescending(x => x.ValidTo).FirstOrDefault();
                var personFound = clientFound.PhysicalPeople.
                    FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);

                MergePhysical(personIncoming, personFound);
            }
            else if (clientIncoming.Clients.First().ClientType == (int)ClientType.Company && clientIncoming.Clients.First().ClientType == clientFound.Clients.First().ClientType)
            {
                Company companyIncoming = clientIncoming.Companies.OrderByDescending(x => x.ValidTo).FirstOrDefault();
                Company companyFound = clientFound.Companies.
                    FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);

                MergeCompany(companyIncoming, companyFound);
            }


            var userCompIncoming = clientIncoming.UserClientComps.FirstOrDefault();
            MergeUserComposition(userCompIncoming, clientFound);

            //Address
            Address addressIncoming = clientIncoming.Addresses.
                FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
            Address addressFound = clientFound.Addresses.
                FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);

            MergeAddress(addressIncoming, addressFound, clientFound);



            CreateLeadObject(clientIncoming, clientFound);


            //Documents
            var documentsIncoming = clientIncoming.Documents.
                Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList();
            var documentsFound = clientFound.Documents.
            Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList();

            MergeDocuments(documentsIncoming, documentsFound, clientFound);


            //Contacts
            var contactsIncoming = clientIncoming.ClientContactInfoComps.
                Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList();
            var contactsFound = clientFound.ClientContactInfoComps.
                Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList();

            MergeContacts(contactsIncoming, contactsFound, clientFound);


            //Comments
            var commentsIncoming = clientIncoming.CommentComps.Where(x => x.ContactId == null).ToList();
            var commentsFound = clientFound.CommentComps.ToList();

            MergeComments(commentsIncoming, commentsFound, clientFound);

            //Tags
            var tagsIncoming = clientIncoming.TagComps.ToList();
            var tagsFound = clientFound.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList();

            MergeTags(tagsIncoming, tagsFound, clientFound);


            //Relations
            var relationsIncmoing = clientIncoming.ClientRelationCompClient1Navigations.
                Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList();

            var relationsFound = clientFound.ClientRelationCompClient1Navigations.
                    Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList();

            MergeRelations(relationsIncmoing, relationsFound, clientFound);

            var assetsIncmoing = clientIncoming.Assets.ToList();

            MergeAssets(assetsIncmoing, clientFound);

            MergeClientHistory(clientFound, clientIncoming);

            _db.SaveChanges();

            //bura qeder elimizde en son client yaranan veya update olunan client clientNewVersion oldu.
            //bundan sonra clientNewVersion butun doclarina gore Dcouments de axtaris edirik, ve yalniz inn
            //clientNewVersion-in innden  ferqli olanlari gotururuk. Cunki eger eyni olanlari gotursek ele ozu gelmis olacaq

            List<int> duplicateInn = new List<int>();

            //sened nomresine gore duplikat ehtimali olanlar
            List<Document> documents = new List<Document>();
            var foundActiveDocuments = clientFound.Documents.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ToList();
            foreach (var doc in foundActiveDocuments)
            {
                var foundDocs = _db.Documents.Where
                    (d => d.DocumentNumber == doc.DocumentNumber &&
                    d.DocumentType == doc.DocumentType &&
                    d.ValidFrom <= DateTime.Now &&
                    d.ValidTo >= DateTime.Now &&
                    d.Inn != doc.Inn).ToList();

                if (foundDocs != null && foundDocs.Count() > 0)
                    documents.AddRange(foundDocs);
            }

            foreach (var doc in documents.Where(x => !x.DocumentType.In(13, 14) && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now))
            {
                duplicateInn.Add(doc.Inn);
            }



            //Fiziki sexsler ucun pine gore dublikat ehtimali olanlar
            if (clientIncoming.Clients.First().ClientType == (int)ClientType.Pyhsical)
            {
                var duplicatefoundPhyscalPerson = _db.PhysicalPeople.Where(x => x.Inn != clientIncoming.Inn && x.Inn != clientFound.Inn
          && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now
          && x.Pin == clientIncoming.PhysicalPeople.First().Pin
          && !String.IsNullOrEmpty(x.Pin));

                duplicatefoundPhyscalPerson.ToList()?.ForEach(x => duplicateInn.Add(x.Inn));
            }


            //burda elimizde artiq clientNewVersion ferqli clientleri tapdigimiz ucun 
            //tapilan butun clientleri clientNewVersion ile muqayise edib udan terefi active saxlayiriq.
            // daha sonra active terefi clientNewVersion ile merge edirik

            foreach (var duplicateinn in duplicateInn)
            {
                var query = _db.ClientRefs.Where(x => x.Inn == duplicateinn);
                query.Include(x => x.Clients.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == duplicateinn)).Load();
                query.Include(x => x.PhysicalPeople.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == duplicateinn)).Load();
                query.Include(x => x.Companies.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == duplicateinn)).Load();
                query.Include(x => x.Documents.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == duplicateinn)).Load();
                query.Include(x => x.ClientContactInfoComps.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == duplicateinn))
                    .ThenInclude(x => x.ContactInfo).Load();
                query.Include(x => x.Addresses.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == duplicateinn)).Load();
                query.Include(x => x.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.Inn == duplicateinn))
                    .ThenInclude(x => x.Tag).Load();
                ClientRef clientRef = query.Include(x => x.ClientRelationCompClient1Navigations.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.Client1 == duplicateinn))
              .OrderBy(x => new Guid())
                    .First();


                ClientRef client = DeactivateDuplicates(clientRef, clientFound);
                //burda ola biler ki DeactivateDuplicates methodu bize clientNewVersion-i aktiv client kimi qaytarmis olsun.
                //neticede biz clientNewVersion -i ozu ile  yeniden merge asagida gondermis olaq.
                //yeni , MergeClents methodu bu sekilde cagrilmis olar MergeClients(clientNewVersion,clientNewVersion)
                //bu hal olanda clientNewVersion ozunde ki ClientContactComp ve diger bu table kimi tablelarda\ olan rowlar clientNewVersion ucun yeniden
                //tekrarlana biler. Yeni clientNewVersion ozundeki clientcomntactComplari ele yeniden ozune copy edek.
                //bunun qarsisini almaq ucun hemise clientNewVersion-in artiq clientContactComp olub olmadigini
                //yuxarida contactlari duzeldende yoxladim
                Merge(clientFound, client);
            }



            return resultClient;
        }

        private void MergeClientHistory(ClientRef clientFound, ClientRef clientIncoming)
        {
            foreach (var history in clientIncoming.ClientHistories)
            {
                clientFound.ClientHistories.Add(history);
            }
        }

        /// <summary>
        /// Compare 2 clients based on sum of quality levels
        /// -1 incoming is greater, 0 equals, 1 found is greater
        /// </summary>
        /// <param name="incoming"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        private int ComparePhysicalPerson(PhysicalPerson incoming, PhysicalPerson found)
        {
            if (incoming == null || found == null) return 0;

            int qlIncoming = (incoming.BirthDateQl ?? 0) +
                                          (incoming.FatherNameQl ?? 0) +
                                          (incoming.FirstNameQl ?? 0) +
                                          (incoming.FullNameQl ?? 0) +
                                          (incoming.LastNameQl ?? 0) +
                                          (incoming.ImageBase64Ql ?? 0) +
                                          (incoming.MonthlyIncomeQl ?? 0) +
                                          (incoming.PinQl ?? 0) +
                                          (incoming.PositionQl ?? 0);

            int qlFound = (found.BirthDateQl ?? 0) +
                        (found.FatherNameQl ?? 0) +
                        (found.FirstNameQl ?? 0) +
                        (found.FullNameQl ?? 0) +
                        (found.LastNameQl ?? 0) +
                        (found.ImageBase64Ql ?? 0) +
                        (found.MonthlyIncomeQl ?? 0) +
                        (found.PinQl ?? 0) +
                        (found.PositionQl ?? 0);

            if (qlIncoming > qlFound) return -1;
            else if (qlIncoming < qlFound) return 1;
            else return 0;
        }

        private int CompareCompay(Company incoming, Company found)
        {
            if (incoming == null || found == null) return 0;

            int qlIncoming = 0;
            qlIncoming = incoming.CompanyNameQl ?? 0;

            int qlFound = 0;
            qlFound = found.CompanyNameQl ?? 0;

            if (qlIncoming > qlFound) return -1;
            else if (qlIncoming < qlFound) return 1;
            else return 0;
        }

        private int CompareAddress(Address incoming, Address found)
        {
            if (incoming == null || found == null) return 0;

            int qlIncoming = 0;

            var incomingAddress = incoming;
            qlIncoming += (incomingAddress.CountryQl ?? 0) +
                (incomingAddress.DistrictOrStreetQl ?? 0) +
                (incomingAddress.RegOrCityQl ?? 0);

            int qlFound = 0;
            var foundAddress = found;
            qlFound += (foundAddress.CountryQl ?? 0) +
                        (foundAddress.DistrictOrStreetQl ?? 0) +
                        (foundAddress.RegOrCityQl ?? 0);

            if (qlIncoming > qlFound) return -1;
            else if (qlIncoming < qlFound) return 1;
            else return 0;

        }


        private bool CompareClientInfo<T>(object incoming, object found, out T diffence)
        {
            bool isChanged = false;

            if (found == null || incoming == null)
            {
                diffence = default(T);
                return isChanged;
            }


            T Difference = (T)Activator.CreateInstance(typeof(T));


            Type incomingobject = incoming.GetType();
            Type foundobject = found.GetType();
            Type Differenceobject = found.GetType();

            IList<PropertyInfo> incomingprops = new List<PropertyInfo>(incomingobject.GetProperties());
            IList<PropertyInfo> foundprops = new List<PropertyInfo>(foundobject.GetProperties());
            IList<PropertyInfo> differenceprops = new List<PropertyInfo>(Differenceobject.GetProperties());


            foreach (var item in foundprops)
            {
                if (item.Name == "Id")
                    continue;

                differenceprops.FirstOrDefault(x => x.Name == item.Name).SetValue(Difference, item.GetValue(found, null));
            }

            //var config = new MapperConfiguration(cfg => cfg.CreateMap<T, T>());
            //var mapper = config.CreateMapper();

            //Difference = mapper.Map<T>(found); 




            foreach (PropertyInfo prop in incomingprops)
            {
                Object incoimgpropValue = prop.GetValue(incoming, null);
                Object incomingpropName = prop.Name;

                Object foundpropValue = foundprops.FirstOrDefault(x => x.Name == prop.Name).GetValue(found, null);

                if (incoimgpropValue is null)
                    continue;


                if (incomingprops.Any(x => x.Name == $"{incomingpropName}Ql"))
                {
                    int? incoimgpropQlValue = (incomingprops.FirstOrDefault(x => x.Name == $"{incomingpropName}Ql").GetValue(incoming, null) ?? 0) as int?;

                    int? foundpropQlValue = (foundprops.FirstOrDefault(x => x.Name == $"{incomingpropName}Ql").GetValue(found, null) ?? 0) as int?;




                    if (

                        ((incoimgpropQlValue ?? 0) > (foundpropQlValue ?? 0) && !(incoimgpropValue is null)) ||

                        ((incoimgpropQlValue ?? 0) == (foundpropQlValue ?? 0) && !incoimgpropValue.Equals(foundpropValue) && !(incoimgpropValue is null)) ||

                        (foundpropValue is null && !(incoimgpropValue is null))

                        )
                    {
                        //Datetime values
                        if (prop.PropertyType == typeof(DateTime?))
                        {
                            differenceprops.FirstOrDefault(x => x.Name == prop.Name).SetValue(Difference, (DateTime)incoimgpropValue);
                        }

                        //decimal values
                        else if (prop.PropertyType == typeof(Decimal?))
                        {
                            differenceprops.FirstOrDefault(x => x.Name == prop.Name).SetValue(Difference, (Decimal)incoimgpropValue);
                        }

                        //string values
                        else if (prop.PropertyType == typeof(string))
                        {
                            differenceprops.FirstOrDefault(x => x.Name == prop.Name).SetValue(Difference, (string)incoimgpropValue);
                        }


                        else
                        {


                            differenceprops.FirstOrDefault(x => x.Name == prop.Name).SetValue(Difference, incoimgpropValue);
                        }




                        if ((incoimgpropQlValue ?? 0) < (foundpropQlValue ?? 0) && (!foundpropQlValue.HasValue))
                        {
                            differenceprops.FirstOrDefault(x => x.Name == $"{incomingpropName}Ql").SetValue(Difference, 0);
                        }
                        else
                            differenceprops.FirstOrDefault(x => x.Name == $"{incomingpropName}Ql").SetValue(Difference, incoimgpropQlValue);

                        isChanged = true;
                    }



                }

                else if (prop.Name == "PositionCustom")
                {

                    if ((incoimgpropValue)?.ToString().Trim() == (foundpropValue)?.ToString().Trim())
                        continue;
                    differenceprops.FirstOrDefault(x => x.Name == prop.Name).SetValue(Difference, ((string)incoimgpropValue).Trim());
                    isChanged = true;
                }
            }

            if (isChanged)
            {
                differenceprops.FirstOrDefault(x => x.Name == "ValidFrom").SetValue(Difference, incomingprops.FirstOrDefault(x => x.Name == "ValidFrom").GetValue(incoming, null));
                differenceprops.FirstOrDefault(x => x.Name == "ValidTo").SetValue(Difference, incomingprops.FirstOrDefault(x => x.Name == "ValidTo").GetValue(incoming, null));

            }

            diffence = Difference;

            return isChanged;
        }

        private void MergePhysical(PhysicalPerson personIncoming, PhysicalPerson personFound)
        {
            PhysicalPerson dif = new PhysicalPerson();


            if (CompareClientInfo<PhysicalPerson>(personIncoming, personFound, out dif))
            {
                var clientFound = personFound.ClientRef.Clients.First(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
                //var clientIncoming = personIncoming.ClientRef.Clients.First(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
                //MergeClient(clientIncoming, clientFound);


                //deactivate PhysicalPerson
                personFound.ValidTo = DateTime.Now.AddMilliseconds(-4);

                clientFound.ClientRef.PhysicalPeople.Add(dif);
            }
        }

        private void MergeCompany(Company companyIncoming, Company companyFound)
        {

            Company dif = new Company();


            if (CompareClientInfo<Company>(companyIncoming, companyFound, out dif))
            {
                var clientFound = companyFound.ClientRef.Clients.First(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
                //var clientIncoming = personIncoming.ClientRef.Clients.First(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
                //MergeClient(clientIncoming, clientFound);


                //deactiveate
                companyFound.ValidTo = DateTime.Now.AddMilliseconds(-4);

                //create new
                companyFound.ClientRef.Companies.Add(dif);
            }

            //bool createNewVersion = false;
            //Company companyDifference = new Company()
            //{
            //    ClientRef = companyFound.ClientRef,
            //    CompanyName = companyFound.CompanyName,
            //    CompanyNameQl = companyFound.CompanyNameQl,
            //    ValidFrom = companyIncoming.ValidFrom,
            //    ValidTo = companyIncoming.ValidTo
            //};

            //if ((companyIncoming.CompanyNameQl ?? 0) > (companyFound.CompanyNameQl ?? 0) ||
            //    (companyIncoming.CompanyNameQl.HasValue && (companyIncoming.CompanyNameQl == (companyFound.CompanyNameQl ?? 0)) &&
            //    companyIncoming.CompanyName != companyFound.CompanyName))
            //{
            //    companyDifference.CompanyName = companyIncoming.CompanyName;
            //    companyDifference.CompanyNameQl = companyIncoming.CompanyNameQl;
            //    createNewVersion = true;
            //}

            //companyDifference.ValidFrom = companyIncoming.ValidFrom;
            //companyDifference.ValidTo = companyIncoming.ValidTo;

            //if (createNewVersion)
            //{
            //    //deactivate old client
            //    var clientFound = companyFound.ClientRef.Clients.
            //        First(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);
            //    //var clientIncoming = companyIncoming.ClientRef.Clients.
            //    //   First(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now);


            //    //MergeClient(clientIncoming, clientFound);

            //    //deactiveate
            //    companyFound.ValidTo = DateTime.Now.AddMilliseconds(-4);

            //    //create new
            //    companyFound.ClientRef.Companies.Add(companyDifference);
            //}
        }

        private void MergeAddress(Address addressIncoming, Address addressFound, ClientRef foundClientRef)
        {
            //if there is not any address related to this client add new address
            if (addressFound == null && addressIncoming != null)
            {
                //create new
                foundClientRef.Addresses.Add(new Address
                {
                    ClientRef = foundClientRef,
                    CountryId = addressIncoming.CountryId,
                    DistrictOrStreet = addressIncoming.DistrictOrStreet,
                    RegOrCityId = addressIncoming.RegOrCityId,
                    CountryQl = addressIncoming.CountryQl,
                    DistrictOrStreetQl = addressIncoming.DistrictOrStreetQl,
                    RegOrCityQl = addressIncoming.RegOrCityQl,
                    AdditionalInfo = addressIncoming.AdditionalInfo,
                    ValidFrom = addressIncoming.ValidFrom,
                    ValidTo = addressIncoming.ValidTo

                });
            }

            else if (addressIncoming != null && addressFound != null)
            {
                Address addressDifference = new Address
                {
                    AdditionalInfo = addressFound.AdditionalInfo,
                    ClientRef = addressFound.ClientRef,
                    CountryId = addressFound.CountryId,
                    CountryQl = addressFound.CountryQl,
                    DistrictOrStreet = addressFound.DistrictOrStreet,
                    DistrictOrStreetQl = addressFound.DistrictOrStreetQl,
                    RegOrCityId = addressFound.RegOrCityId,
                    RegOrCityQl = addressFound.RegOrCityQl,
                    ValidFrom = addressIncoming.ValidFrom,
                    ValidTo = addressIncoming.ValidTo

                };

                if (((addressIncoming.CountryQl ?? 0) > (addressFound.CountryQl ?? 0)) ||
                    ((addressIncoming.CountryQl ?? 0) == (addressFound.CountryQl ?? 0) &&
                    (addressIncoming.CountryId != addressFound.CountryId)))
                {
                    addressDifference.CountryId = addressIncoming.CountryId;
                    addressDifference.CountryQl = addressIncoming.CountryQl;
                    addressIncoming.CountryQl += 1;
                }

                if (((addressIncoming.DistrictOrStreetQl ?? 0) > (addressFound.DistrictOrStreetQl ?? 0)) ||
                    (((addressIncoming.DistrictOrStreetQl ?? 0) == (addressFound.DistrictOrStreetQl ?? 0)) &&
                    (addressIncoming.DistrictOrStreet != addressFound.DistrictOrStreet)))
                {
                    addressDifference.DistrictOrStreet = addressIncoming.DistrictOrStreet;
                    addressDifference.DistrictOrStreetQl = addressIncoming.DistrictOrStreetQl;
                    addressIncoming.DistrictOrStreetQl += 1;
                }

                if (((addressIncoming.RegOrCityQl ?? 0) > (addressFound.RegOrCityQl ?? 0)) ||
                    (((addressIncoming.RegOrCityQl ?? 0) == (addressFound.RegOrCityQl ?? 0)) &&
                    (addressIncoming.RegOrCityId != addressFound.RegOrCityId)))
                {
                    addressDifference.RegOrCityId = addressIncoming.RegOrCityId;
                    addressDifference.RegOrCityQl = addressIncoming.RegOrCityQl;
                    addressIncoming.RegOrCityQl += 1;
                }

                if (addressIncoming.AdditionalInfo != addressFound.AdditionalInfo)
                {
                    addressDifference.AdditionalInfo = addressIncoming.AdditionalInfo;
                    addressIncoming.RegOrCityQl += 1;
                }


                addressDifference.ValidFrom = addressIncoming.ValidFrom;
                addressDifference.ValidTo = addressIncoming.ValidTo;

                int resAdd = CompareAddress(addressDifference, addressFound);
                if (resAdd >= 0)
                {
                    //deactiveate address
                    addressFound.ValidTo = DateTime.Now.AddMilliseconds(-4);
                    //create new
                    addressFound.ClientRef.Addresses.Add(addressDifference);
                }
            }

        }

        private void MergeDocuments(List<Document> documentsIncoming, List<Document> documentsFound, ClientRef foundClientRef)
        {
            ClientRef clientFound = foundClientRef;

            foreach (var doc in documentsIncoming)
            {
                var diffDoc = documentsFound.FirstOrDefault
                    (x => x.DocumentType == doc.DocumentType);

                if (diffDoc == null)
                {
                    Document newDoc = new Document
                    {
                        DocumentExpireDate = doc.DocumentExpireDate ?? DateTime.MaxValue,
                        DocumentType = doc.DocumentType,
                        DocumentNumber = doc.DocumentNumber,
                        ValidFrom = doc.ValidFrom,
                        ValidTo = doc.ValidTo,
                        ClientRef = clientFound //pay atention !!!
                    };

                    clientFound.Documents.Add(newDoc);
                }
                else if (diffDoc.DocumentNumber != doc.DocumentNumber)
                {
                    diffDoc.ValidTo = DateTime.Now.AddMilliseconds(-4);

                    Document newDoc = new Document
                    {
                        DocumentExpireDate = doc.DocumentExpireDate ?? DateTime.MaxValue,
                        DocumentType = doc.DocumentType,
                        DocumentNumber = doc.DocumentNumber,
                        ValidFrom = doc.ValidFrom,
                        ValidTo = doc.ValidTo,
                        ClientRef = clientFound //pay atention !!!
                    };

                    clientFound.Documents.Add(newDoc);
                }
            }
        }

        private void MergeRelations(List<ClientRelationComp> relationsIncoming, List<ClientRelationComp> relationsFound, ClientRef foundClientRef)
        {
            ClientRef clientFound = foundClientRef;

            foreach (var incomingRel in relationsIncoming)
            {
                var existingRel = relationsFound.FirstOrDefault(x => x.Client2 == incomingRel.Client2);

                if (existingRel != null)
                {
                    existingRel.ValidFrom = DateTime.Now;

                    if (existingRel.RelationId != incomingRel.RelationId)
                    {
                        existingRel.ValidTo = DateTime.Now.AddMilliseconds(-4);
                        AddNewClientRelation(clientFound, incomingRel);
                    }
                }
                else
                {
                    AddNewClientRelation(clientFound, incomingRel);
                }
            }
        }

        private void AddNewClientRelation(ClientRef clientFound, ClientRelationComp incomingRel)
        {
            ClientRelationComp newRel = new ClientRelationComp
            {
                ClientRef = clientFound,
                Client2 = incomingRel.Client2,
                RelationId = incomingRel.RelationId,
                ValidFrom = incomingRel.ValidFrom,
                ValidTo = incomingRel.ValidTo
            };

            clientFound.ClientRelationCompClient1Navigations.Add(newRel);
        }


        private void MergeTags(List<TagComp> incomingTags, List<TagComp> foundTags, ClientRef foundClientRef)
        {
            ClientRef clientFound = foundClientRef;

            List<TagComp> deletedTags = new List<TagComp>();
            List<TagComp> addedTags = new List<TagComp>();

            foreach (var f in foundTags)
            {
                if (incomingTags.FirstOrDefault(x => x.Tag.Name == f.Tag.Name) == null)
                {
                    deletedTags.Add(f);
                }
            }

            foreach (var f in incomingTags)
            {
                if (foundTags.FirstOrDefault(x => x.Tag.Name == f.Tag.Name) == null)
                {
                    addedTags.Add(f);
                }
            }

            //only works for UI
            if (addedTags.Any(x => x.Tag.Name == "FromUI"))
            {
                foreach (var tag in deletedTags)
                {
                    foundTags.FirstOrDefault(x => x.TagId == tag.TagId).ValidTo = DateTime.Now.AddMilliseconds(-4);
                }
                var uiTag = addedTags.FirstOrDefault(x => x.Tag.Name == "FromUI");
                addedTags.Remove(uiTag);
            }



            foreach (var tag in addedTags)
            {
                if (foundTags.FirstOrDefault(x => x.TagId == tag.TagId && x.ValidFrom <= DateTime.Now &&
                x.ValidTo >= DateTime.Now) == null)
                {
                    clientFound.TagComps.Add(new TagComp
                    {
                        ClientRef = clientFound,
                        Tag = tag.Tag,
                        ValidFrom = tag.ValidFrom,
                        ValidTo = tag.ValidTo
                    });
                }


            }


        }

        private void MergeUserComposition(UserClientComp userCompIncoming, ClientRef foundClientRef)
        {
            if (userCompIncoming is null)
            {
                return;
            }

            if (_db.UserClientComps.Any(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now
                                               && x.Inn == foundClientRef.Inn
                                               && x.LobOid == userCompIncoming.LobOid && x.ULogonName == userCompIncoming.ULogonName))
            {
                return;
            }

            var list = _db.UserClientComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now
                                               && x.Inn == foundClientRef.Inn
                                               && x.LobOid == userCompIncoming.LobOid).ToList();

            if (list != null && list.Count() > 0)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list[i].ULogonName != userCompIncoming.ULogonName)
                    {
                        list[i].ValidTo = DateTime.Now;
                    }
                }
            }

            foundClientRef.UserClientComps.Add(
               new UserClientComp
               {
                   ClientRef = foundClientRef,
                   ULogonName = userCompIncoming.ULogonName,
                   LobOid = userCompIncoming.LobOid,
                   ValidFrom = DateTime.Now.AddMilliseconds(3),
                   ValidTo = DateTime.MaxValue
               });
        }

        private void MergeContacts(List<ClientContactInfoComp> incomingContacts, List<ClientContactInfoComp> foundContacts, ClientRef foundClientRef)
        {
            ClientRef clientFound = foundClientRef;

            foreach (var contactComp in incomingContacts)
            {
                if (contactComp.ContactInfo is null)
                    continue;

                var contact = _db.ContactInfos.
                    FirstOrDefault(x => x.Type == contactComp.ContactInfo.Type && x.Value == contactComp.ContactInfo.Value);

                if (contact == null)
                {
                    contact = new ContactInfo
                    {
                        Type = contactComp.ContactInfo.Type,
                        Value = contactComp.ContactInfo.Value

                    };

                    _db.ContactInfos.Add(contact);
                }


                //bu sualdi. lazimdirmi?
                //CallHistory callHistory = new CallHistory()
                //{
                //    CallTimestamp = DateTime.Now,
                //    ContactInfo = contact,
                //    ResponsibleNumner = contact.Value
                //};
                //_db.CallHistories.Add(callHistory);


                //eger contact bu cliente baglanmayibsa contacta client e bagla. eks halda baglama
                //cunki en axirda ola biler ki, aktiv client clientNewVersion olsun,
                //ve neticede biz clientNewVersion ozu il merge etmis olaq
                if (!foundContacts.Any(x => x.ContactInfoId == contact.Id))
                {
                    ClientContactInfoComp clientContactInfoComp = new ClientContactInfoComp
                    {
                        ContactInfo = contact,
                        Point = 1,
                        ValidFrom = contactComp.ValidFrom,
                        ValidTo = contactComp.ValidTo,
                        ClientRef = clientFound // pay atention !!!
                    };

                    clientFound.ClientContactInfoComps.Add(clientContactInfoComp);
                }

                MergeComments(contactComp.ContactInfo.CommentComps.Where(x => x.ContactId == null).ToList(), clientFound.CommentComps.ToList(), foundClientRef);
            }
        }

        private void MergeComments(List<CommentComp> incomingComments, List<CommentComp> foundComments, ClientRef foundClientRef)
        {
            ClientRef clientFound = foundClientRef;

            foreach (var comm in incomingComments)
            {
                var diffComm = foundComments.
                    FirstOrDefault(x => x.Comment.CreateTimestamp == comm.Comment.CreateTimestamp
                   && x.Comment.Text == comm.Comment.Text)?.Comment;

                if (diffComm == null)
                {
                    diffComm = new Comment
                    {
                        Creator = comm.Comment.Creator,
                        Text = comm.Comment.Text,
                        CreateTimestamp = comm.Comment.CreateTimestamp,
                        FullName = comm.Comment.FullName
                    };

                    _db.Comments.Add(diffComm);
                }

                if (!foundComments.Any(x => x.CommentId == diffComm.Id))
                {
                    CommentComp commentComp = new CommentComp()
                    {
                        Comment = diffComm,
                        ContactInfo = null,
                        ClientRef = clientFound
                    };

                    clientFound.CommentComps.Add(commentComp);
                }
            }
        }

        private void MergeAssets(List<Asset> incomingAssets, ClientRef foundClientRef)
        {
            ClientRef clientFound = foundClientRef;
            AssetDetails assetDetails = null;
            foreach (var ass in incomingAssets)
            {
                Asset assetComp = null;
                var diffasset = _db.Assets.
                           Where(x => x.AssetInfo == ass.AssetInfo).OrderByDescending(x => x.ValidFrom).FirstOrDefault();

                // if there is not any asset detail with the params given, then just insert it
                // do not insert dublicate asset detail
                if (diffasset == null)
                {

                    assetDetails = new AssetDetails
                    {
                        AssetDescription = ass.AssetDetail.AssetDescription,
                        AssetInfo = ass.AssetDetail.AssetInfo,
                        InsuredObjectGuid = ass.AssetDetail.InsuredObjectGuid,
                        AssetType = ass.AssetDetail.AssetType,
                    };

                    _db.AssetDetails.Add(assetDetails);

                }

                // if there is asset in db with the given asset info, BUT inn is different, then
                // insert to only assets_composition  (just add 1 new row and change existing row to expired!
                // and give appropriate asset_details to asset_composition

                else
                {
                    if (diffasset.Inn != foundClientRef.Inn)
                    {
                        diffasset.ValidTo = DateTime.Now;
                        assetDetails = _db.AssetDetails.Where(ad => ad.Id == diffasset.AssetDetailId).FirstOrDefault();
                    }
                }

                if (assetDetails != null)
                {
                    assetComp = new Asset()
                    {
                        AssetDetail = assetDetails,
                        ClientRef = clientFound,
                        ValidFrom = DateTime.Now,
                        ValidTo = DateTime.MaxValue,
                        Inn = foundClientRef.Inn,
                        AssetInfo = ass.AssetInfo
                    };

                    clientFound.Assets.Add(assetComp);
                }
            }
        }

        /// <summary>
        /// CreateLeadObject
        /// </summary>
        /// <param name="clientIncoming"></param>
        /// <param name="clientFound"></param>
        public void CreateLeadObject(ClientRef clientIncoming, ClientRef clientFound)
        {
            List<LeadObject> incomingLeads = clientIncoming.LeadObjects.Where(x => x.ValidFrom.Date <= DateTime.Now.Date && x.ValidTo.Date >= DateTime.Now.Date).ToList();

            List<LeadObject> existingLead = clientFound.LeadObjects.Where(x => x.ValidFrom.Date <= DateTime.Now.Date && x.ValidTo.Date >= DateTime.Now.Date).ToList();

            MergeLeadObject(incomingLeads, existingLead, clientFound);
        }
        /// <summary>
        /// MergeLeadObject
        /// </summary>
        /// <param name="incomingLead"></param>
        /// <param name="existingLead"></param>
        /// <param name="clientRef"></param>
        public void MergeLeadObject(List<LeadObject> incomingLeads, List<LeadObject> existingLeads, ClientRef clientRef)
        {
            if (!incomingLeads.Any()) return;

            foreach (var incomingLead in incomingLeads)
            {
                var existingLead = existingLeads.FirstOrDefault(x => x.UniqueKey == incomingLead.UniqueKey);

                if (existingLead != null)
                    existingLead.ObejctId = incomingLead.ObejctId;
                else
                {
                    clientRef.LeadObjects.Add(new LeadObject
                    {
                        ClientRef = incomingLead.ClientRef,
                        CampaignName = incomingLead.CampaignName,
                        UniqueKey = incomingLead.UniqueKey,
                        ValidFrom = incomingLead.ValidFrom,
                        ValidTo = incomingLead.ValidTo,
                        DiscountPercentage = incomingLead.DiscountPercentage,
                        UserGuid = incomingLead.UserGuid,
                        Payload = incomingLead.Payload,
                        ObejctId = incomingLead.ObejctId
                    });
                }
            }
        }
    }

}

