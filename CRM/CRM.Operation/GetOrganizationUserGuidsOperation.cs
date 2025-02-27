using CRM.Data.Database;
using CRM.Operation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Zircon.Core.Authorization;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetOrganizationUserGuidsOperation : BusinessOperation
    {
        private CRMDbContext CrmDbContext;
        public List<UserGuids> UserGuids_ { get; set; }

        public GetOrganizationUserGuidsOperation(CRMDbContext dbContext) : base(dbContext)
        {
            CrmDbContext = dbContext;
        }

        protected override void DoExecute()
        {
            QueryModel queryModel = new QueryModel();
            queryModel.Parameters = "{\"userGuid\":\"" + CommonUserContextManager.CurrentUserGuid + "\"}";
            queryModel.DataType = "json";
            queryModel.ActionName = "Deals.FetchOrgUserGuids";
            QueryProxyOperation operation = new QueryProxyOperation(CrmDbContext);
            operation.Execute(queryModel);
            if (operation.Result.Success)
            {
                UserGuids_ = new List<UserGuids>();
                var list = JsonConvert.DeserializeObject<Item>(operation.Model.Data);
                UserGuids_ = list?.Items;
            }
        }


    }
}
