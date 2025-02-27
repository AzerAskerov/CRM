using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Cache
{
    public class DataCaching<T>
    {


        public async static Task Cache(T data, string cachingKey, Blazored.SessionStorage.ISessionStorageService sessionStorage)
        {
            try
            {
                string storagedData = await sessionStorage.GetItemAsync<string>(cachingKey);

                if (storagedData == null)
                {
                    await sessionStorage.SetItemAsync(cachingKey, JsonConvert.SerializeObject(data));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caching exception: " + ex.Message);
                Console.WriteLine("Caching exception: " + ex?.InnerException?.Message);
            }
        }



        public static async Task<T> GetCachedData(string cachingKey, Blazored.SessionStorage.ISessionStorageService sessionStorage)
        {
            T data = default(T);
            var key = await sessionStorage.GetItemAsync<string>(cachingKey);

            if (key != null)
            {
                data = JsonConvert.DeserializeObject<T>(await sessionStorage.GetItemAsync<string>(cachingKey));
            }

            return data;
        }



    }
}
