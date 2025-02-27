using System;
using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using ClientType = Zircon.Core.Enums.ClientType;

namespace CRM.Operation.Models.DealOfferModels
{
    public class PropertyLiabilityInsuranceOfferModel : IDealOfferFormModel, IRenewableDealOfferModel
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

        // ***********
        [DisplayNameLocalized("OfferModel.CompanyActivityType")]
        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        public string CompanyActivityType { get; set; }
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.LocalCompanyFoundDate")]
        public DateTime? LocalCompanyFoundDate { get; set; }
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.ActivityFeatures")]
        public string ActivityFeatures { get; set; }

        [CustomSource(typeof(InsuranceAreaSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsuranceAreaType")]

        public int? InsuranceAreaType { get; set; }


        [DisplayNameLocalized("OfferModel.InsuranceArea")]

        public string InsuranceArea { get; set; }

        /// <summary>
        /// Sizin şirkətiniz, müəssisələriniz, giriş çıxışlarınız
        /// yaxşı hasarlanıb və qorunurmu, onlar yaxşı vəziyətdədirmi?
        /// </summary>
        [DisplayNameLocalized("OfferModel.IsSituationGood")]
        public bool? IsSituationGood { get; set; }


        /// <summary>
        /// Sizin fəaliyyətinizin hər hansı məhsulu 
        /// təhlükəli hesab edilə bilərmi
        /// </summary>
        [DisplayNameLocalized("OfferModel.IsAnyProductDangerous")]
        public bool? IsAnyProductDangerous { get; set; }

        /// <summary>
        /// Əgər bəli, təfərrüatlı şəkildə təsvir edin
        /// </summary>

        [DisplayNameLocalized("OfferModel.ExplainDangerousProduct")]
        [RequiredIfLocalized(nameof(IsAnyProductDangerous), EqualTo = true)]
        public string ExplainDangerousProduct { get; set; }


        [DisplayNameLocalized("OfferModel.NumberOfEmployees")]
        [RequiredLocalized]
        public int? NumberOfEmployees { get; set; }


        /// <summary>
        /// Sizin müəssisənin ərazisində əvvəllər ətraf mühitin çirklənməsi halları olubmu?
        /// </summary>
        [DisplayNameLocalized("OfferModel.IsAnyDirtAround")]

        public bool? IsAnyDirtAround { get; set; }
        /// <summary>
        /// Şirkətin illik dövriyyəsi / satış həcmi
        /// </summary>

        [DisplayNameLocalized("OfferModel.AnnualGeneralLimit")]
        [RequiredLocalized]
        public decimal? AnnualGeneralLimit { get; set; }

        /// <summary>
        /// Bir hadisə üzrə hədd
        /// </summary>

        [DisplayNameLocalized("OfferModel.LimitPerAccident")]
        [RequiredLocalized]
        public decimal? LimitPerAccident { get; set; }


        [CustomSource(typeof(OfferPolicyPeriodSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public int? PolicyPeriodOid { get; set; }

        [CustomSource(typeof(CurrencySourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public int CurrencyOid { get; set; }



        [CustomSource(typeof(PaymentTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public int PaymentTypeOid { get; set; }

        [DisplayNameLocalized("OfferModel.AnnualTurnoverOfCompany")]
        [RequiredLocalized]
        public decimal? AnnualTurnoverOfCompany { get; set; }



        [DisplayNameLocalized("OfferModel.UnconditionalExemptionAmount")]
        [RequiredLocalized]

        public decimal? UnconditionalExemptionAmount { get; set; }

        /// <summary>
        /// Son 5 il ərzində zərər tarixçəsi barədə məlumat
        /// </summary>

        [DisplayNameLocalized("OfferModel.LossHistory")]
        [RequiredLocalized]
        public string LossHistory { get; set; }

        /// <summary>
        /// Bu sığorta riskinə təsir edə bilən və Sığortaçıya faydalı ola biləcək hər hansı digər informasiya varmı?
        /// </summary>
        [DisplayNameLocalized("OfferModel.AnyInfo")]
        [RequiredLocalized]
        public string AnyInfo { get; set; }

        //********


        [DisplayNameLocalized("OfferModel.IsRenew")]
        [RequiredLocalized]
        public bool IsRenew { get; set; }

        [DisplayNameLocalized("PolicyNumber")]
        [RequiredIfLocalized(nameof(IsRenew), EqualTo = true)]
        public string ExistingPolicyNumber { get; set; }

        public RequestModels.ClientType DealClientType { get; set; }


        /// <summary>
        /// İstifadə ediləcək turşuları, qazları, kimyəvi və partlayıcı maddələri və onların hansı həcmdə istifadə ediləcəyini göstərin
        /// </summary>
        [DisplayNameLocalized("OfferModel.ShowAcids")]
        [RequiredLocalized]
        public string ShowAcids { get; set; }



        [DisplayNameLocalized("OfferModel.TempWorkersUsage")]
        public bool? TempWorkersUsage { get; set; }
    }
}


