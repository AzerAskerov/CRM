using AutoMapper;
using CRM.Data.Database;
using CRM.Operation.Models.DealOfferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetVehiclesForInsuranceOperation : BusinessOperation<Guid>
    {
        private CRMDbContext _db;
        public IMapper _mapper;
        public List<InsuredVehicleModel> Vehicles { get; set; }

        public GetVehiclesForInsuranceOperation(CRMDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _db = dbContext;
            _mapper = mapper;
        }
        protected override void DoExecute()
        {
            Vehicles = _mapper.Map<List<InsuredVehicleModel>>(_db.VehiclesForInsurance.Where(x => x.DealGuid == Parameters));


        }
    }
}
