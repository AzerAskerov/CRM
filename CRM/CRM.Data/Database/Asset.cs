using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
    public partial class Asset
    {
        public int Id { get; set; }
        public int AssetDetailId { get; set; }
        public int Inn { get; set; }
        public string AssetInfo { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime? ValidFrom { get; set; }
        public virtual ClientRef ClientRef { get; set; }
        public virtual AssetDetails AssetDetail { get; set; }
    }
}
