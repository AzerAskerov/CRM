using System.Net.Http;
using System.Text;
using CRM.Operation.Helpers;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using Newtonsoft.Json;
using Zircon.Core.OperationModel;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CRM.Operation
{
    /// <summary>
    /// Archives files.
    /// </summary>
    public class ArchiveFilesOperation : Zircon.Core.OperationModel.Operation<string>
    {
        protected override void DoExecute()
        {
            using HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms();
            
            var requestUrl = "File/ArchiveFiles";

            var operationResult = Zircon.Core.HttpContextHelper.HttpClientHelper
                .PostJsonAsync<FileManagementGetIFrameLinkRequest, object>(httpClient, requestUrl, new FileManagementGetIFrameLinkRequest(){Token = Parameters}).Result;

            Result.Merge(operationResult);
        }
    }
}