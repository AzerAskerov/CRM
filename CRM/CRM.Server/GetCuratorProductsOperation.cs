using AntDesign.TableModels;
using CRM.Data.Database;
using CRM.Operation;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zircon.Core.Authorization.SystemProvider;
using Zircon.Core.OperationModel;

namespace CRM.Server
{
    public class GetCuratorProductsOperation : BusinessOperation<CuratorProductRequest>
    {
        public List<CuratorProductModel> Model { get; set; }
        protected CRMDbContext _context;
        public GetCuratorProductsOperation(CRMDbContext db) : base(db)
        {
            _context = db;
        }
        protected override void DoExecute()
        {
            Model = new List<CuratorProductModel>();
            QueryModel queryModel = new QueryModel();
            queryModel.Parameters = GenerateQueryParameters();
            queryModel.DataType = "json";
            queryModel.ActionName = "GetCuratorProductsForClient";
            QueryProxyOperation operation = new QueryProxyOperation(_context);
            operation.Execute(queryModel);

            if (!operation.Result.Success)
            {
                Result.Merge(operation.Result);
                return;
            }

            var list = JsonConvert.DeserializeObject<QueryResultItems<CuratorProductModel>>(operation.Model.Data);
            Model = list?.Items;

        }

        protected virtual string GenerateQueryParameters()
        {
            return "{\"inn\":\"" + Parameters.Inn  +"\"}";
        }
    }
}
