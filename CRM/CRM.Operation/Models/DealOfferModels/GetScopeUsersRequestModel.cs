using CRM.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Models.DealOfferModels
{
    public class GetScopeUsersRequestModel
    {
        public UserAccessScope Scope { get; set; }
        public bool IsSeller { get; set; }
        public int? ClientInn { get; set; }
        public string FullName { get; set; }
        public string DiscussionText { get; set; }
        public string MediatorUserName { get; set; }
        public string UnderwriterUserName { get; set; }
        public string PinNumber { get; set; }
        public string DealNumber { get; set; }
        public string DocumentNumber { get; set; }
        public int DealStatus { get; set; }
        public int DealType { get; set; }
    }
}
