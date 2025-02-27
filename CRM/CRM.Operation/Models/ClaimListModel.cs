using CRM.Operation.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models
{
    public class ClaimListItemModel
    {
        [DisplayNameLocalized("ClaimListItemModel.AppNumber")]
        public string AppNumber { get; set; }
        [DisplayNameLocalized("ClaimListItemModel.PartNo")]
        public int PartNo { get; set; }
        [DisplayNameLocalized("ClaimListItemModel.CardNumber")]
        public string PartName { get; set; }
        
        public bool Active { get; set; }
        
        public string StatusVariant { get; set; }
        [DisplayNameLocalized("ClaimListItemModel.StatusText")]
        public string StatusText { get; set; }
       
        public int Status { get; set; }
        [DisplayNameLocalized("ClaimListItemModel.Description")]
        public string Description { get; set; }
        [DisplayNameLocalized("ClaimListItemModel.PolicyNumber")]
        public string PolicyNumber { get; set; }
        [DisplayNameLocalized("ClaimListItemModel.AccidentDate")]
        public DateTime AccidentDate { get; set; }
    }
}
