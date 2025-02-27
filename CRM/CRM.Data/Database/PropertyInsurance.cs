using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class PropertyInsurance
    {
        public Guid DealGuid { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public int PolicyPeriodOid { get; set; }
        public int CurrencyOid { get; set; }
        public int PaymentTypeOid { get; set; }
        public string EmailToNotify { get; set; }
        public decimal? Commission { get; set; }


        public decimal? ImmovablePropertyInsuranceAmount { get; set; }

        public decimal? MovablePropertyInsuranceAmount { get; set; }

        public int InsuranceTypeOid { get; set; }

        public int InsuranceAreaType { get; set; }


        public string InsuranceArea { get; set; }

        public string MovablePropertyDescription { get; set; }

        public string SecurityAndFirefightingSystemInfo { get; set; }


        public string LostHistoryForTheLastFiveYears { get; set; }

        public bool IsRenew { get; set; }

        public bool SpecialConditions { get; set; }

        public string SpecialConditionsYes { get; set; }

        public string ExistingPolicyNumber { get; set; }

        public int? PropertyTypeOid { get; set; }

        public int? ImmovablePropertyTypeOid { get; set; }

        public int? MovablePropertyTypeOid { get; set; }

        public string ImmovablePropertyTypeOther { get; set; }

        public string MovablePropertyTypeOther { get; set; }



        public string AnyUsefulInfoForInsuranceCompany { get; set; }


        public string CompanyActivityType { get; set; }



        public string InsurancePredmetAddress { get; set; }


        public decimal MovablePropertyValue { get; set; }

        public decimal ImmovablePropertyValue { get; set; }
        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }
    }
}
