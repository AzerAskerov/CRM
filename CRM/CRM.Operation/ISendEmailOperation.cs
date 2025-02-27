using CRM.Operation.Models.RequestModels;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public interface ISendEmailOperation
    {
        public OperationResult SendEmail(EmailModel model)
        {
            return new SendEmailOperation().Execute(model);
        }
    }
}