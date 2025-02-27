using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using System;
using ClientType = Zircon.Core.Enums.ClientType;

namespace CRM.Operation.Models.DealOfferModels
{
    public class PropertyInsuranceOfferModel : IDealOfferFormModel, IRenewableDealOfferModel
    {
        public Guid DealGuid { get; set; }
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
        
        
        [DisplayNameLocalized("OfferModel.Commission")]
        [RequiredLocalized]
        public decimal? Commission { get; set; }

        [DisplayNameLocalized("OfferModel.ImmovablePropertyInsuranceAmount")]
        [RequiredLocalized]
        public decimal? ImmovablePropertyInsuranceAmount { get; set; }

        [DisplayNameLocalized("OfferModel.MovablePropertyInsuranceAmount")]
        [RequiredLocalized]
        public decimal? MovablePropertyInsuranceAmount { get; set; }

        [CustomSource(typeof(InsuranceTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("DropDown.InsuranceTypeOid")]
        public int? InsuranceTypeOid { get; set; }
        public string InsuranceType { get; set; }
        public string SelectedInsuranceType
        {
            get
            {
                if (InsuranceTypeOid!=null)
                {
                    try
                    {
                        var item = new InsuranceTypeSourceLoader().GetItem(InsuranceTypeOid);
                        return item.Value;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                   
                }
                return null;
              

            }

        }
        [CustomSource(typeof(InsuranceAreaSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("DropDown.InsuranceAreaType")]

        public int InsuranceAreaType { get; set; }


        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsuranceArea")]

        public string InsuranceArea { get; set; }


        [DisplayNameLocalized("OfferModel.SecurityAndFirefightingSystemInfo")]
        [RequiredLocalized]
        public string SecurityAndFirefightingSystemInfo { get; set; }

        [DisplayNameLocalized("OfferModel.LostHistoryForTheLastFiveYears")]
        [RequiredLocalized]
        public string LostHistoryForTheLastFiveYears { get; set; }

        [DisplayNameLocalized("OfferModel.IsRenew")]
        [RequiredLocalized]
        public bool IsRenew { get; set; }
        /// <summary>
        /// Nə zamansa, sizinlə sığorta müqaviləsi bağlamaqdan, şəhadətnamənin müddətini uzatmaqdan imtina ediblərmi  və ya sığortanı yeniləmək üçün xüsusi şərtlər qoyublar mı? 
        /// </summary>
        [DisplayNameLocalized("OfferModel.SpecialConditions")]
        [RequiredLocalized]
        public bool SpecialConditions { get; set; }

        /// <summary>
        /// Əgər bəli, bunlar barəsində ətraflı məlumat verin.
        /// </summary>
        [DisplayNameLocalized("SpecialConditionsYes")]
        [RequiredIfLocalized(nameof(SpecialConditions), EqualTo = true)]
        public string SpecialConditionsYes { get; set; }

        [DisplayNameLocalized("PolicyNumber")]
        [RequiredIfLocalized(nameof(IsRenew), EqualTo = true)]
        public string ExistingPolicyNumber { get; set; }
        
        [CustomSource(typeof(PropertyTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("DropDown.DasinarEmlakNovu")]
        public int? ImmovablePropertyTypeOid { get; set; }
        public string ImmovablePropertyType { get; set; }

        [CustomSource(typeof(MovablePropertyTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("DropDown.DasinarEmlakNovu")]
        public int? MovablePropertyTypeOid { get; set; }
        public string MovablePropertyType { get; set; }

        [DisplayNameLocalized("DropDown.ImmovablePropertyTypeOther")]
        [RequiredIfLocalized(nameof(ImmovablePropertyTypeOid), EqualTo = 2)]
        public string ImmovablePropertyTypeOther { get; set; }


        [DisplayNameLocalized("DropDown.MovablePropertyTypeOther")]
        [RequiredIfLocalized(nameof(MovablePropertyTypeOid), EqualTo = 5)]
        public string MovablePropertyTypeOther { get; set; }


        [DisplayNameLocalized("OfferModel.AnyUsefulInfoForInsuranceCompany")]
        [RequiredLocalized]
        public string AnyUsefulInfoForInsuranceCompany { get; set; }
        // ***********
       
        [DisplayNameLocalized("OfferModel.CompanyActivityType")]
        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        public string CompanyActivityType { get; set; }

       

        public string SelectedImmovablePropertyType
        {
            get
            {
                if (ImmovablePropertyTypeOid != null)
                {
                    try
                    {
                        var item = new PropertyTypeSourceLoader().GetItem(ImmovablePropertyTypeOid);
                        return item.Value;

                    }
                    catch (Exception)
                    {
                        return null;

                    }

                }
                else
                    return null;
              

            }
        }

        public string SelectedMovablePropertyType
        {
            get
            {
                if (MovablePropertyTypeOid != null)
                {
                    try
                    {
                        var item = new PropertyTypeSourceLoader().GetItem(MovablePropertyTypeOid);
                        return item.Value;

                    }
                    catch (Exception)
                    {
                        return null;

                    }

                }
                else
                    return null;


            }
        }
        public RequestModels.ClientType DealClientType { get; set; }

        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsurancePredmetAddress")]
        public string InsurancePredmetAddress { get; set; }

        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.MovablePropertyValue")]
        public decimal MovablePropertyValue { get; set; }


        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.ImmovablePropertyValue")]
        public decimal ImmovablePropertyValue { get; set; }


    }
}