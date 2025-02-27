using System;
using System.Collections.Generic;
using System.Text;
using Zircon.Core.Authorization;

namespace CRM.Operation.Models
{
    public class DealScopeModel
    {
        public List<UserGuids> Guids { get; set; }
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
