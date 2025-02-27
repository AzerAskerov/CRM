using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CRM.Operation.Models;
using Zircon.Core;
using Zircon.Core.Config;
using Zircon.Core.Extensions;
using Zircon.Core.HttpContextHelper;
using Zircon.Core.OperationModel;

namespace CRM.Operation.SourceLoaders
{
    public class VehicleModelsSourceLoader : SourceLoader<IVehicleModel>
    {
        private const int OtherModel = 999998;

        protected override IEnumerable<ISourceItem> DoLoad(IVehicleModel model)
        {
            if (  model==null || !model.VehBrandOid.HasValue)
                return default;
            
            using (HttpClient httpClient = Helpers.HttpClientHelper.GetHttpClientForWebIms())
            {
                var response = httpClient.GetJsonAsync<IEnumerable<SourceItem>>($"Classifiers/GetVehicleModels?brandId={model.VehBrandOid}").Result.Model;

                return response;
            }
        }

        public override ISourceItem GetItem(object key, IVehicleModel model)
        {
            if (OtherModel.Equals(key))
            {
                return CreateItem(OtherModel, "VehicleModelSourceLoader.OtherModel".Translate(), sequence: OtherModel);
            }

            var item = DoLoad(model).FirstOrDefault(s => s.Key.ToString().Equals(key.ToString()));

            return item != null
                ? CreateItem(item.Key, item.Value)
                : null;
        }
    }
}