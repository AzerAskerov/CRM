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
    public class GetPropertySourceDataInfo : BusinessOperation<DealOfferDropdownDataModel>
    {
        private CRMDbContext _db;
        public DealOfferDropdownDataModel Data { get; set; }

        public GetPropertySourceDataInfo(CRMDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
        protected override void DoExecute()
        {
            Data = new DealOfferDropdownDataModel();
            Data.MovableType = _db.Codes.Where(x => x.CodeOid == Parameters.MovableTypeOid && x.TypeOid == (int)CodeTypeEnum.MOVABLE_PROPERTY_TYPE && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo).FirstOrDefault()?.Value;
            Data.ImmovableType = _db.Codes.Where(x => x.CodeOid == Parameters.ImmovableTypeOid && x.TypeOid == (int)CodeTypeEnum.IMMOVABLE_PROPERTY_TYPE && DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo).FirstOrDefault()?.Value;
        }
    }
}
