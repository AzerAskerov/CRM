using System;
using CRM.Operation.Attributes;
using CRM.Operation.SourceLoaders;
using Zircon.Core.Attributes;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.DealOfferModels
{
    public class OfferViewModel : IModel
    {
        public int Id { get; set; }
        public Guid DealGuid { get; set; }
        public string OfferNumber { get; set; }

        [CustomSource(typeof(OfferPeriodTypeSourceLoader))]
        [Attributes.RequiredLocalized]
        public int? OfferPeriodTypeOid { get; set; } = 1;
        
        [Attributes.RequiredIfLocalized(nameof(OfferPeriodTypeOid),EqualTo = 2)]
        public DateTime? StartDate { get; set; }
        [Attributes.RequiredIfLocalized(nameof(OfferPeriodTypeOid),EqualTo = 2)]
        public DateTime? ExpireDate { get; set; }

        public bool? IsAgreed { get; set; }
        public string FileUploadIFrameUrl { get; set; }
        public string FileManagementToken { get; set; }
        public bool IsReadOnly { get; set; }

        public string Notes { get; set; }
        public string PlannedMaliciousness { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}