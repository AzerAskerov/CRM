using System;
using CRM.Data.Enums;


namespace CRM.Data
{
    public class Invoice
    {
        public Guid ClientGuid { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime CreateTimeStamp { get; set; }
        public Guid InvoiceGuid { get; set; }
        public string InvoiceNumber { get; set; }
        public InvoiceStatusCode StatusCode { get; set; }
        public string StatusText{get;set;}
        public decimal AmountPaid { get; set; }
        public decimal AmountTotal { get; set; }
    }
}
