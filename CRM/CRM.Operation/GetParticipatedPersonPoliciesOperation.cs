using CRM.Data.Database;
using CRM.Data.Models;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetParticipatedPersonPoliciesOperation : BusinessOperation<string>
    {
        private CRMDbContext _context;
        public List<ParticipatedInsuredPersonPoliciesViewModel> Model { get; set; }

        public GetParticipatedPersonPoliciesOperation(CRMDbContext context):base(context)
        {
            _context = context;
        }

        protected override void DoExecute()
        {
          
            QueryModel queryModel = new QueryModel();
            queryModel.Parameters = "{\"code\":\"" + Parameters+ "\"}";
            queryModel.DataType = "json";
            queryModel.ActionName = "ParticipatedInsuredPersonPolicies";
            QueryProxyOperation operation = new QueryProxyOperation(_context);
            operation.Execute(queryModel);
            if (operation.Result.Success)
            {
              
                Model = new List<ParticipatedInsuredPersonPoliciesViewModel>();
                var list = JsonConvert.DeserializeObject<QueryResultItems<ParticipatedInsuredPersonPoliciesViewModel>>(operation.Model.Data);
                Model = list?.Items;
            }
        }
    }
}
