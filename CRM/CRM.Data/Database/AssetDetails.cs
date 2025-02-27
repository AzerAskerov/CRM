using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Database
{
    public partial class AssetDetails
    {
        public int Id { get; set; }
        public Guid InsuredObjectGuid { get; set; }
        public string AssetInfo { get; set; }
        public string AssetDescription { get; set; }
        public int AssetType { get; set; }

        public virtual List<Asset> Assets { get; set; }
    }
}
