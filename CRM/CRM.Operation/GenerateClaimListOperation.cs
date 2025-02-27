using System;
using System.Linq;
using Newtonsoft.Json;

using Zircon.Core.OperationModel;

using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Enums;

namespace CRM.Operation
{
    public class GenerateClaimListOperation : BusinessOperation<ClientContract>
    {
        public ClaimModel Model { get; set; }
        private CRMDbContext _context;
        public GenerateClaimListOperation(CRMDbContext db) : base(db)
        {
            _context = db;
        }

        protected override void DoExecute()
        {
            string code = "";

            if (Parameters.ClientType == ClientType.Pyhsical)
            {
                code = _context.PhysicalPeople.FirstOrDefault(x => x.Inn == Parameters.INN && (x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now)).Pin;
            }
            else
            {
                code = _context.Documents.FirstOrDefault(x => x.Inn == Parameters.INN && x.DocumentType == (int)DocumentTypeEnum.Tin
                 && (x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now)).DocumentNumber;
            }

            QueryModel queryModel = new QueryModel();
            queryModel.Parameters = "{\"clientcode\":\"" + code + "\"}";
            queryModel.DataType = "json";
            queryModel.ActionName = "GetClientClaim";
            QueryProxyOperation operation = new QueryProxyOperation(_context);
            operation.Execute(queryModel);
            Model = new ClaimModel();
            var list = JsonConvert.DeserializeObject<QueryResultItems<ClaimListItemModel>>(operation.Model.Data);
            Model.Claims = list?.Items;
            Model.AccidentCount = Model.Claims.ToList().Count();

            queryModel.Parameters = "{\"code\":\"" + code + "\"}";
            queryModel.DataType = "json";
            queryModel.ActionName = "GetNonClaimDayCount";
            operation = new QueryProxyOperation(_context);
            operation.Execute(queryModel);

            var days = JsonConvert.DeserializeObject<QueryResultItems<ClaimModel>>(operation.Model.Data);
            Model.NonClaimDayCount = days.Items.FirstOrDefault()?.NonClaimDayCount ?? 0;
        }
    }
}
