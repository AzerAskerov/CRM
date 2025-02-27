using CRM.Data.Database;
using CRM.Operation.Models;
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
    public class GetRemoteCountriesOperation : BusinessOperation
    {
        public List<string> Countries = new List<string>()
        {
            "CountryNotSelected".Translate()
        };

        public RemoteCountryModel Data = null;
        CRMDbContext context;


        public GetRemoteCountriesOperation(CRMDbContext db) : base(db)
        {
            context = db;
        }

        protected override void DoExecute()
        {
            var url = Settings.GetString("RemoteCountryApi");
            HttpClient httpClient = new HttpClient();
            string json = @"{""type"": ""long"", ""min"": 1,""max"": 500}";
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var api = Task.Run(async () =>
                await httpClient.GetAsync(url)).Result;
            var response = Task.Run(async () => await api.Content.ReadAsStringAsync()).Result;
            var deserializedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RemoteCountryModel>(response);


            Data = deserializedResponse;



        }
    }
}
