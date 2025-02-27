using CRM.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.DealOfferModels
{
   public partial  class HiringLiabilityOffer
    {
        public Guid DealGuid { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public int PolicyPeriodOid { get; set; }
        public int? DeductibleAmountOid { get; set; }
        public int? CurrencyOid { get; set; }
        public int? PaymentTypeOid { get; set; }
        public string CompanyActivityType { get; set; }
        public int? InsuranceAreaType { get; set; }
        public string InsuranceArea { get; set; }
        public decimal? AnnualGeneralLimit { get; set; }
        public decimal? LimitPerAccident { get; set; }
        public decimal? LimitPerPerson { get; set; }
        public bool IsRenew { get; set; }
        public string ExistingPolicyNumber { get; set; }
        public int? ServantQuantity { get; set; }
        public int? LaborQuantity { get; set; }
        public decimal? ServantAnnualSalaryFond { get; set; }
        public decimal? LaborAnnualSalaryFond { get; set; }
        public int? SeaWorkerAnnualSalaryFond { get; set; }
        public decimal? SeaWorkerQuantity { get; set; }
        public decimal? DistanceFromSea { get; set; }
         public int? MaxTransportedEmployees { get; set; }
        public int? MaxEmployeesInSea { get; set; }
        public int? DaysPassedInSea { get; set; }
        public string ShowAcids { get; set; }
      

        public string TypeOfTransportation { get; set; }


        public string GateGoodCondition { get; set; }


        public decimal? UnconditionalExemptionAmount { get; set; }


        public string LostHistoryForTheLastFiveYears { get; set; }


        public string AnyInfo { get; set; }


        public bool? WorkOnSea { get; set; }
        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }
    }
}
