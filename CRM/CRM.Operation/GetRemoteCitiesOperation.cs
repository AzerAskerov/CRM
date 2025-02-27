using CRM.Data.Database;
using CRM.Operation.Helpers;
using CRM.Operation.Models.RemoteApi;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zircon.Core.Config;
using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

namespace CRM.Operation
{
    public class GetRemoteCitiesOperation : BusinessOperation<string>
    {
        public List<string> Cities = new List<string>() { "CityNotSelected".Translate() };
        CRMDbContext context;

        public GetRemoteCitiesOperation(CRMDbContext db) : base(db)
        {
            context = db;
        }

        protected override void DoExecute()
        {
            var url = Settings.GetString("RemoteCityApi");
            HttpClient httpClient = new HttpClient();

            var json = RemoteApiHelper.GetValidParameter(Parameters);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var api = Task.Run(async () =>
                await httpClient.PostAsync(url, stringContent)).Result;
            var response = Task.Run(async () => await api.Content.ReadAsStringAsync()).Result;
            var deserializedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<City>(response);


            if (deserializedResponse != null && deserializedResponse.Error)
            {
                Result.AddError(string.Format("NoCityFound".Translate(), Parameters));
                return;

            }

            foreach (var city in deserializedResponse?.Data)
            {
                if (city != null)
                {
                    Cities.Add(city);
                }
            }

        }
    }
}
