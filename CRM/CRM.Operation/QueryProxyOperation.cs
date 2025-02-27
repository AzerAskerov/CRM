using System.Net.Http;
using System.Threading.Tasks;

using Zircon.Core.Config;
using Zircon.Core.HttpContextHelper;
using Zircon.Core.OperationModel;

using CRM.Data.Database;
using CRM.Operation.Models;


namespace CRM.Operation
{
    public class QueryProxyOperation : Operation<QueryModel>
    {
        public QueryResult Model;
        public QueryProxyOperation(CRMDbContext db) : base(db)
        {
        }

        protected override void DoExecute()
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("username", Settings.GetString("QueryProxyUser"));
            httpClient.DefaultRequestHeaders.Add("password", Settings.GetString("QueryProxyUserPassword"));

            var resultquery = Task.Run(async () =>
                       await httpClient.PostJsonAsync<QueryModel, QueryResult>
                       ($"{Settings.GetString("BaseUrl")}/queryproxy/runscript", Parameters)).Result;

            if (!resultquery.Success)
            {
                Result.Merge(resultquery);
                return;
            }

            Model = resultquery.Model;
        }
    }
}
