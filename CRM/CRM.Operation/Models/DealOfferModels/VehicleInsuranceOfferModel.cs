using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using ClientType = Zircon.Core.Enums.ClientType;

namespace CRM.Operation.Models.DealOfferModels
{
    public class VehicleInsuranceOfferModel : IDealOfferFormModel,IVehicleModel, IRenewableDealOfferModel
    {
        public Guid DealGuid { get; set; }
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public string PolicyPeriod { get; set; }
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public string Currency { get; set; }
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public string PaymentType { get; set; }
        [DisplayNameLocalized("DropDown.InsuranceAreaType")]
        public string InsuranceAreaTypeDisplayValue { get; set; }

        [DisplayNameLocalized("OfferModel.Beneficiary")]
        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        public ClientContract Beneficiary { get; set; }

        [CustomSource(typeof(OfferPolicyPeriodSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public int PolicyPeriodOid { get; set; }
        
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.Commission")]
        public decimal? Commission { get; set; }
        
        [CustomSource(typeof(DeductibleAmountSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.DeductibleAmountOid")]
        public int? DeductibleAmountOid { get; set; }
        public string DeductibleAmount { get; set; }

        [CustomSource(typeof(CurrencySourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public int? CurrencyOid { get; set; }
        
        [CustomSource(typeof(PaymentTypeSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public int? PaymentTypeOid { get; set; }

        public string InsuranceArea { get; set; }
        public string SelectedModel
        {
            get
            {

                if (VehModelOid != null)
                {
                    try
                    {
                        return new VehicleModelsSourceLoader().GetItem(VehModelOid, this)?.Value;

                    }
                    catch (Exception e)
                    {
                        return null;

                    }

                }
                else
                    return null;



            }
        }
        [DisplayNameLocalized("OfferModel.IsRenew")]
        [RequiredLocalized]
        public bool IsRenew { get; set; }
        
        [DisplayNameLocalized("PolicyNumber")]
        [RequiredIfLocalized(nameof(IsRenew), EqualTo = true)]
        public string ExistingPolicyNumber { get; set; }
        [DisplayNameLocalized("UnconditionalExemptionAmount")]
        public decimal? UnconditionalExemptionAmount { get; set; }
        public RequestModels.ClientType DealClientType { get; set; }

        [CustomSource(typeof(VehicleBrandsSourceLoader))]
        [DisplayNameLocalized("DropDown.VehBrandOid")]
        public int? VehBrandOid { get; set; }
        public string VehBrand { get; set; }

        public string SelectedBrand
        {
            get
            {

                if (VehBrandOid != null)
                {
                    try
                    {
                        return new VehicleBrandsSourceLoader().GetItem(VehBrandOid, null)?.Value;

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

    
        [CustomSource(typeof(VehicleUsagePurposeSourceLoader))]
        [DisplayNameLocalized("DropDown.SelectedVehicleUsagePurposeOid")]
        public int? SelectedVehicleUsagePurposeOid { get; set; }
        public string SelectedVehicleUsagePurpose { get; set; }
        [DisplayNameLocalized("DropDown.VehModelOid")]
        [CustomSource(typeof(VehicleModelsSourceLoader))]
        public int? VehModelOid { get; set; }
        public string VehModel { get; set; }


        [CustomSource(typeof(PersonalAccidentItemsSourceLoader))]
        [DisplayNameLocalized("DropDown.PersonalAccidentInsuranceOfDriverAndPassengerItemsOid")]
        public int? PersonalAccidentInsuranceOfDriverAndPassengerItemsOid { get; set; }
        public string PersonalAccidentInsuranceOfDriverAndPassengerItems { get; set; }

        [CustomSource(typeof(PropertyLiabilityItemsSourceLoader))]
        [DisplayNameLocalized("DropDown.PropertyLiabilityInsuranceItemsOid")]
        public int? PropertyLiabilityInsuranceItemsOid { get; set; }
        public string PropertyLiabilityInsuranceItems { get; set; }



        [ValidateComplexType]
        public List<InsuredVehicleModel> Vehicles { get; set; }

        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.LastFiveYearLossHistory")]
        public string LastFiveYearLossHistory { get; set; }

        [DisplayNameLocalized("OfferModel.InfoForInsuranceCompany")]
        public string InfoForInsuranceCompany { get; set; }

        // ***********
        [DisplayNameLocalized("OfferModel.CompanyActivityType")]
        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        public string CompanyActivityType { get; set; }


    }
}