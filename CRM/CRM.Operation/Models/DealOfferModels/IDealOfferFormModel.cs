using CRM.Operation.Attributes;
using CRM.Operation.JsonConverters;
using Zircon.Core.Enums;
using Zircon.Core.OperationModel;
using ClientType = CRM.Operation.Models.RequestModels.ClientType;

namespace CRM.Operation.Models.DealOfferModels
{
    [JsonInterfaceConverter(typeof(DealOfferFormJsonConverter))]
    public interface IDealOfferFormModel : IModel
    {
        public ClientType DealClientType { get; set; }
    }
}