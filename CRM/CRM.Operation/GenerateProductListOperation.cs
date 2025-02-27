using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using Zircon.Core.OperationModel;

using CRM.Data.Database;
using CRM.Data.Models;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Enums;

namespace CRM.Operation
{
    public class GenerateProductListOperation<T> : BusinessOperation<T>
    where T : ClientContract
    {
        public List<ProductBaseModel> Model { get; set; }
        protected CRMDbContext _context;
        public GenerateProductListOperation(CRMDbContext db) : base(db)
        {
            _context = db;
        }
        protected override void DoExecute()
        {
            Model = new List<ProductBaseModel>();

            QueryModel queryModel = new QueryModel();
            queryModel.Parameters = GenerateQueryParameters();
            queryModel.DataType = "json";
            queryModel.ActionName = "GetClientPoliciesWithArchiveStatus";
            QueryProxyOperation operation = new QueryProxyOperation(_context);
            operation.Execute(queryModel);

            if (!operation.Result.Success)
            {
                Result.Merge(operation.Result);
                return;
            }

            var list = JsonConvert.DeserializeObject<QueryResultItems<Policy>>(operation.Model.Data);
            List<Policy> output = list?.Items;

            if (output != null)
            {
                foreach (Policy row in output)
                {
                    Model.Add(new ProductBaseModel(row));
                }
            }
        }

        protected virtual string GenerateQueryParameters()
        {
            return "{\"client_code\":\"" + GetClientCode() + "\",\"policy_number\":\"null\"}";
        }

        protected string GetClientCode()
        {
            string code = string.Empty;

            if (Parameters.ClientType == ClientType.Pyhsical)
            {
                code = _context.PhysicalPeople.FirstOrDefault(x => x.Inn == Parameters.INN && (x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now))?.Pin;
            }

            if (string.IsNullOrEmpty(code))
            {
                code = _context.Documents.FirstOrDefault(x => x.Inn == Parameters.INN && x.DocumentType == (int)DocumentTypeEnum.Tin
                    && (x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now))
                    ?.DocumentNumber;
            }

            return code;
        } 
    }
}
