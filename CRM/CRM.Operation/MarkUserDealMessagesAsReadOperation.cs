using System.Linq;
using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class MarkUserDealMessagesAsReadOperation : BusinessOperation<MarkUserDealMessagesAsReadModel>
    {
        private readonly CRMDbContext _crmDbContext;
        private readonly IMapper _mapper;
        
        public MarkUserDealMessagesAsReadOperation(DbContext db, IMapper mapper) : base(db)
        {
            _crmDbContext = (CRMDbContext) db;
            _mapper = mapper;
        }

        protected override void DoExecute()
        {
            var deal = _crmDbContext.Deal.Find(Parameters.DealGuid);

            if (deal is null)
            {
                Result.AddError($"Deal not found with id : {Parameters.DealGuid}");
                return;
            }

            var discussionsToMarkAsRead = deal.Discussion.Where(x => x.ReceiverGuid.Equals(Parameters.UserGuid));
            
            foreach (var discussion in discussionsToMarkAsRead)
            {
                discussion.IsRead = true;
            }

            _crmDbContext.SaveChanges();
        }
    }
}