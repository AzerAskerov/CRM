using System.Collections.Generic;
using System.Net.Http;
using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Helpers;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using Zircon.Core.Config;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class SetUnderwriterForDealOperation: BusinessOperation<DealModel>
    {
        private readonly CRMDbContext _dbContext;
        private readonly IMapper _mapper;

        public SetUnderwriterForDealOperation(CRMDbContext db, IMapper mapper) : base(db)
        {
            _dbContext = db;
            _mapper = mapper;
        }

        protected override void DoExecute()
        {
            var deal = _dbContext.Deal.Find(Parameters.DealGuid);
            
            if(deal is null)
            {
                Result.AddError($"Deal not found in db : {Parameters.DealGuid}");
                return;
            }

            deal.UnderwriterUserGuid = Parameters.UnderwriterUser?.UserGuid;
            deal.UnderwriterUserFullName = Parameters.UnderwriterUser?.FullName;
            deal.CurrentUserGuid = Parameters.CurrentUserGuid;
            deal.CurrentUserName = Parameters.CurrentUserName;
            _dbContext.SaveChanges();
        }
    }
}