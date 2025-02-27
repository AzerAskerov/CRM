using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class DealPolicyLink
    {
        public int Id { get; set; }
        public Guid DealGuid { get; set; }
        public int OfferId { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime LinkDate { get; set; }
        public Guid ByUserGuid { get; set; }

        public virtual Deal Deal { get; set; }
        public virtual Offer Offer { get; set; }
    }
}
