using System;
using System.Linq;
using AutoMapper;
using CRM.Data.Database;
using CRM.Operation.Models.DealOfferModels;
using Microsoft.EntityFrameworkCore;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    /// <summary>
    /// Loads deal by dealNumber by calling <see cref="LoadDealByDealGuidOperation"/> operation internally.
    /// </summary>
    public class LoadDealByDealNumberOperation : BusinessOperation<string>
    {
        private CRMDbContext CrmDbContext => DbContext as CRMDbContext;
        private readonly IMapper _mapper;
        public DealModel DealModel { get; private set; }

        public LoadDealByDealNumberOperation(DbContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }

        protected override void DoExecute()
        {
            var deal = CrmDbContext.Deal.FirstOrDefault(x => x.DealNumber.Equals(Parameters));

            if (deal is null)
            {
                Result.AddError($"Deal not found with identifier : {Parameters}");
                return;
            }

            var loadDealByGuidOperation = new LoadDealByDealGuidOperation(DbContext, _mapper);

            loadDealByGuidOperation.Execute(deal.DealGuid);

            Result.Merge(loadDealByGuidOperation.Result);

            DealModel = loadDealByGuidOperation.DealModel;
        }
    }
}