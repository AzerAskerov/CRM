using System;
using CRM.Operation.Attributes;
using CRM.Operation.Enums;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using ClientType = Zircon.Core.Enums.ClientType;
namespace CRM.Operation.Models.DealOfferModels
{
   public class ElectronicsInsuranceOfferModel : IDealOfferFormModel, IRenewableDealOfferModel
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

        [CustomSource(typeof(OfferPolicyPeriodSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public int PolicyPeriodOid { get; set; }

        [CustomSource(typeof(CurrencySourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public int CurrencyOid { get; set; }

        [CustomSource(typeof(PaymentTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public int PaymentTypeOid { get; set; }

        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsuranceArea")]

        public string InsuranceArea { get; set; }


        [DisplayNameLocalized("OfferModel.IsRenew")]
        [RequiredLocalized]
        public bool IsRenew { get; set; }

        [DisplayNameLocalized("PolicyNumber")]
        [RequiredIfLocalized(nameof(IsRenew), EqualTo = true)]
        public string ExistingPolicyNumber { get; set; }

        public RequestModels.ClientType DealClientType { get; set; }



        public string ImmovablePropertyType { get; set; }

        public string MovablePropertyType { get; set; }








        [DisplayNameLocalized("OfferModel.CompanyActivityType")]
        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        public string CompanyActivityType { get; set; }


        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsurancePredmetAddress")]
        public string InsurancePredmetAddress { get; set; }


        [DisplayNameLocalized("OfferModel.Unit")]
        [RequiredLocalized]
        public string Unit { get; set; }


        [DisplayNameLocalized("OfferModel.Description")]
        [RequiredLocalized]
        public string Description { get; set; }

        [DisplayNameLocalized("OfferModel.Year")]
        [RequiredLocalized]
        public int Year { get; set; }



         [DisplayNameLocalized("OfferModel.DeviceReplacementCost")]
        [RequiredLocalized]
        public decimal DeviceReplacementCost { get; set; }

        [DisplayNameLocalized("OfferModel.ManufacturerName")]
        [RequiredLocalized]
        public string ManufacturerName { get; set; }


        [DisplayNameLocalized("OfferModel.Series")]
        [RequiredLocalized]
        public string Series { get; set; }


        [DisplayNameLocalized("OfferModel.Type")]
        [RequiredLocalized]
        public string Type { get; set; }

        [DisplayNameLocalized("OfferModel.Voltage")]
        [RequiredLocalized]
        public string Voltage { get; set; }




        [DisplayNameLocalized("OfferModel.Power")]
        [RequiredLocalized]
        public string Power { get; set; }


        [DisplayNameLocalized("OfferModel.Power")]
        [RequiredLocalized]
        public decimal CustomsExpenses { get; set; }


        [DisplayNameLocalized("OfferModel.Power")]
        [RequiredLocalized]
        public decimal PackagingExpenses { get; set; }


        [DisplayNameLocalized("OfferModel.CurrentPrice")]
        [RequiredLocalized]
        public decimal CurrentPrice { get; set; }


        [DisplayNameLocalized("OfferModel.ElectronicAnyInfo")]
        public string ElectronicAnyInfo { get; set; }


        [DisplayNameLocalized("OfferModel.LostHistoryForTheLastFiveYears")]
        [RequiredLocalized]
        public string LastFiveYearsInjuryKnowledge { get; set; }


        [DisplayNameLocalized("OfferModel.SecurityAndFirefightingSystemInfo")]
        [RequiredLocalized]
        public string SecurityAndFirefightingSystemInfo { get; set; }

        /// <summary>
        /// Nə zamansa, sizinlə sığorta müqaviləsi bağlamaqdan, şəhadətnamənin müddətini uzatmaqdan imtina ediblərmi  və ya sığortanı yeniləmək üçün xüsusi şərtlər qoyublar mı? 
        /// </summary>
        [DisplayNameLocalized("OfferModel.SpecialConditions")]
        [RequiredLocalized]
        public bool SpecialConditions { get; set; }


        /// <summary>
        /// Əgər bəli, bunlar barəsində ətraflı məlumat verin.
        /// </summary>
        [DisplayNameLocalized("OfferModel.SpecialConditionsYes")]
        [RequiredIfLocalized(nameof(SpecialConditions), EqualTo = true)]
        public string SpecialConditionsYes { get; set; }

        [DisplayNameLocalized("OfferModel.DeviceAddress")]
        [RequiredLocalized]
        public string DeviceAddress { get; set; }

        [DisplayNameLocalized("OfferModel.DeviceIsNew")]
        [RequiredLocalized]
        public bool DeviceIsNew { get; set; }

        [DisplayNameLocalized("OfferModel.TakeCare")]
        [RequiredLocalized]
        public bool TakeCare { get; set; }


        [DisplayNameLocalized("OfferModel.Training")]
        [RequiredLocalized]
        public bool Training { get; set; }

        [DisplayNameLocalized("OfferModel.DangerousChemicalsUsage")]
        public bool? DangerousChemicalsUsage { get; set; }

        [DisplayNameLocalized("OfferModel.DangerousChemicals")]
        [RequiredIfLocalized(nameof(DangerousChemicalsUsage), EqualTo = true)]
        public string DangerousChemicals { get; set; }
    }
}
