using CRM.Data.Database;
using CRM.Operation.Queue;
using CRM.Operation.Models.RequestModels;
using Hangfire;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zircon.Core.OperationModel;
using CRM.Operation.Enums;
using System.Linq;

namespace CRM.Operation
{
    public class ClientQueuePutOperation : BusinessOperation<ClientInfoContract>
    {
        CRMDbContext _db;
        public long QueueId { get; set; }

        public ClientQueuePutOperation(CRMDbContext db) : base(db)
        {
            _db = db;
        }

        protected override void Prepare()
        {
            base.Prepare();
        }

        protected override void DoExecute()
        {
            foreach (var clientInfo in Parameters.Clients)
            {
                ClientDataPayload payload = MapToPayload(clientInfo, Parameters.Source, Parameters.ResponsiblePerson, Parameters.LobOid);
                QueuePutOperation<ClientDataPayload> queuePutOperation = new QueuePutOperation<ClientDataPayload>(_db);
                queuePutOperation.Execute(payload);

                if (queuePutOperation.Result.Success)
                    Result.AddSuccess("Sorğunuz emal edilməsi üçün növbəyə alındı. Nömrə - " + queuePutOperation.Item.Id);
            }
        }

        private ClientDataPayload MapToPayload(ClientContract clientContract, int sourceType, string responsiblePerson, int? lobOid)
        {
            List<DocumentData> documentsData = null;
            List<ContactInfoData> contactsInfoData = null;
            List<RelationData> relationsData = null;
            List<TagData> tagsData = null;
            List<CommentData> comments = null;
            List<AssetData> assets = null;
            List<LeadObjectData> leadObjects = null;


            if (clientContract.Documents != null)
            {
                documentsData = new List<DocumentData>();
                foreach (var doc in clientContract.Documents)
                {
                    DocumentData document = new DocumentData()
                    {
                        DocumentExpireDate = doc.DocumentExpireDate,
                        DocumentNumber = doc.DocumentNumber,
                        DocumentType = doc.DocumentType
                    };

                    documentsData.Add(document);
                }
            }


            if (clientContract.ContactsInfo != null)
            {
                contactsInfoData = new List<ContactInfoData>();
                List<CommentData> commentsData = null;
                List<CallData> callsData = null;
                foreach (var info in clientContract.ContactsInfo)
                {
                    if (info.Type == null || info.Type == 0 || info.Value == null)
                        continue;

                    if (info.ContactComments != null)
                    {
                        commentsData = new List<CommentData>();
                        foreach (var comment in info.ContactComments)
                        {

                            CommentData commentData = new CommentData()
                            {
                                Creator = comment.Creator,
                                Text = comment.Text,
                                FullName = comment.FullName
                            };
                            commentsData.Add(commentData);
                        }
                    }

                    if (info.Calls != null)
                    {
                        callsData = new List<CallData>();
                        foreach (var call in info.Calls)
                        {

                            CallData callData = new CallData()
                            {
                                CallTimestamp = call.CallTimestamp,
                                Direction = call.Direction,
                                ResponsibleNumber = call.ResponsibleNumber
                            };
                            callsData.Add(callData);
                        }
                    }

                    ContactInfoData contactInfoData = new ContactInfoData()
                    {
                        Type = info.Type,
                        Value = info.Value,
                        Reason = info.Reason,
                        ContactComments = commentsData
                    };


                    contactsInfoData.Add(contactInfoData);
                }
            }


            if (clientContract.Relations != null)
            {
                relationsData = new List<RelationData>();
                foreach (var rel in clientContract.Relations)
                {
                    RelationData relationData = new RelationData()
                    {
                        ClientINN = rel.ClientINN,
                        RelationType = rel.RelationType,
                        Code = rel.Code,
                        DocumentType = (DocumentTypeEnum)rel.DocumentType
                    };

                    relationsData.Add(relationData);
                }
            }


            if (clientContract.Tags != null)
            {
                tagsData = new List<TagData>();
                foreach (var tagContract in clientContract.Tags)
                {
                    TagData tag = new TagData()
                    {
                        Id = tagContract.Id,
                        Name = tagContract.Name
                    };

                    tagsData.Add(tag);
                }
            }


            if (clientContract.ClientComments != null)
            {
                comments = new List<CommentData>();
                foreach (var com in clientContract.ClientComments)
                {
                    comments.Add(
                     new CommentData()
                     {
                         Creator = com.Creator,
                         Text = com.Text,
                         FullName = com.FullName
                     });
                }
            }

            if (clientContract.Assets != null)
            {
                assets = new List<AssetData>();
                foreach (var ass in clientContract.Assets)
                {
                    assets.Add(
                     new AssetData()
                     {
                         InsuredObjectGuid = ass.InsuredObjectGuid,
                         AssetType = ass.AssetType,
                         AssetDescription = ass.AssetDescription,
                         AssetInfo = ass.AssetInfo,
                         ValidTo = DateTime.MaxValue,
                         ValidFrom = DateTime.Now

                     });
                }
            }



            AddressData address = null;
            if (clientContract.Address != null &&
                (clientContract.Address.CountryId != null ||
                 clientContract.Address.RegOrCityId != null ||
                 clientContract.Address.DistrictOrStreet != null ||
                 clientContract.Address.AdditionalInfo != null
                ))
            {
                address = new AddressData
                {
                    additionalInfo = clientContract.Address.AdditionalInfo,
                    countryId = clientContract.Address.CountryId,
                    districtOrStreet = clientContract.Address.DistrictOrStreet,
                    regOrCityId = clientContract.Address.RegOrCityId
                };
            }



            if (clientContract?.LeadObjects?.Any() == true)
            {
                leadObjects = clientContract.LeadObjects
                    .Select(lead =>
                    {
                        return new LeadObjectData
                        {
                            CampaignName = lead.CampaignName,
                            DiscountPercentage = lead.DiscountPercentage,
                            Payload = lead.Payload,
                            UserGuid = lead.UserGuid,
                            UniqueKey = lead.UniqueKey,
                            ObjectId = lead.ObjectId,
                            ValidTo = DateTime.Parse(lead.ValidTo),
                            ValidFrom = DateTime.Parse(lead.ValidFrom)
                        };
                    })
                    .Where(leadData => leadData != null)
                    .ToList();
            }

            return new ClientDataPayload()
            {
                Source = sourceType,
                ResponsiblePerson = responsiblePerson,
                INN = clientContract.INN,
                BirthDate = clientContract.BirthDate,
                Gender = clientContract.Gender,
                ClientType = clientContract.ClientType,
                CompanyName = clientContract.CompanyName,
                FatherName = clientContract.FatherName,
                LastName = clientContract.LastName,
                FirstName = clientContract.FirstName,
                FullName = clientContract.FullName,
                MonthlyIncome = clientContract.MonthlyIncome,
                PinNumber = clientContract.PinNumber,
                Position = clientContract.Position,
                PositionCustom = clientContract.PositionCustom,
                Recipient = "CRM",
                ClientComments = comments,
                Address = address,
                Relations = relationsData,
                Documents = documentsData,
                ContactsInfo = contactsInfoData,
                Tags = tagsData,
                OperationDescription = clientContract.OperationDescription,
                OperationType = clientContract.OperationType,
                Assets = assets,
                LobOid = lobOid,
                ImageBase64 = clientContract.ImageBase64,
                LeadObjects = leadObjects
            };

        }

        protected override void DoFinalize()
        {
            base.DoFinalize();
            if (Result.Success)
                RecurringJob.Trigger("JobManagerJobManager");
        }
    }
}
