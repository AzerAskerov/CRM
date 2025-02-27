using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.SourceLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetVehicleInsDealDataInfo : BusinessOperation<DealOfferDropdownDataModel>
    {
        private CRMDbContext _db;
        public DealOfferDropdownDataModel Data { get; set; }

        public GetVehicleInsDealDataInfo(CRMDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
        protected override void DoExecute()
        {
            Data = new DealOfferDropdownDataModel();
            Data.PersonalAccidentInsuranceOfDriverAndPassengerItems = _db.Codes?.FirstOrDefault(x => x.CodeOid == Convert.ToInt32(Parameters.PersonalAccidentInsuranceOfDriverAndPassengerItemsOid) &&
                                                                                               x.TypeOid == (int)CodeTypeEnum.PERSONAL_ACCIDENT_ITEMS && 
                                                                                               DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo)?.Value;



            Data.PropertyLiabilityInsuranceItems = _db.Codes?.FirstOrDefault(x => x.CodeOid == Convert.ToInt32(Parameters.PropertyLiabilityInsuranceItemsOid) &&
                                                                            x.TypeOid == (int)CodeTypeEnum.PROPERTY_LIABILITY_ITEMS &&
                                                                            DateTime.Now > x.ValidFrom && DateTime.Now < x.ValidTo)?.Value;
           
            
            Data.SelectedVehicleUsagePurpose = _db.Codes.FirstOrDefault(x => x.CodeOid == Convert.ToInt32(Parameters.SelectedVehicleUsagePurposeOid) 
                                                                                                            && x.TypeOid == (int)CodeTypeEnum.VEHICLE_USAGE_PURPOSE
                                                                                                            && DateTime.Now > x.ValidFrom 
                                                                                                            && DateTime.Now < x.ValidTo)?.Value;

        }


    }
}
