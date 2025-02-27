using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Models.DealOfferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetCargoDataOperation : BusinessOperation<DealOfferDropdownDataModel>
    {
        private CRMDbContext _db;
        public IMapper _mapper;
        public DealOfferDropdownDataModel Data { get; set; }

        public GetCargoDataOperation(CRMDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _db = dbContext;
            _mapper = mapper;
        }
        protected override void DoExecute()
        {
            Data = new DealOfferDropdownDataModel();
            Data.TransportationName = _db.Codes?.FirstOrDefault(x => x.CodeOid == Convert.ToInt32(Parameters.TransportationId) &&
                                                                                               x.TypeOid == (int)CodeTypeEnum.TRANSPORTATION_TYPE &&
                                                                                               DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo)?.Value;


            Data.PackagingName = _db.Codes?.FirstOrDefault(x => x.CodeOid == Convert.ToInt32(Parameters.PackagingId) &&
                                                                                   x.TypeOid == (int)CodeTypeEnum.CARGO_PACKAGING &&
                                                                                   DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo)?.Value;
        }
    }
}
