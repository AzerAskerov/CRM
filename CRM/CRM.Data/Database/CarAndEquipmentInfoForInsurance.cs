using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CRM.Data.Database
{
    public class CarAndEquipmentInfoForInsurance
    {
        public int Id { get; set; }
        public decimal? MarketValueOfVehicle { get; set; }
        public int? ManufactureYear { get; set; }
        public int? VehBrandOid { get; set; }
        public int? VehModelOid { get; set; }
        public double? EngineCapacity { get; set; }
        public int? SelectedVehicleUsagePurposeOid { get; set; }
        public string VehicleRegNumber { get; set; }

        public Guid DealGuid { get; set; }

        [ForeignKey("DealGuid")]
        public virtual CarAndEquipmentInsurance CarAndEquipment { get; set; }


    }
}
