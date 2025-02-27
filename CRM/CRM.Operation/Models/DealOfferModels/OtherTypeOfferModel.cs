using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using ClientType = Zircon.Core.Enums.ClientType;

namespace CRM.Operation.Models.DealOfferModels
{
    public class OtherTypeOfferModel : IDealOfferFormModel, IRenewableDealOfferModel
    {
        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        [DisplayNameLocalized("OfferModel.Beneficiary")]
        public ClientContract Beneficiary { get; set; }
        
        [CustomSource(typeof(OfferPolicyPeriodSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public int PolicyPeriodOid { get; set; }

        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public string PolicyPeriod { get; set; }

        [CustomSource(typeof(CurrencySourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public int CurrencyOid { get; set; }

        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public string Currency { get; set; }

        [CustomSource(typeof(PaymentTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public int PaymentTypeOid { get; set; }


        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public string PaymentType { get; set; }


        [DisplayNameLocalized("OfferModel.IsRenew")]
        [RequiredLocalized]
        public bool IsRenew { get; set; }
        
        [DisplayNameLocalized("PolicyNumber")]
        [RequiredIfLocalized(nameof(IsRenew), EqualTo = true)]
        public string ExistingPolicyNumber { get; set; }

        [DisplayNameLocalized("OfferModel.Comment")]
        [RequiredLocalized]
        public string Comment { get; set; }

        public RequestModels.ClientType DealClientType { get; set; }


        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        [DisplayNameLocalized("OfferModel.CompanyActivityType")]
        public string CompanyActivityType { get; set; }



        [CustomSource(typeof(InsuranceAreaSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsuranceAreaType")]

        public int? InsuranceAreaType { get; set; }

        [DisplayNameLocalized("OfferModel.InsuranceAreaType")]

        public string InsuranceAreaTypeDisplayValue { get; set; }


        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsuranceArea")]

        public string InsuranceArea { get; set; }


        [DisplayNameLocalized("OfferModel.LastFiveYearLossHistory")]
        [RequiredLocalized]
        public string LastFiveYearsInjuryKnowledge { get; set; }


        /// <summary>
        /// Bu sığorta riskinə təsir edə bilən və Sığortaçıya faydalı ola biləcək hər hansı digər informasiya varmı?
        /// </summary>
        [DisplayNameLocalized("OfferModel.AnyInfo")]
        [RequiredLocalized]
        public string AnyInfo { get; set; }


        [CustomSource(typeof(InsuranceTypeSourceLoader))]
        [DisplayNameLocalized("DropDown.InsuranceTypeOid")]
        public int? InsuranceTypeOid { get; set; }
        public string InsuranceType { get; set; }

    }
}