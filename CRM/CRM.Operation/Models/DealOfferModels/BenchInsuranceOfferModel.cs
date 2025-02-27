using System;
using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using ClientType = Zircon.Core.Enums.ClientType;
namespace CRM.Operation.Models.DealOfferModels
{
   public class BenchInsuranceOfferModel : IDealOfferFormModel, IRenewableDealOfferModel
    {
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

        [CustomSource(typeof(InsuranceAreaSourceLoader))]
        [DisplayNameLocalized("OfferModel.InsuranceArea")]

        public int InsuranceAreaType { get; set; }
        
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


        [DisplayNameLocalized("OfferModel.CompanyActivityType")]
        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        public string CompanyActivityType { get; set; }


        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsurancePredmetAddress")]
        public string InsurancePredmetAddress { get; set; }

        [DisplayNameLocalized("OfferModel.InsuredBenchValue")]
        [RequiredLocalized]
        public decimal InsuredBenchValue { get; set; }


        [DisplayNameLocalized("OfferModel.InsuredBenchAmount")]
        [RequiredLocalized]
        public decimal InsuredBenchAmount { get; set; }


        [DisplayNameLocalized("OfferModel.Unit")]
        [RequiredLocalized]
        public string Unit { get; set; }


        [DisplayNameLocalized("OfferModel.Year")]
        [RequiredLocalized]
        public int Year { get; set; }


        [DisplayNameLocalized("OfferModel.Injuries")]
        [RequiredIfLocalized(nameof(AnyInjuryKnowledge), EqualTo = true)]
        public string Injuries { get; set; }


         [DisplayNameLocalized("OfferModel.ReplacementCost")]
        [RequiredLocalized]
        public decimal ReplacementCost { get; set; }

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


        [DisplayNameLocalized("OfferModel.AnyInjuryKnowledge")]
        public bool? AnyInjuryKnowledge { get; set; }


        [DisplayNameLocalized("OfferModel.LastThreeYearsBenchInjuryKnowledge")]
        [RequiredLocalized]
        public bool LastThreeYearsInjuryKnowledge { get; set; }

        [DisplayNameLocalized("OfferModel.LastThreeYearsBenchInjuryKnowledge")]
        [RequiredIfLocalized(nameof(LastThreeYearsInjuryKnowledge), EqualTo =true)]
        public string LastThreeYearsInjuryKnowledgeYes { get; set; }


        [CustomSource(typeof(PropertyTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.ImmovablePropertyTypeOid")]
        public int? ImmovablePropertyTypeOid { get; set; }

        public string ImmovablePropertyType { get; set; }


        [CustomSource(typeof(MovablePropertyTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.DasinarEmlakNovu")]
        public int? MovablePropertyTypeOid { get; set; }
        public string MovablePropertyType { get; set; }

        [DisplayNameLocalized("DropDown.ImmovablePropertyTypeOther")]
        [RequiredIfLocalized(nameof(ImmovablePropertyTypeOid), EqualTo = 2)]
        public string ImmovablePropertyTypeOther { get; set; }


        [DisplayNameLocalized("DropDown.MovablePropertyTypeOther")]
        [RequiredIfLocalized(nameof(MovablePropertyTypeOid), EqualTo = 5)]
        public string MovablePropertyTypeOther { get; set; }

        [DisplayNameLocalized("OfferModel.SecurityAndFirefightingSystemInfo")]
        [RequiredLocalized]
        public string SecurityAndFirefightingSystemInfo { get; set; }


        [DisplayNameLocalized("OfferModel.AnyUsefulInfoForInsuranceCompany")]
        [RequiredLocalized]
        public string AnyUsefulInfoForInsuranceCompany { get; set; }

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

        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public string PolicyPeriod { get; set; }
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public string Currency { get; set; }
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public string PaymentType { get; set; }
        [DisplayNameLocalized("OfferModel.InsuranceAreaType")]

        public string InsuranceAreaTypeDisplayValue { get; set; }



        public string SelectedPayment
        {
            get
            {
                try
                {
                    return new PaymentTypeSourceLoader().GetItem(PaymentTypeOid)?.Value;

                }
                catch (Exception)
                {
                    return null;

                }

            }
        }

        public string SelectedCurrency
        {
            get
            {
                try
                {
                    return new CurrencySourceLoader().GetItem(CurrencyOid)?.Value;

                }
                catch (Exception)
                {
                    return null;

                }

            }
        }
    }
}
