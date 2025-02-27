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
using HttpClientHelper = CRM.Operation.Helpers.HttpClientHelper;

namespace CRM.Operation.SourceLoaders
{
    public class VehicleBrandsSourceLoader : SourceLoader<IVehicleModel>
    {
        private const int OtherBrand = 999999;
        public static IEnumerable<SourceItem> alllist;

        protected override IEnumerable<ISourceItem> DoLoad(IVehicleModel model)
        {
            if (alllist == null || alllist?.Count() == 0)
            {
                using (HttpClient httpClient = HttpClientHelper.GetHttpClientForWebIms())
                {
                    alllist = httpClient.GetJsonAsync<IEnumerable<SourceItem>>("Classifiers/GetVehicleBrands").Result.Model;

                    return alllist;
                }
            }
            else
                return alllist;
           
        }

        public override ISourceItem GetItem(object key, IVehicleModel model)
        {
            if (OtherBrand.Equals(key))
            {
                return CreateItem(OtherBrand, "VehicleBrandSourceLoader.OtherBrand".Translate(), sequence: OtherBrand);
            }

            var item = DoLoad(model).FirstOrDefault(s => s.Key.ToString().Equals(key.ToString()));

            return item != null
                ? CreateItem(item.Key, item.Value)
                : null;
        }


    }
}