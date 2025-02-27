using System.Collections.Generic;
using System.Net.Http;
using CRM.Operation.Helpers;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Zircon.Core.Authorization;
using Zircon.Core.OperationModel;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CRM.Operation
{
    public class GetUsersWithPermissionParameterValueFromWebImsOperation : BusinessOperation<GetUsersWithPermissionParameterValueContract>
    { 
        public IEnumerable<UserSummaryShort> Users { get; private set; }

        public GetUsersWithPermissionParameterValueFromWebImsOperation(DbContext db = null) : base(db)
        {
        }

        protected override void DoExecute()
        {
            using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                var requestUrl = $"UserManagement/GetUsersWithPermissionParameterValue";

                var response = Zircon.Core.HttpContextHelper.HttpClientHelper.PostJsonAsync<GetUsersWithPermissionParameterValueContract,IEnumerable<UserSummaryShort>>(httpClient,requestUrl,Parameters).Result;

                if (!response.Success)
                {
                    Result.Merge(response);
                }
                
                Users = response.Model;
            }
        }
    }
}