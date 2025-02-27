using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using ClientType = Zircon.Core.Enums.ClientType;

namespace CRM.Operation.Models.DealOfferModels
{
    public class LiabilityInsuranceOfferModel : IDealOfferFormModel, IRenewableDealOfferModel
    {
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public string PolicyPeriod { get; set; }
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public string Currency { get; set; }
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public string PaymentType { get; set; }
        [DisplayNameLocalized("OfferModel.InsuranceAreaType")]

        public string InsuranceAreaTypeDisplayValue { get; set; }

        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        [DisplayNameLocalized("OfferModel.Beneficiary")]
        public ClientContract Beneficiary { get; set; }
        
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.Commission")]
        public decimal? Commission { get; set; }

        [DisplayNameLocalized("OfferModel.BodyInjuryLimitPerPerson")]
        [RequiredLocalized]
        public decimal? BodyInjuryLimitPerPerson { get; set; }

        [DisplayNameLocalized("OfferModel.PropertyDamageLimitPerAccident")]
        [RequiredLocalized]
        public decimal? PropertyDamageLimitPerAccident { get; set; }
        
        [CustomSource(typeof(OfferPolicyPeriodSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public int? PolicyPeriodOid { get; set; }
        
        [CustomSource(typeof(CurrencySourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public int? CurrencyOid { get; set; }
        
        [CustomSource(typeof(PaymentTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public int? PaymentTypeOid { get; set; }
        
        [DisplayNameLocalized("OfferModel.InsuranceArea")]
        [RequiredLocalized]
        public string InsuranceArea { get; set; }

        [DisplayNameLocalized("OfferModel.AnnualTurnoverOfCompany")]
        [RequiredLocalized]
        public decimal? AnnualTurnoverOfCompany { get; set; }

        [DisplayNameLocalized("OfferModel.BodyInjuryLimitPerAccident")]
        [RequiredLocalized]
        public decimal? BodyInjuryLimitPerAccident { get; set; }

        [DisplayNameLocalized("OfferModel.BodyInjuryAnnualAggregateLimit")]
        [RequiredLocalized]
        public decimal? BodyInjuryAnnualAggregateLimit { get; set; }

        [DisplayNameLocalized("OfferModel.PropertyDamageAnnualAggregateLimit")]
        [RequiredLocalized]
        public decimal? PropertyDamageAnnualAggregateLimit { get; set; }
        

        [DisplayNameLocalized("OfferModel.IsRenew")]
        [RequiredLocalized]
        public bool IsRenew { get; set; }

        [DisplayNameLocalized("PolicyNumber")]
        [RequiredIfLocalized(nameof(IsRenew), EqualTo = true)]
        public string ExistingPolicyNumber { get; set; }

        public RequestModels.ClientType DealClientType { get; set; }
    }
}