using System;
using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using ClientType = Zircon.Core.Enums.ClientType;

namespace CRM.Operation.Models.DealOfferModels
{
    public class CargoInsuranceOfferModel : IDealOfferFormModel
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
        

        [CustomSource(typeof(InsuranceAreaSourceLoader))]
        [DisplayNameLocalized("OfferModel.InsuranceAreaType")]

        public int? InsuranceAreaType { get; set; }


        [DisplayNameLocalized("OfferModel.InsuranceArea")]

        public string InsuranceArea { get; set; }
        [DisplayNameLocalized("OfferModel.InvoiceNumber")]
        [RequiredLocalized]
        public string InvoiceNumber { get; set; }

        [DisplayNameLocalized("OfferModel.InvoiceDate")]
        [RequiredLocalized]
        public DateTime? InvoiceDate { get; set; }

        [DisplayNameLocalized("OfferModel.InvoiceAmountOfCargo")]
        [RequiredLocalized]
        public decimal? InvoiceAmountOfCargo { get; set; }

        [DisplayNameLocalized("OfferModel.ShippingCosts")]
        [RequiredLocalized]
        public decimal? ShippingCosts { get; set; }

        [DisplayNameLocalized("OfferModel.Route")]
        public string Route { get; set; }

        [DisplayNameLocalized("OfferModel.StartPoint")]
      
        public string StartPoint { get; set; }

        [DisplayNameLocalized("OfferModel.DestinationPoint")]

        public string DestinationPoint { get; set; }

        [DisplayNameLocalized("OfferModel.ProbableShippingDate")]
        [RequiredLocalized]
        public DateTime? ProbableShippingDate { get; set; }

        [DisplayNameLocalized("OfferModel.ProbableDeliveryDate")]
        [RequiredLocalized]
        public DateTime? ProbableDeliveryDate { get; set; }

        [CustomSource(typeof(CargoPackagingSourceLoader))]
        [DisplayNameLocalized("OfferModel.Packaging")]
        [RequiredLocalized]
        public int? Packaging { get; set; }

        public string PackagingName { get; set; }

        [CustomSource(typeof(TransportationSourceLoader))]
        [DisplayNameLocalized("OfferModel.TransportationKind")]
        [RequiredLocalized]
        public int?TransportationKind { get; set; }


        public string TransportationKindName { get; set; }


        [DisplayNameLocalized("OfferModel.PackagingOther")]
        [RequiredIfLocalized(nameof(Packaging),EqualTo = 6)]
        public string PackagingOther { get; set; }

        public RequestModels.ClientType DealClientType { get; set; }



        [DisplayNameLocalized("OfferModel.CompanyActivityType")]
        [RequiredIfLocalized(nameof(DealClientType), EqualTo = ClientType.LegalPersonCode)]
        public string CompanyActivityType { get; set; }


        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.CargoDescription")]
        public string CargoDescription { get; set; }


        [RequiredLocalized]
        [DisplayNameLocalized("OfferModel.CargoFeatures")]
        public string CargoFeatures { get; set; }


        [DisplayNameLocalized("OfferModel.LostHistoryForTheLastFiveYears")]
        [RequiredLocalized]
        public string LostHistoryForTheLastFiveYears { get; set; }


        [DisplayNameLocalized("OfferModel.AnyInfo")]
        [RequiredLocalized]
        public string AnyInfo { get; set; }

        [RequiredLocalized]
        public string StartPointCountryId { get; set; }
        [RequiredLocalized]
        public string EndPointCountryId { get; set; }
     //   [RequiredLocalized]
        public string RouteCountryId { get; set; }
     //   [RequiredLocalized]
        public string RouteCityId { get; set; }
        public string RouteCity { get; set; }
        [RequiredLocalized]
        public string StartPointCityId { get; set; }
        [RequiredLocalized]
        public string EndPointCityId { get; set; }


    }
}