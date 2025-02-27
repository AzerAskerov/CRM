using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
    public partial class LeadObject : BaseEntity
    {
        public string UniqueKey { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string CampaignName { get; set; }
        public string Payload { get; set; }
        public string ObejctId { get; set; }
        public Guid UserGuid { get; set; }
        public virtual ClientRef ClientRef { get; set; }

    }
}
