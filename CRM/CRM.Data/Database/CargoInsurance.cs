using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class CargoInsurance
    {
        public Guid DealGuid { get; set; }
        public int? BeneficiaryClientInn { get; set; }
        public decimal? Commission { get; set; }
        public int? PolicyPeriodOid { get; set; }
        public int? CurrencyOid { get; set; }
        public int? PaymentTypeOid { get; set; }
        public string EmailToNotify { get; set; }
        public string InsuranceArea { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? InvoiceAmountOfCargo { get; set; }
        public decimal? ShippingCosts { get; set; }
        public string StartPoint { get; set; }
        public string DestinationPoint { get; set; }
        public DateTime? ProbableShippingDate { get; set; }
        public DateTime? ProbableDeliveryDate { get; set; }
        public string Packaging { get; set; }
        public string PackagingOther { get; set; }
        public string CompanyActivityType { get; set; }
        public string LostHistoryForTheLastFiveYears { get; set; }
        public string CargoFeatures { get; set; }
        public string AnyInfo { get; set; }
        public string Route { get; set; }
        public string CargoDescription { get; set; }
        public int? InsuranceAreaType { get; set; }


        public string StartPointCountryId { get; set; }
        public string EndPointCountryId { get; set; }
        public string StartPointCityId { get; set; }
        public string EndPointCityId { get; set; }
        public virtual ClientRef BeneficiaryClientInnNavigation { get; set; }
        public virtual Deal Deal { get; set; }

        public int? TransportationKind { get; set; }

    }
}
