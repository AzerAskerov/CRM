using CRM.Data.Database;
using CRM.Operation.Enums;
using CRM.Operation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GenerateMedicineListOperation : BusinessOperation<int>
    {
        public List<MedicineModel> Model { get; set; }
        private CRMDbContext DbContext;

        public GenerateMedicineListOperation(CRMDbContext db) : base(db)
        {
            DbContext = db;
        }

        protected override void DoExecute()
        {
            List<string> cardNumbers = DbContext.Documents.Where(x => x.Inn == Parameters && x.DocumentType == (int)DocumentTypeEnum.MedicineCard)?.Select(x => x.DocumentNumber).ToList();


            if (cardNumbers == null || cardNumbers.Count == 0)
            {
                return;
            }

            Model = new List<MedicineModel>();
            foreach (var cardNumber in cardNumbers)
            {
                QueryModel queryModel = new QueryModel();
                queryModel.Parameters = "{\"cardnumber\":\"" + cardNumber + "\"}";
                queryModel.DataType = "json";
                queryModel.ActionName = "GetClientMedicineService";
                QueryProxyOperation operation = new QueryProxyOperation(DbContext);
                operation.Execute(queryModel);
                
                var list = JsonConvert.DeserializeObject<QueryResultItems<MedicineModel>>(operation.Model.Data);
                Model.AddRange(list?.Items);

            }

        }
    }
}
