using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
   public partial  class HiringLiability
    {
        public string Comment { get; set; }
        public Guid DealGuid { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public int PolicyPeriodOid { get; set; }
        public int CurrencyOid { get; set; }
        public int PaymentTypeOid { get; set; }
        public string EmailToNotify { get; set; }
        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }

        public string CompanyActivityType { get; set; }

        public int InsuranceAreaType { get; set; }

        public string InsuranceArea { get; set; }

        public decimal? AnnualGeneralLimit { get; set; }


        public decimal? LimitPerAccident { get; set; }

        public decimal? LimitPerPerson { get; set; }


        public bool IsRenew { get; set; }


        public string ExistingPolicyNumber { get; set; }


        public int ServantQuantity { get; set; }


        public int LaborQuantity { get; set; }

        public decimal ServantAnnualSalaryFond { get; set; }

        public decimal LaborAnnualSalaryFond { get; set; }

        public decimal? SeaWorkerAnnualSalaryFond { get; set; }
        public int? SeaWorkerQuantity { get; set; }

        public decimal? DistanceFromSea { get; set; }
        public int? MaxTransportedEmployees { get; set; }

        public int? MaxEmployeesInSea { get; set; }

        public int? DaysPassedInSea { get; set; }
        public string ShowAcids { get; set; }

        public bool  WorkOnSea { get; set; }


        public string TypeOfTransportation { get; set; }
        public string GateGoodCondition { get; set; }
        public decimal UnconditionalExemptionAmount { get; set; }
        public string LostHistoryForTheLastFiveYears { get; set; }
        public string AnyInfo { get; set; }
        public bool? IsAnyProductDangerous { get; set; }

        public string ExplainDangerousProduct { get; set; }

    }
}
