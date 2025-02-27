using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CRM.Data.Database
{
    public partial class Offer
    {
        public Offer()
        {
            DealPolicyLink = new HashSet<DealPolicyLink>();
        }
        
        public int Id { get; set; }
        public Guid DealGuid { get; set; }
        public string OfferNumber { get; set; }
        public int OfferPeriodTypeOid { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool? IsAgreed { get; set; }
        public string SubmitAgreedOffer { get; set; }
        public string Notes { get; set; }
        public string PlannedMaliciousness { get; set; }
        public virtual Deal Deal { get; set; }
        public virtual ICollection<DealPolicyLink> DealPolicyLink { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
