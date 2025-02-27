using System;
using CRM.Data.Enums;
using CRM.Data;
using CRM.Operation.Attributes;

namespace CRM.Operation.Models
{
    public class InvoiceModel
    {
        public Guid ClientGuid { get; set; }
        [DisplayNameLocalized("InvoiceModel.PolicyNumber")]
        public string PolicyNumber { get; set; }
        [DisplayNameLocalized("InvoiceModel.InvoiceNumber")]
        public string InvoiceNumber { get; set; }
        [DisplayNameLocalized("InvoiceModel.CreateTimeStamp")]
        public DateTime CreateTimeStamp { get; set; }
        public Guid InvoiceGuid { get; set; }
        
        public InvoiceStatusCode StatusCode { get; set; }
        [DisplayNameLocalized("InvoiceModel.AmountPaid")]
        public decimal AmountPaid { get; set; }
        [DisplayNameLocalized("InvoiceModel.AmountTotal")]
        public decimal AmountTotal { get; set; }
        [DisplayNameLocalized("InvoiceModel.StatusText")]
        public string StatusText { get; set; }

        public InvoiceModel()
        {

        }

        public InvoiceModel(Invoice inv)
        {
            ClientGuid = inv.ClientGuid;
            PolicyNumber = inv.PolicyNumber;
            CreateTimeStamp = inv.CreateTimeStamp;
            InvoiceGuid = inv.InvoiceGuid;
            InvoiceNumber = inv.InvoiceNumber;
            StatusCode = inv.StatusCode;
            StatusText = inv.StatusText;
            AmountPaid = inv.AmountPaid;
            AmountTotal = inv.AmountTotal;
        }
    }
}
