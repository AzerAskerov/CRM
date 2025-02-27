using System;
using System.Text.Json.Serialization;
using CRM.Operation.Models.Login;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.DealOfferModels
{
    public class DiscussionViewModel : IModel
    {
        public int Id { get; set; }
        public Guid DealGuid { get; set; }
        public UserSummaryShort Sender { get; set; }
        public UserSummaryShort Receiver { get; set; }
        public Guid SenderGuid { get; set; }
        public Guid? ReceiverGuid { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        [JsonIgnore]
        public Guid CurrentUserGuid { get; set; }
        [JsonIgnore]
        public string CurrentUserName { get; set; }

        [JsonIgnore]
        public string SenderFullname => Sender?.FullName;
        [JsonIgnore]
        public string ReceiverFullname => Receiver is null ? string.Empty : Receiver.FullName;
    }
}