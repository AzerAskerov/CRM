using CRM.Operation.Attributes;
using CRM.Operation.SourceLoaders;
using System;
using System.Collections.Generic;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.DealOfferModels
{
    public class TenderModel: IModel
    {
        [DisplayNameLocalized("Tender.Note")]
        public string Note { get; set; }
        [CustomSource(typeof(TenderSourceLoader))]

        [DisplayNameLocalized("Tender.PurposeOid")]
        public int? PurposeOid { get; set; }
        public string Purpose { get; set; }
        public Guid DealGuid { get; set; }

        [DisplayNameLocalized("Tender.CreatedDate")]
        public DateTime? CreatedDate { get; set; }
    }
}
