using Zircon.Core.OperationModel;

namespace CRM.Operation.Models
{
    public interface IVehicleModel : IModel
    {
        int? VehBrandOid { get; set; }

        int? VehModelOid { get; set; }

    }
}