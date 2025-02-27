using System;
using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using ClientType = Zircon.Core.Enums.ClientType;


namespace CRM.Operation.Models.DealOfferModels
{
    public class HiringLiabilityInsuranceOfferModel : IDealOfferFormModel, IRenewableDealOfferModel
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

        [CustomSource(typeof(InsuranceAreaSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsuranceAreaType")]

        public int InsuranceAreaType { get; set; }



        [CustomSource(typeof(OfferPolicyPeriodSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public int? PolicyPeriodOid { get; set; }

        [CustomSource(typeof(CurrencySourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public int? CurrencyOid { get; set; }


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

        /// <summary>
        /// Bir şəxs üzrə hədd
        /// </summary>

        [DisplayNameLocalized("OfferModel.LimitPerPerson")]
        [RequiredLocalized]
        public decimal? LimitPerPerson { get; set; }

        [CustomSource(typeof(PaymentTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public int? PaymentTypeOid { get; set; }


        [DisplayNameLocalized("OfferModel.IsRenew")]
        [RequiredLocalized]
        public bool IsRenew { get; set; }

        [DisplayNameLocalized("PolicyNumber")]
        [RequiredIfLocalized(nameof(IsRenew), EqualTo = true)]
        public string ExistingPolicyNumber { get; set; }

        public RequestModels.ClientType DealClientType { get; set; }



        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.ServantQuantity")]

        public int? ServantQuantity { get; set; }



        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.LaborQuantity")]

        public int? LaborQuantity { get; set; }



        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.ServantAnnualSalaryFond")]

        public decimal? ServantAnnualSalaryFond { get; set; }


        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.LaborAnnualSalaryFond")]

        public decimal? LaborAnnualSalaryFond { get; set; }



        [DisplayNameLocalized("OfferModel.SeaWorkerAnnualSalaryFond")]

        public decimal? SeaWorkerAnnualSalaryFond { get; set; }



        [DisplayNameLocalized("OfferModel.SeaWorkerQuantity")]

        public int? SeaWorkerQuantity { get; set; }





        [DisplayNameLocalized("OfferModel.DistanceFromSea")]

        public decimal? DistanceFromSea { get; set; }


        /// <summary>
        /// Sizin fəaliyyətinizin hər hansı məhsulu 
        /// təhlükəli hesab edilə bilərmi
        /// </summary>
        [DisplayNameLocalized("OfferModel.IsAnyProductDangerous")]
        public bool? IsAnyProductDangerous { get; set; }

        [DisplayNameLocalized("OfferModel.ExplainDangerousProduct")]
        [RequiredIfLocalized(nameof(IsAnyProductDangerous), EqualTo = true)]
        public string ExplainDangerousProduct { get; set; }

        [DisplayNameLocalized("OfferModel.MaxTransportedEmployees")]

        public int? MaxTransportedEmployees { get; set; }


        [DisplayNameLocalized("OfferModel.TypeOfTransportation")]

        public string TypeOfTransportation { get; set; }


        [DisplayNameLocalized("OfferModel.MaxEmployeesInSea")]

        public int? MaxEmployeesInSea { get; set; }



        [DisplayNameLocalized("OfferModel.DaysPassedInSea")]

        public int? DaysPassedInSea { get; set; }


        /// <summary>
        /// İstifadə ediləcək turşuları, qazları, kimyəvi və partlayıcı maddələri və onların hansı həcmdə istifadə ediləcəyini göstərin
        /// </summary>
        [DisplayNameLocalized("OfferModel.ShowAcids")]
        [RequiredLocalized]
        public string ShowAcids { get; set; }

        /// <summary>
        /// İstifadə ediləcək turşuları, qazları, kimyəvi və partlayıcı maddələri və onların hansı həcmdə istifadə ediləcəyini göstərin
        /// </summary>
        [DisplayNameLocalized("OfferModel.GateGoodCondition")]
        public string GateGoodCondition { get; set; }


        [DisplayNameLocalized("OfferModel.UnconditionalExemptionAmount")]
        public decimal? UnconditionalExemptionAmount { get; set; }


        [DisplayNameLocalized("OfferModel.LostHistoryForTheLastFiveYears")]
        public string LostHistoryForTheLastFiveYears { get; set; }

        [DisplayNameLocalized("OfferModel.AnyInfo")]
        [RequiredLocalized]
        public string AnyInfo { get; set; }


        [DisplayNameLocalized("OfferModel.WorkOnSea")]
        public bool? WorkOnSea { get; set; }



    }
}
