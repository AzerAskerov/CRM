using System;
using CRM.Data.Enums;

namespace CRM.Operation.Models.RequestModels
{
    public class ChangeDealStatusRequest
    {
        public Guid CurrentUserGuid { get; set; }
        public string CurrentUserName { get; set; }
        public Guid DealGuid { get; set; }
        public DealStatus DealStatus { get; set; }
    }
}