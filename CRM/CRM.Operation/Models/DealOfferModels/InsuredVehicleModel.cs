using CRM.Operation.Attributes;
using CRM.Operation.SourceLoaders;
using System;
using System.Collections.Generic;
using System.Text;
using Zircon.Core;

namespace CRM.Operation.Models.DealOfferModels
{
    public class InsuredVehicleModel
    {
        public decimal? MarketValueOfVehicle { get; set; }

        public int? ManufactureYear { get; set; }


        public string VehBrandOid { get; set; }
        public string VehBrand { get; set; }

        public string OfficialServiceRequired { get; set; }


        public string VehModelOid { get; set; }
        public string VehModel { get; set; }



        public string PropertyLiabilityInsuranceItemsOid { get; set; }
        public string PropertyLiabilityInsuranceItems { get; set; }


        public int? CountOfInsuredSeats { get; set; }


        public double? EngineCapacity { get; set; }


        public string SelectedVehicleUsagePurposeOid { get; set; }
        public string SelectedVehicleUsagePurpose { get; set; }

        public int? NumberOfSeats { get; set; }


        public string VehicleRegNumber { get; set; }

        public string PersonalAccidentInsuranceOfDriverAndPassengerItemsOid { get; set; }
        public string PersonalAccidentInsuranceOfDriverAndPassengerItems { get; set; }

        public List<SourceItem> VehModels { get; set; }

    }
}
