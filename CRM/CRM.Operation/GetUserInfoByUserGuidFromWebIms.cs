using System;
using System.Net.Http;
using CRM.Operation.Helpers;
using CRM.Operation.Models.Login;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetUserInfoByUserGuidFromWebIms : BusinessOperation<Guid>
    {
        public UserSummaryShort UserSummaryShortModel { get; private set; }
        public GetUserInfoByUserGuidFromWebIms(DbContext db = null) : base(db)
        {
        }

        protected override void DoExecute()
        {
            using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                var requestUrl = $"UserManagement/GetUserInfoByUserGuid?userGuid={Parameters}";

                var response = httpClient.GetAsync(requestUrl).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Result.AddError("Error while load user");
                }
                
                var responseAsJson = response.Content.ReadAsStringAsync().Result;
                
                UserSummaryShortModel = JsonConvert.DeserializeObject<UserSummaryShort>(responseAsJson);
            }
        }
    }
}