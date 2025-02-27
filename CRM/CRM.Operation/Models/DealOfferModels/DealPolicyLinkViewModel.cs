using System;
using CRM.Operation.Attributes;
using CRM.Operation.Models.Login;

namespace CRM.Operation.Models.DealOfferModels
{
    public class DealPolicyLinkViewModel
    {
        public int Id { get; set; }
        public Guid DealGuid { get; set; }
        public int OfferId { get; set; }
        
        [DisplayNameLocalized("OfferNumber")]
        public string OfferNumber { get; set; }
        
        public DateTime LinkDate { get; set; }
        public Guid ByUserGuid { get; set; }
        public UserSummaryShort ByUser { get; set; }
        
        [DisplayNameLocalized("PolicyNumber")]
        public string PolicyNumber { get; set; }
        public ProductBaseModel PolicyModel { get; set; }

        public Guid CurrentUserGuid { get; set; }
        public string CurrentUserName { get; set; }
    }
}