using System.Net.Http;
using CRM.Operation.Helpers;
using CRM.Operation.Models.RequestModels;

namespace CRM.Operation
{
    /// <summary>
    /// Puts email to email queue on webims.
    /// </summary>
    public class SendEmailOperation : Zircon.Core.OperationModel.Operation<EmailModel>
    {
        protected override void DoExecute()
        {
            using HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms();
            
            var requestUrl = "Messaging/SendEmail";

            var operationResult = Zircon.Core.HttpContextHelper.HttpClientHelper
                .PostJsonAsync<EmailModel, object>(httpClient, requestUrl, Parameters).Result;

            Result.Merge(operationResult);
        }
    }
}