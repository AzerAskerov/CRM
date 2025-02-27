using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Models.DealOfferModels;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class SubmitOffersOperation : BusinessOperation<DealModel>
    {
        private readonly CRMDbContext _dbContext;
        private readonly IMapper _mapper;
    
        public SubmitOffersOperation(CRMDbContext db, IMapper mapper) : base(db)
        {
            _dbContext = db;
            _mapper = mapper;
        }

        protected override void DoExecute()
        {
            var offers = _mapper.Map<List<Offer>>(Parameters.Offers.Where(x=>!x.IsReadOnly));

            var deal = _dbContext.Deal.Find(Parameters.DealGuid);
            
            if(deal is null)
            {
                Result.AddError($"Deal not found in db : {Parameters.DealNumber}");
                return;
            }

            deal.DealStatus = (int) DealStatus.Offered;
            deal.ResponsiblePersonType = (int) DealResponsiblePersonTypeEnum.Seller;
            deal.UnderwriterUserGuid = Parameters.UnderwriterUser.UserGuid;
            deal.CurrentUserGuid = Parameters.CurrentUserGuid;
            deal.CurrentUserName = Parameters.CurrentUserName;
            _dbContext.Offer.AddRange(offers);

            _dbContext.SaveChanges();
        }
    }
}