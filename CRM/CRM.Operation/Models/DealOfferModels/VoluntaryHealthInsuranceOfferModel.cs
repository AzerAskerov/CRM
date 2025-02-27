using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CRM.Operation.Attributes;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.SourceLoaders;
using Zircon.Core.OperationModel;
using ClientType = Zircon.Core.Enums.ClientType;

namespace CRM.Operation.Models.DealOfferModels
{
    public class VoluntaryHealthInsuranceOfferModel : IDealOfferFormModel, IRenewableDealOfferModel
    {
        [DisplayNameLocalized("OfferModel.PolicyPeriodOid")]
        public string PolicyPeriod { get; set; }
        [DisplayNameLocalized("OfferModel.CurrencyOid")]
        public string Currency { get; set; }
        [DisplayNameLocalized("OfferModel.PaymentTypeOid")]
        public string PaymentType { get; set; }
        [DisplayNameLocalized("OfferModel.InsuranceAreaType")]

        public string InsuranceAreaTypeDisplayValue { get; set; }
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

        [DisplayNameLocalized("OfferModel.TypeOfActivity")]
        
        public string TypeOfActivity { get; set; }

        [DisplayNameLocalized("OfferModel.InsuranceArea")]
        
        public string InsuranceArea { get; set; }

        [DisplayNameLocalized("OfferModel.IsJobContainsHarmfulFactors")]
        
        public string IsJobContainsHarmfulFactors { get; set; }

        [DisplayNameLocalized("OfferModel.NumberOfOfficeWorkers")]
        
        public int? NumberOfOfficeWorkers { get; set; }

        [DisplayNameLocalized("OfferModel.NumberOfProductionWorkers")]
        
        public int? NumberOfProductionWorkers { get; set; }

        [DisplayNameLocalized("OfferModel.IsRenew")]
        [RequiredLocalized]
        public bool IsRenew { get; set; }
        
        [DisplayNameLocalized("OfferModel.PolicyNumber")]
        [RequiredIfLocalized(nameof(IsRenew), EqualTo = true)]
        public string ExistingPolicyNumber { get; set; }

        [ValidateComplexType]
        public IList<VoluntaryHealthInsuranceEmployeeGroupViewModel> VoluntaryHealthInsuranceEmployeeGroups { get; set; }

        public RequestModels.ClientType DealClientType { get; set; }
    }
}