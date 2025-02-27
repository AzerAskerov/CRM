using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class CarAndEquipmentInsurance
    {
        public CarAndEquipmentInsurance()
        {
            Vehicles = new HashSet<CarAndEquipmentInfoForInsurance>();
        }
        public virtual ICollection<CarAndEquipmentInfoForInsurance> Vehicles { get; set; }
        public Guid DealGuid { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public int? PolicyPeriodOid { get; set; }
        public decimal Commission { get; set; }
        public int DeductibleAmountOid { get; set; }
        public int CurrencyOid { get; set; }
        public int? PaymentTypeOid { get; set; }
        public string EmailToNotify { get; set; }
        public string InsuranceArea { get; set; }
        public int? ManufactureYear { get; set; }
        public int? VehBrandOid { get; set; }
        public int? VehModelOid { get; set; }
        public int? SelectedVehicleUsagePurposeOid { get; set; }


        public string CompanyActivityType { get; set; }
        public string AnyInfoForInsuranceCompany { get; set; }
        public string LastFiveYearLossHistory { get; set; }
        public int? InsuranceAreaType { get; set; }
        public decimal? UnconditionalExemptionAmount { get; set; }
        public decimal? InsuranceValue { get; set; }

        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }

    }
}
