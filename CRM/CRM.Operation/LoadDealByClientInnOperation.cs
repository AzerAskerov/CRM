using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CRM.Data.Database;
using CRM.Operation.Models.DealOfferModels;
using Microsoft.EntityFrameworkCore;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    /// <summary>
    /// Loads deals by client inn with minimum info.
    /// </summary>
    public class LoadDealByClientInnOperation : BusinessOperation<int>
    {
        private CRMDbContext CrmDbContext => DbContext as CRMDbContext;
        private readonly IMapper _mapper;
        public List<DealModel> DealModels { get; private set; }

        public LoadDealByClientInnOperation(DbContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }

        protected override void DoExecute()
        {
            var deals = CrmDbContext.Deal.Where(x=>x.ClientInn.Equals(Parameters));

            DealModels = _mapper.Map<List<DealModel>>(deals);
        }
    }
}