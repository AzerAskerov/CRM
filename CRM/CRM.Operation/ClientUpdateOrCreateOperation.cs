using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Queue;
using CRM.Operation.Normalize;
using CRM.Operation.Terror;
using Zircon.Core.Extensions;

namespace CRM.Operation
{
    public class ClientUpdateOrCreateOperation : QueueItemHandlerBase<ClientDataPayload>
    {
        CRMDbContext _db;
        private int? Inn;

        public ClientUpdateOrCreateOperation(CRMDbContext db) : base(db)
        {
            _db = db;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Parameters = Parameters.Normalize<ClientDataPayload>(_db);
            Result.Merge(ModelNormalizerExtentions._result);
        }

        protected override void Prepare()
        {
            base.Prepare();
            FailureType = Enums.QueueHandlerFailureType.ForceRetry;
            if (Parameters.INN == 0)
                Parameters.INN = null;
        }

        protected override void DoExecute()
      {
            if (!string.IsNullOrEmpty(Parameters.PinNumber))
            {
                Parameters.ClientType = ClientType.Pyhsical;
                var personDB = _db.PhysicalPeople.FirstOrDefault(x => x.Pin == Parameters.PinNumber
                                                                      && DateTime.Now <= x.ValidTo && DateTime.Now >= x.ValidFrom);

                if (personDB != null)
                {
                    var query = _db.ClientRefs.Where(x => x.Inn == personDB.Inn);
                    query.Include(x => x.Clients.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == personDB.Inn)).Load();
                    query.Include(x => x.UserClientComps.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == personDB.Inn)).Load();
                    query.Include(x => x.PhysicalPeople.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == personDB.Inn)).Load();
                    query.Include(x => x.Companies.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == personDB.Inn)).Load();
                    query.Include(x => x.Documents.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == personDB.Inn)).Load();
                    query.Include(x => x.ClientContactInfoComps.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == personDB.Inn))
                        .ThenInclude(x => x.ContactInfo).Load();
                    query.Include(x => x.Addresses.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == personDB.Inn)).Load();
                    query.Include(x => x.LeadObjects.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == personDB.Inn)).Load();
                    query.Include(x => x.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.Inn == personDB.Inn))
                        .ThenInclude(x => x.Tag).Load();
                    ClientRef clientRef  =   query.Include(x => x.ClientRelationCompClient1Navigations.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.Client1 == personDB.Inn))
                 .First();


                    SendClientsToUpdate(clientRef);
                }
                else
                {
                    bool clientFoundWithDocument = LookForDocument();
                    if (!Result.Success)
                        return;
                    if (!clientFoundWithDocument)
                        SendClientToCreate();
                }
            }
            else if (Parameters.INN.HasValue && Parameters.INN.Value != 0)
            {
                var clientRef = _db.ClientRefs.Where(x => x.Inn == Parameters.INN.Value)
                    .Include(x => x.Clients.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now))
                    .Include(x => x.UserClientComps.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now))
                    .Include(x => x.PhysicalPeople.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now))
                    .Include(x => x.Companies.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now))
                    .Include(x => x.LeadObjects.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now))
                    .Include(x => x.Documents.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now))
                    .Include(x => x.Addresses.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now))
                    .Include(x => x.ClientContactInfoComps.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now))
                    .Include(x => x.ClientContactInfoComps.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now))
                    .ThenInclude(x => x.ContactInfo)
                    .Include(x => x.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now))
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.ClientRelationCompClient1Navigations.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now))
                    .First();

                SendClientsToUpdate(clientRef);
            }

            else if(Parameters.Documents.Any() || Parameters.ContactsInfo.Any())
            {
                if (!Parameters.Documents.Any())
                {
                    if (Parameters.ContactsInfo.Any())
                    {
                        LookContactInfo();
                    }
                }
                else
                {
                    bool clientFoundWithDocument = LookForDocument();

                    if (!clientFoundWithDocument)
                    {
                        SendClientToCreate();
                    }

                    if (!clientFoundWithDocument && Parameters.ContactsInfo.Any())
                    {
                        LookContactInfo();
                    }

                  
                }
            }
            else
                
                SendClientToCreate();
        }

        protected override void DoFinalize()
        {
            if (Result.Success && Inn.HasValue && Parameters.ClientType==ClientType.Pyhsical)
            {
                var counterTerrorismOpt = new CounterTerrorismOperation(_db);
                Parameters.INN = Inn;
                counterTerrorismOpt.Execute(Parameters);
            }
        }

        private void LookContactInfo()
        {
            int? clientINN = null;
            foreach (var contactParam in Parameters.ContactsInfo)
            {
                var contcatInfosDB = _db.ContactInfos.FirstOrDefault(x => contactParam.Value == x.Value);

                if (contcatInfosDB != null)
                {
                    var contactInfoComp = _db.ClientContactInfoComps.FirstOrDefault(x => contcatInfosDB.Id == x.ContactInfoId
                                                                                         && DateTime.Now <= x.ValidTo && DateTime.Now >= x.ValidFrom);

                    foreach (var comment in contactParam.ContactComments)
                    {
                        var newComment = new Comment()
                        {
                            Text = comment.Text,
                            CreateTimestamp = DateTime.Now,
                            Creator = Parameters.ResponsiblePerson,
                            FullName = comment.FullName
                        };
                        _db.Comments.Add(newComment);


                        _db.CommentComps.Add(new CommentComp()
                        {
                            Comment = newComment,
                            Inn = contactInfoComp.Inn,
                            ContactId = contcatInfosDB.Id
                        });
                    }
                }
                else
                {
                    if (clientINN == null)
                        clientINN = SendClientToCreate();
                }
            }

            _db.SaveChanges();
        }

        private bool LookForDocument()
        {
            bool clientFoundWithDocument = false;
            foreach (var document in Parameters.Documents.Where(x => !x.DocumentType.In( 13, 14)))
            {
               var documentDB = _db.Documents.FirstOrDefault(y => y.DocumentNumber == document.DocumentNumber && y.DocumentType==document.DocumentType
                                                                   && DateTime.Now <= y.ValidTo && DateTime.Now >= y.ValidFrom);
                if (documentDB == null)
                    continue;

                clientFoundWithDocument = true;


                var query = _db.ClientRefs.Where(x => x.Inn == documentDB.Inn);
                query.Include(x => x.Clients.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == documentDB.Inn)).Load();
                query.Include(x => x.PhysicalPeople.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == documentDB.Inn)).Load();
                query.Include(x => x.Companies.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == documentDB.Inn)).Load();
                query.Include(x => x.Documents.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == documentDB.Inn)).Load();
                query.Include(x => x.ClientContactInfoComps.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == documentDB.Inn))
                    .ThenInclude(x => x.ContactInfo).Load();
                query.Include(x => x.Addresses.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == documentDB.Inn)).Load();
                query.Include(x => x.LeadObjects.Where(x => x.ValidFrom < DateTime.Now && x.ValidTo > DateTime.Now && x.Inn == documentDB.Inn)).Load();

                query.Include(x => x.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.Inn == documentDB.Inn))
                    .ThenInclude(x => x.Tag).Load();
                ClientRef clientRef = query.Include(x => x.ClientRelationCompClient1Navigations.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.Client1 == documentDB.Inn))
             .First();


                SendClientsToUpdate(clientRef);
                break; //if first client fount, than dont look other documents
            }

            return clientFoundWithDocument;
        }

        private void SendClientsToUpdate(ClientRef client)
        {
            if (Parameters.ClientType == 0)
                Parameters.ClientType = (ClientType)client.Clients.First().ClientType;
            UpdateMergeModel model = new UpdateMergeModel { FoundClient = client, IncomingClient = Parameters };
            ClientUpdateOperation clientUpdateOperation = new ClientUpdateOperation(_db);
            clientUpdateOperation.Execute(model);
            Inn = client.Inn;
            Result.Merge(clientUpdateOperation.Result);
        }

        private int SendClientToCreate()
        {
            if (Parameters.ClientType != ClientType.Company && Parameters.ClientType != ClientType.Pyhsical)
            {
                Result.AddError($"Client with client type: {Parameters.ClientType} can not be created. Because client not found.");
                return -1;
            }

            UpdateMergeModel model = new UpdateMergeModel { FoundClient = null, IncomingClient = Parameters };
            ClientCreateOperation clientCreateOperation = new ClientCreateOperation(_db);
            clientCreateOperation.Execute(model);

            Result.Merge(clientCreateOperation.Result);
            Inn = clientCreateOperation.NewCreatedClientINN;
            return clientCreateOperation.NewCreatedClientINN;
        }
    }
}