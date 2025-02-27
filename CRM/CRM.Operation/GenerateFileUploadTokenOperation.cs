using System.Net.Http;
using System.Text;
using CRM.Operation.Helpers;
using CRM.Operation.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Zircon.Core.OperationModel;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CRM.Operation
{
    public class GenerateFileUploadTokenOperation : Operation<FileManagementGenerateTokenRequest>
    {
        public FileManagementGenerateTokenResponse Response;
        
        public GenerateFileUploadTokenOperation()
        {
        }

        protected override void DoExecute()
        {
            using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                var generateTokenUrl = "file/GenerateToken";

                var generateTokenResponse = httpClient.PostAsync(generateTokenUrl, new StringContent(JsonSerializer.Serialize(Parameters),Encoding.UTF8,"application/json")).Result;

                Response = JsonConvert.DeserializeObject<FileManagementGenerateTokenResponse>(generateTokenResponse.Content.ReadAsStringAsync().Result);
            }
        }
    }
}