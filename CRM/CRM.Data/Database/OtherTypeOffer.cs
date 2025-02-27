

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

using System;

namespace CRM.Data.Database
{
    public partial class OtherTypeOffer
    {
        public Guid DealGuid { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public int PolicyPeriodOid { get; set; }
        public int CurrencyOid { get; set; }
        public int PaymentTypeOid { get; set; }
        public string EmailToNotify { get; set; }
        public bool IsRenew { get; set; }
        public string ExistingPolicyNumber { get; set; }
        public string Comment { get; set; }
        public string InsuranceArea { get; set; }
        public string CompanyActivityType { get; set; }
        public string LastFiveYearsInjuryKnowledge { get; set; }
        public string AnyInfo { get; set; }
        public int? InsuranceAreaType { get; set; }
        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }
        public int? InsuranceTypeOid { get; set; }
    }
}
