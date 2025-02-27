using CRM.Operation.Attributes;
using CRM.Operation.Enums;
using CRM.Operation.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class AssetViewModel
    {
        public InsuredObjectType AssetType { get; set; }


        public string AssetTypes
        {
            get
            {
                return AssetType.ToString();
            }
        }

        [DisplayNameLocalized("Assets.AssetType")]
        public string AssetTypeTranslation
        {
            get
            {
                return $"InsuredObjectType.{AssetType.ToString()}".Translate();
            }
        }


        [DisplayNameLocalized("Assets.AssetInfo")]
        public string AssetInfo { get; set; }


        [DisplayNameLocalized("Assets.AssetType")]
        public string AssetDescription { get; set; }
    }
}
