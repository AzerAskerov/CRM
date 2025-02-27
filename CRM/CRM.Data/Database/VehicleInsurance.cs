using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class VehicleInsurance 
    {
        public VehicleInsurance()
        {
            Vehicles = new HashSet<VehicleInfoForInsurance>();
        }

        public Guid DealGuid { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public int? PolicyPeriodOid { get; set; }
        public decimal? Commission { get; set; }
        public int? DeductibleAmountOid { get; set; }
        public int? CurrencyOid { get; set; }
        public int? PaymentTypeOid { get; set; }
        public string EmailToNotify { get; set; }
        public string InsuranceArea { get; set; }
       
        public bool IsRenew { get; set; }
        public string ExistingPolicyNumber { get; set; }

        public string LastFiveYearLossHistory { get; set; }
        public string InfoForInsuranceCompany { get; set; }
        public string CompanyActivityType { get; set; }
        public decimal? UnconditionalExemptionAmount { get; set; }
        public decimal? ThirdPartiesAmount { get; set; }
        public decimal? PersonalAccidentAmount { get; set; }


        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }
        public virtual ICollection<VehicleInfoForInsurance> Vehicles { get; set; }
    }
}
