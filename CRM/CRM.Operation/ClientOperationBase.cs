using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Enums;
using CRM.Operation.Models.RequestModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class ClientOperationBase : BusinessOperation<UpdateMergeModel>
    {
        private CRMDbContext _db;
        public ClientRef ClientRef;

        public ClientOperationBase(CRMDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
        protected override void Prepare()
        {
            ClientRef = MapToClient(Parameters.IncomingClient);
        }
        protected override void DoExecute()
        {
            throw new NotImplementedException();
        }

        protected override void DoFinalize()
        {
            if (!Result.Success)
                return;

            int INN = ClientRef.Inn == 0 ? Parameters.FoundClient.Inn : ClientRef.Inn;

            int suspensiospoint = 5;
            int suspensioscount = 10;
            int suspensiosthreshold = -10;
            var point = _db.Codes.Where(x => x.TypeOid == (int)CodeTypeEnum.DBO_POINT_REASON).ToList();

            foreach (var no in Parameters.IncomingClient.ContactsInfo)
            {
                bool issuspensios = false;
                var allothercomposition = _db.ClientContactInfoComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now
                                            && x.ContactInfo.Value == no.Value && x.ContactInfo.Type == no.Type && x.Inn != INN);

                if (allothercomposition.Select(x => x.Inn).Distinct().Count() >= suspensioscount)
                {
                    issuspensios = true;
                    foreach (var item in allothercomposition)
                    {
                        item.Point = ((item.Point ?? 0) - suspensiospoint); //if this contact info repeated more than 10 client it means it is suspicious case and decrease those point 5 
                        if (item.Point < suspensiosthreshold) // in this point we disable that composition in order to give chance to that contact info to be more surely link to client
                            item.ValidTo = DateTime.Now;
                    }
                }

                var d = _db.ClientContactInfoComps.FirstOrDefault(x => x.ContactInfo.Value == no.Value && x.ContactInfo.Type == no.Type && x.Inn == INN);

                int? addPoint = null;
                if (no.Reason.HasValue)
                {
                    var p = point.FirstOrDefault(x => x.CodeOid == no.Reason);
                    if (p != null)
                    {
                        addPoint = Convert.ToInt32(p.Value);
                        d.ReasonId = no.Reason.Value;
                    }
                }
                if (d != null)
                {
                    if (!d.Point.HasValue && addPoint.HasValue)
                        d.Point = 0;

                    d.Point += (issuspensios ? -suspensiospoint : (addPoint ?? 0));
                }





                _db.SaveChanges();

            }
        }


        private ClientRef MapToClient(ClientDataPayload payload)
        {
            DateTime validFrom = DateTime.Now;
            DateTime validTo = DateTime.MaxValue;
            Position position = null;
            Tag tag = null;

            ClientRef clientRef = new ClientRef();

            //documents
            if (payload.Documents != null)
            {
                foreach (var doc in payload.Documents)
                {
                    if (!string.IsNullOrEmpty(doc.DocumentNumber))
                    {
                        Document document = new Document()
                        {
                            DocumentExpireDate = doc.DocumentExpireDate,
                            DocumentNumber = doc.DocumentNumber,
                            DocumentType = doc.DocumentType,
                            ValidFrom = validFrom,
                            ValidTo = validTo,
                            ClientRef = clientRef
                        };

                        clientRef.Documents.Add(document);
                    }
                }
            }

            //added queueid as document so we can identify which queue is used to create this client
            Document queueDoc = new Document()
            {
                DocumentExpireDate = validTo,
                DocumentNumber = payload.QueueId.ToString(),
                DocumentType = (int)DocumentTypeEnum.Queue,
                ValidFrom = validFrom,
                ValidTo = validTo,
                ClientRef = clientRef
            };
            clientRef.Documents.Add(queueDoc);


            //contactsInfo

            if (payload.ContactsInfo != null)
            {

                var points = _db.Codes.Where(x => x.TypeOid == (int)CodeTypeEnum.DBO_POINT_REASON).ToList();

                foreach (var info in payload.ContactsInfo)
                {
                    var contactInfo = _db.ContactInfos.
                    FirstOrDefault(x => x.Type == info.Type && x.Value == info.Value);

                    if (contactInfo == null && !string.IsNullOrEmpty(info.Value))
                    {
                        contactInfo = new ContactInfo()
                        {
                            Type = info.Type,
                            Value = info.Value
                        };
                    }

                    int? value = 1;
                    if (info.Reason.HasValue)
                    {
                        Code code = points.FirstOrDefault(x => x.CodeOid == info.Reason.Value);
                        value = code == null ? 1 : Convert.ToInt32(code.Value);
                    }
                    if (contactInfo != null)
                    {
                        ClientContactInfoComp infoComp = new ClientContactInfoComp()
                        {
                            ContactInfo = contactInfo,
                            ReasonId = info.Reason,
                            Point = value,
                            ValidFrom = validFrom,
                            ValidTo = validTo,
                            ClientRef = clientRef
                        };
                        clientRef.ClientContactInfoComps.Add(infoComp);
                    }


                    if (info.ContactComments != null)
                    {
                        foreach (var comm in info.ContactComments)
                        {

                            Comment comment = new Comment
                            {
                                Creator = comm.Creator,
                                Text = comm.Text,
                                CreateTimestamp = DateTime.Now,
                                FullName = comm.FullName
                            };


                            CommentComp commentComp = new CommentComp()
                            {
                                Comment = comment,
                                ContactInfo = contactInfo,
                                ClientRef = clientRef
                            };

                            clientRef.CommentComps.Add(commentComp);
                        }
                    }

                    if (info.Calls != null)
                    {
                        foreach (var call in info.Calls)
                        {

                            CallHistory callHistory = new CallHistory
                            {
                                Direction = call.Direction,
                                CallTimestamp = call.CallTimestamp,
                                ResponsibleNumner = call.ResponsibleNumber
                            };
                            info.Calls.Add(call);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(payload.OperationDescription) || !string.IsNullOrEmpty(payload.OperationType))
            {
                int? inn = GetInn(payload);


                ClientHistory clientHistory = new ClientHistory()
                {
                    Inn = inn,
                    JsonResult = JsonConvert.SerializeObject(payload),
                    OperationDescription = payload.OperationDescription,
                    OperationType = payload.OperationType,
                    RegisteredDateTime = DateTime.Now
                };

                clientRef.ClientHistories.Add(clientHistory);

            }

            if (!string.IsNullOrEmpty(payload.ResponsiblePerson))
            {
                if (payload.Source != 3)
                {

                    PhysicalPerson personDB = null;
                    Document docDB = null;
                    int? inn = GetInn(payload);


                    UserClientComp userClientComp = new UserClientComp()
                    {
                        ULogonName = payload.ResponsiblePerson,
                        ClientRef = clientRef,
                        Inn = inn,
                        LobOid = payload.LobOid,
                        ValidFrom = DateTime.Now,
                        ValidTo = DateTime.MaxValue
                    };


                    clientRef.UserClientComps.Add(userClientComp);
                }
            }

            //relations
            if (payload.Relations != null)
            {
                int? inn = null;
                foreach (var rel in payload.Relations)
                {
                    if (rel.ClientINN.HasValue)
                    {
                        inn = rel.ClientINN.Value;
                    }
                    else if (rel.DocumentType == DocumentTypeEnum.IdCard) //IDCard
                    {
                        inn = _db.PhysicalPeople.FirstOrDefault(x => x.Pin == rel.Code
                        && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now)?.Inn;
                    }
                    else
                    {
                        inn = _db.Documents.FirstOrDefault(x => x.DocumentType == (int)rel.DocumentType
                        && x.DocumentNumber == rel.Code
                        && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now)?.Inn;
                    }

                    if (!inn.HasValue)
                    {
                        Result.AddError($"Relational client with document type {rel.DocumentType} and code {rel.Code} not found");
                        return null;
                    }

                    ClientRelationComp relation = new ClientRelationComp()
                    {
                        ClientRef = clientRef,
                        Client2 = inn.Value,
                        RelationId = rel.RelationType,
                        ValidFrom = validFrom,
                        ValidTo = validTo
                    };


                    clientRef.ClientRelationCompClient1Navigations.Add(relation);
                }
            }

            //client
            Client client = new Client
            {
                ClientType = (int)payload.ClientType,
                ClientRef = clientRef,
                ValidFrom = validFrom,
                ValidTo = validTo
            };
            clientRef.Clients.Add(client);

            if (!string.IsNullOrEmpty(payload.PositionCustom))
            {
                var positionName = _db.Positions.FirstOrDefault(x => x.PositionName.CompareTo(payload.PositionCustom) == 0)?.PositionName;
                if (string.IsNullOrEmpty(positionName))
                {
                    position = new Position
                    {
                        PositionName = payload.PositionCustom,
                        ValidFrom = validFrom,
                        ValidTo = validTo
                    };
                    _db.Positions.Add(position);
                }
            }
            else if (payload.Position != null)
            {
                position = _db.Positions.FirstOrDefault(x => x.Id == payload.Position);

                if (position == null)
                {
                    Result.AddError("Position not found");
                    return null;
                }
            }



            //physical legal client
            var qualityLevels = _db.Codes.Where(x => x.CodeOid == (int)payload.Source).ToList();

            if (payload.ClientType == ClientType.Pyhsical)
            {
                PhysicalPerson pPerson = new PhysicalPerson
                {
                    BirthDate = payload.BirthDate,
                    Gender = payload.Gender,
                    FatherName = payload.FatherName,
                    FirstName = payload.FirstName,
                    FullName = payload.FullName,
                    LastName = payload.LastName,
                    MonthlyIncome = payload.MonthlyIncome,
                    Pin = payload.PinNumber?.Trim(),
                    ImageBase64 = payload.ImageBase64,
                    Position = position,
                    PositionCustom = payload.PositionCustom,
                    ValidFrom = validFrom,
                    ValidTo = validTo,
                    ClientRef = clientRef,
                    FirstNameQl = !string.IsNullOrEmpty(payload.FirstName) ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_PHYSICALPERSON_FIRST_NAME_QL)?.Value) : default(int?),
                    LastNameQl = !string.IsNullOrEmpty(payload.LastName) ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_PHYSICALPERSON_LAST_NAME_QL)?.Value) : default(int?),
                    FatherNameQl = !string.IsNullOrEmpty(payload.FatherName) ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_PHYSICALPERSON_FATHER_NAME_QL)?.Value) : default(int?),
                    FullNameQl = !string.IsNullOrEmpty(payload.FullName) ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_PHYSICALPERSON_FULL_NAME_QL)?.Value) : default(int?),
                    ImageBase64Ql = !string.IsNullOrEmpty(payload.ImageBase64) ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_PHYSICALPERSON_IMAGE_QL)?.Value) : default(int?),
                    PinQl = !string.IsNullOrEmpty(payload.PinNumber) ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_PHYSICALPERSON_PIN_QL)?.Value) : default(int?),
                    BirthDateQl = payload.BirthDate.HasValue ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_PHYSICALPERSON_BIRTH_DATE_QL)?.Value) : default(int?),
                    PositionQl = position != null ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_PHYSICALPERSON_POSITION_QL)?.Value) : default(int?),
                    MonthlyIncomeQl = payload.MonthlyIncome.HasValue ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_PHYSICALPERSON_MONTHLY_INCOME_QL)?.Value) : default(int?)
                };



                clientRef.PhysicalPeople.Add(pPerson);
            }
            else if (payload.ClientType == ClientType.Company)
            {

                Company company = new Company
                {
                    CompanyName = payload.CompanyName,
                    ValidFrom = validFrom,
                    ValidTo = validTo,
                    ClientRef = clientRef,
                    CompanyNameQl = !string.IsNullOrEmpty(payload.CompanyName) ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == 7)?.Value) : default(int?)
                };
                clientRef.Companies.Add(company);
            }
            else
            {

            }
            //client Comment
            if (payload.ClientComments != null)
            {
                foreach (var comm in payload.ClientComments)
                {
                    Comment clientComment = new Comment()
                    {
                        Text = comm.Text,
                        Creator = comm.Creator,
                        CreateTimestamp = DateTime.Now,
                        FullName = comm.FullName
                    };


                    CommentComp clientCommentComp = new CommentComp()
                    {
                        Comment = clientComment,
                        ContactInfo = null,
                        ClientRef = clientRef
                    };
                    clientRef.CommentComps.Add(clientCommentComp);
                }
            }

            if (payload.Address != null)
            {
                //client Address
                Address address = new Address()
                {
                    AdditionalInfo = payload.Address.additionalInfo,
                    CountryId = payload.Address.countryId,
                    DistrictOrStreet = payload.Address.districtOrStreet,
                    RegOrCityId = payload.Address.regOrCityId,
                    ClientRef = clientRef,
                    CountryQl = payload.Address.countryId.HasValue ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_ADDRESS_COUNTRY_QL)?.Value) : default(int?),
                    RegOrCityQl = payload.Address.regOrCityId.HasValue ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_ADDRESS_REGORCITY_QL)?.Value) : default(int?),
                    DistrictOrStreetQl = !string.IsNullOrEmpty(payload.Address.districtOrStreet) ? Convert.ToInt32(qualityLevels.FirstOrDefault(x => x.TypeOid == (int)CodeTypeEnum.DBO_ADDRESS_DISTRICTORSTREET_QL)?.Value) : default(int?),
                    ValidFrom = validFrom,
                    ValidTo = validTo
                };
                clientRef.Addresses.Add(address);
            }

            if (payload?.LeadObjects != null)
            {
                var leadObjects = payload.LeadObjects.Select(item => CreateLeadObject(item, clientRef));

                foreach (var leadObject in leadObjects)
                {
                    clientRef.LeadObjects.Add(leadObject);
                }
            }


            if (payload.Assets != null)
            {
                foreach (var item in payload.Assets)
                {
                    AssetDetails assetDetails = new AssetDetails()
                    {
                        AssetDescription = item.AssetDescription,
                        AssetInfo = item.AssetInfo,
                        AssetType = item.AssetType,
                        InsuredObjectGuid = item.InsuredObjectGuid
                    };

                    if (assetDetails != null)
                    {
                        Asset asset = new Asset()
                        {
                            AssetDetail = assetDetails,
                            ValidFrom = validFrom,
                            ValidTo = validTo,
                            ClientRef = clientRef,
                            AssetInfo = assetDetails.AssetInfo
                        };

                        clientRef.Assets.Add(asset);
                    }

                }
            }

            if (payload.Tags != null)
            {

                foreach (var t in payload.Tags)
                {
                    tag = _db.Tags.FirstOrDefault(x => x.Id == t.Id);

                    if (tag == null)
                    {
                        Result.AddError("Tag not found");
                        return null;
                    }

                    TagComp tagComp = new TagComp()
                    {
                        ClientRef = clientRef,
                        ValidFrom = validFrom,
                        ValidTo = validTo,
                        Tag = tag
                    };



                    clientRef.TagComps.Add(tagComp);
                }

                if (payload.Source == 3)//if source  CRM UI
                    clientRef.TagComps.Add(new TagComp { Tag = new Tag { Name = "FromUI" } });
            }

            return clientRef;
        }

        private int? GetInn(ClientDataPayload payload)
        {
            int? inn = 0;
            if (payload.INN == null || payload.INN == 0)
            {
                if (payload.ClientType == ClientType.Pyhsical)
                {
                    var personDB = _db.PhysicalPeople.FirstOrDefault(x => x.Pin == payload.PinNumber
                                                              && DateTime.Now <= x.ValidTo && DateTime.Now >= x.ValidFrom);
                    if (personDB != null)
                    {
                        inn = personDB.Inn;
                    }
                }
                else
                {
                    var docDB = _db.Documents.FirstOrDefault(x => x.DocumentNumber == payload.Documents.FirstOrDefault().DocumentNumber && x.DocumentType == (int)DocumentTypeEnum.Tin
                                                              && DateTime.Now <= x.ValidTo && DateTime.Now >= x.ValidFrom);

                    if (docDB != null)
                    {
                        inn = docDB.Inn;
                    }
                }
            }
            else
            {
                inn = payload.INN;
            }

            return inn;
        }


        private LeadObject CreateLeadObject(LeadObjectData item, ClientRef clientRef)
        {
            Guid.TryParse(item.UserGuid, out var parsedGuid); 

            return new LeadObject
            {
                CampaignName = item.CampaignName,
                UniqueKey = item.UniqueKey,
                DiscountPercentage = item.DiscountPercentage,
                ClientRef = clientRef,
                ValidFrom = item.ValidFrom,
                ValidTo = item.ValidTo,
                UserGuid = parsedGuid, 
                Payload = item.Payload,
                ObejctId = item.ObjectId
            };
        }

    }
}
