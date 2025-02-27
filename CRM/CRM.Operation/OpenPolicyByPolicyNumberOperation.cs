using AutoMapper;
using CRM.Data.Database;
using CRM.Data.Models;
using CRM.Operation.Helpers;
using CRM.Operation.Models;
using CRM.Operation.Models.RemoteApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Zircon.Core.Config;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class OpenPolicyByPolicyNumberOperation : BusinessOperation<string>
    {
        private readonly CRMDbContext _dbContext;
        public PolicyActionGuidModel Model { get; set; } = new PolicyActionGuidModel();

        public OpenPolicyByPolicyNumberOperation(CRMDbContext db) : base(db)
        {
            _dbContext = db;
        }
        protected override void DoExecute()
        {
            using HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms();

            var requestUrl = $"Policy/GetPolicyByPolicyNumber?policyNr={Parameters}";

            var operationResult = httpClient.GetAsync(requestUrl).Result;

            if (operationResult.IsSuccessStatusCode)
            {
                var responseContent = operationResult.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<Policy>(responseContent.Result);
                Model.PolicyActionGuid = model?.PolicyActionGuid.ToString();
            }


            if (string.IsNullOrEmpty(Model.PolicyActionGuid))
            {
                Result.AddError("PolicyNotFound");
            }
            Model.BaseUrl = Settings.GetString("ImsDomain");
        }
    }
}
