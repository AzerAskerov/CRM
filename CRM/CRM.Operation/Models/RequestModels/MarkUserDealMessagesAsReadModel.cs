using System;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.RequestModels
{
    public class MarkUserDealMessagesAsReadModel : IModel
    {
        public Guid UserGuid { get; set; }
        public Guid DealGuid { get; set; }
    }
}