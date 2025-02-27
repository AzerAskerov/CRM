using System.Collections.Generic;
using System.Net.Http;
using CRM.Operation.Helpers;
using CRM.Operation.Models.Login;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Zircon.Core.Authorization;
using Zircon.Core.OperationModel;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CRM.Operation
{
    public class GetUsersWithPermissionFromWebImsOperation : BusinessOperation<PermissionContract>
    { 
        public IEnumerable<UserSummaryShort> Users { get; private set; }

        public GetUsersWithPermissionFromWebImsOperation(DbContext db = null) : base(db)
        {
        }

        protected override void DoExecute()
        {
            using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                var requestUrl = $"UserManagement/GetUsersWithPermission";

                var response = Zircon.Core.HttpContextHelper.HttpClientHelper.PostJsonAsync<PermissionContract,IEnumerable<UserSummaryShort>>(httpClient,requestUrl,Parameters).Result;

                if (!response.Success)
                {
                    Result.AddError("Error while load user");
                }
                
                Users = response.Model;
            }
        }
    }
}