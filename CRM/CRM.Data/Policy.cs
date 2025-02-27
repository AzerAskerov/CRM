using System;
using CRM.Data.Enums;


namespace CRM.Data.Models
{
    public class Policy
    {
        public string CuratorName { get; set; }
        public string CuratorSurname { get; set; }
        public Guid? PolicyActionGuid { get; set; }
        public Guid? ClientGuid { get; set; }
        public string ClientFullName { get; set; }
        public string PolicyNumber { get; set; }
        public string Status { get; set; }
        public ArchiveStatusEnum ArchiveStatus { get; set; }
        public int AddendumCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Product { get; set; }
        public string Custom1 { get; set; }
        public short? Custom2 { get; set; }
        public string LobName { get; set; }
        public PolicyAction Type { get; set; }
        public PolicyActionStatus PolicyActionStatus { get; set; }
        public PolicyStatus PolicyStatus { get; set; }
        public InvoiceStatusCode InvStatusCode { get; set; }

    }
}
