using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class ChangeDealStatusOperation : BusinessOperation<ChangeDealStatusRequest>
    {
        private readonly CRMDbContext _dbContext;
    
        public ChangeDealStatusOperation(CRMDbContext db) : base(db)
        {
            _dbContext = db;
        }

        protected override void DoExecute()
        {
            var deal = _dbContext.Deal.Find(Parameters.DealGuid);

            if (deal is null)
            {
                Result.AddError($"Deal not found with id : {Parameters.DealGuid}");
                return;
            }

            deal.DealStatus = (int)Parameters.DealStatus;
            deal.CurrentUserGuid = Parameters.CurrentUserGuid;
            deal.CurrentUserName = Parameters.CurrentUserName;
            if (Parameters.DealStatus.In(DealStatus.Linked, DealStatus.Rejected))
            {
                deal.ResponsiblePersonType = (int)DealResponsiblePersonTypeEnum.NotAssigned;
            }
            _dbContext.SaveChanges();
        }
    }
}