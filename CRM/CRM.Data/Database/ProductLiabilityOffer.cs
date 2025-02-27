using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
    public partial class ProductLiabilityOffer
    {
        public Guid DealGuid { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public int PolicyPeriodOid { get; set; }
        public int CurrencyOid { get; set; }
        public int PaymentTypeOid { get; set; }
        public string EmailToNotify { get; set; }
        public string Comment { get; set; }

        public string CompanyActivityType { get; set; }

        public DateTime? LocalCompanyFoundDate { get; set; }

        public string ActivityFeatures { get; set; }

        public string AllCompanyNames { get; set; }

        public int InsuranceAreaType { get; set; }

        public string InsuranceArea { get; set; }

        public bool? IsSituationGood { get; set; }


        public string ExplainProducts { get; set; }

        public bool QualityCheck { get; set; }

        public bool IsAnyProductDangerous { get; set; }

        public string ExplainDangerousProduct { get; set; }

        public int NumberOfEmployees { get; set; }


        public bool IsAnyDirtAround { get; set; }

        public decimal? AnnualGeneralLimit { get; set; }


        public decimal? LimitPerAccident { get; set; }


        public decimal? AnnualTurnoverOfCompany { get; set; }


        public string CurrentYearSales { get; set; }


        public decimal UnconditionalExemptionAmount { get; set; }

        public string LossHistory { get; set; }


        public string AnyInfo { get; set; }

        public string ShowAcids { get; set; }

        public bool IsRenew { get; set; }

        public string ExistingPolicyNumber { get; set; }


        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }

    }
}
