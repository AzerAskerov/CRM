using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using ClientType = Zircon.Core.Enums.ClientType;

namespace CRM.Operation.Models.DealOfferModels
{
    public class CarAndEquipmentInsuranceOfferModel : IDealOfferFormModel, IVehicleModel
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


        public List<InsuredVehicleModel> Vehicles { get; set; }

        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        [DisplayNameLocalized("OfferModel.Beneficiary")]
        public ClientContract Beneficiary { get; set; }
        
        [CustomSource(typeof(OfferPolicyPeriodSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public int PolicyPeriodOid { get; set; }
        
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.Commission")]
        public decimal Commission { get; set; }
        
        [CustomSource(typeof(DeductibleAmountSourceLoader))]
        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.DeductibleAmount")]
        public int DeductibleAmountOid { get; set; }
        public string DeductibleAmount { get; set; }

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



        [CustomSource(typeof(VehicleBrandsSourceLoader))]
        [DisplayNameLocalized("DropDown.VehBrandOid")]
        public int? VehBrandOid { get; set; }

        [DisplayNameLocalized("DropDown.VehModelOid")]
        [CustomSource(typeof(VehicleModelsSourceLoader))]
        public int? VehModelOid { get; set; }

        [CustomSource(typeof(VehicleUsagePurposeSourceLoader))]
        [DisplayNameLocalized("DropDown.SelectedVehicleUsagePurposeOid")]
        public int? SelectedVehicleUsagePurposeOid { get; set; }

        public RequestModels.ClientType DealClientType { get; set; }


        [DisplayNameLocalized("OfferModel.CompanyActivityType")]
        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        public string CompanyActivityType { get; set; }

        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.InsuranceValue")]
        public decimal InsuranceValue { get; set; }


     //   [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.UnconditionalExemptionAmount")]
        public decimal UnconditionalExemptionAmount { get; set; }


        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.AnyUsefulInfoForInsuranceCompany")]
        public string AnyInfoForInsuranceCompany { get; set; }

        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.LastFiveYearLossHistory")]
        public string LastFiveYearLossHistory { get; set; }


        public string SelectedVehicleUsagePurpose { get; set; }
        public string VehModel { get; set; }
        public string VehBrand { get; set; }
    }
}