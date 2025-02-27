using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class LiabilityInsurance
    {
        public Guid DealGuid { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public decimal Commission { get; set; }
        public decimal? BodyInjuryLimitPerPerson { get; set; }
        public decimal? PropertyDamageLimitPerAccident { get; set; }
        public int CurrencyOid { get; set; }
        public int PaymentTypeOid { get; set; }
        public int PolicyPeriodOid { get; set; }
        public string InsuranceArea { get; set; }
        public decimal? AnnualTurnoverOfCompany { get; set; }
        public decimal? BodyInjuryLimitPerAccident { get; set; }
        public decimal? BodyInjuryAnnualAggregateLimit { get; set; }
        public decimal? PropertyDamageAnnualAggregateLimit { get; set; }
        public string EmailToNotify { get; set; }
        public bool IsRenew { get; set; }
        public string ExistingPolicyNumber { get; set; }

        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }
    }
}
