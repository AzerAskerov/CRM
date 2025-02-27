using System.Net.Http;
using CRM.Operation.Helpers;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;

namespace CRM.Operation
{
    /// <summary>
    /// Creates shortcut for files based on given parameters.
    /// </summary>
    public class CreateShortcutForFilesOperation : Zircon.Core.OperationModel.Operation<CreateShortcutForFilesRequest>
    {
        protected override void DoExecute()
        {
            using HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms();
            
            var requestUrl = "File/CreateShortcutForFiles";

            var operationResult = Zircon.Core.HttpContextHelper.HttpClientHelper
                .PostJsonAsync<CreateShortcutForFilesRequest, object>(httpClient, requestUrl, Parameters).Result;

            Result.Merge(operationResult);
        }
    }
}