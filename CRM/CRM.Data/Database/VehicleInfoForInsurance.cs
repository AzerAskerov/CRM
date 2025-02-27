using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.Data.Database
{
    public partial class VehicleInfoForInsurance
    {
        public int Id { get; set; }
        public decimal? MarketValueOfVehicle { get; set; }
        public int? ManufactureYear { get; set; }
        public int? VehBrandOid { get; set; }
        public int? VehModelOid { get; set; }
        public string OfficialServiceRequired { get; set; }
        public int? PersonalAccidentInsuranceOfDriverAndPassengerItemsOid { get; set; }
        public int? PropertyLiabilityInsuranceItemsOid { get; set; }
        public int? CountOfInsuredSeats { get; set; }
        public double? EngineCapacity { get; set; }
        public int? SelectedVehicleUsagePurposeOid { get; set; }
        public string VehicleRegNumber { get; set; }
        public int? NumberOfSeats { get; set; }
        public bool? RepairNeed { get; set; }

        public Guid DealGuid { get; set; }

        [ForeignKey("DealGuid")]
        public virtual VehicleInsurance VehicleInfo { get; set; }


    }
}
