using CRM.Data.Database;
using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class CommentsWithCreatorsOperation : BusinessOperation<CommentsWithCreatorRequestModel>
    {
        private CRMDbContext _db;
        public List<CommentContract> Comments { get; set; } = new List<CommentContract>();
        public CommentsWithCreatorsOperation(CRMDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
        protected override void DoExecute()
        {

            QueryModel queryModel = new QueryModel();
            queryModel.Parameters = "{\"pin\":\"" + Parameters.Pin + "\"}";
            queryModel.DataType = "json";
            queryModel.ActionName = "CommentsWithCreator";
            QueryProxyOperation operation = new QueryProxyOperation(_db);
            operation.Execute(queryModel);

            var list = JsonConvert.DeserializeObject<QueryResultItems<CommentContract>>(operation.Model.Data);
            Comments.AddRange(list?.Items);
        }
    }
}
