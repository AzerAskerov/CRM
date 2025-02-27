using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using Zircon.Core.OperationModel;

using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Enums;

namespace CRM.Operation
{
    public class GenerateInvoiceListOperation : BusinessOperation<ClientContract>
    {
        public List<InvoiceModel> Model { get; set; }
        private CRMDbContext _context;
        public GenerateInvoiceListOperation(CRMDbContext db) : base(db)
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
            queryModel.Parameters = "{\"client_code\":\"" + code + "\"}";
            queryModel.DataType = "json";
            queryModel.ActionName = "GetClientInvoices";
            QueryProxyOperation operation = new QueryProxyOperation(_context);
            operation.Execute(queryModel);

            var list = JsonConvert.DeserializeObject<QueryResultItems<InvoiceModel>>(operation.Model.Data);
            Model = list?.Items;
        }
    }
}
