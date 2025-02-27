using Blazored.LocalStorage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Cache
{
    public class DataCachingLocal<T>
    {


        public async static Task Cache(T data, string cachingKey, ILocalStorageService sessionStorage)
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



        public static async Task<T> GetCachedData(string cachingKey, ILocalStorageService sessionStorage)
        {
            T data = default(T);
            try
            {
                var key = await sessionStorage.GetItemAsync<string>(cachingKey);
                if (key != null)
                {
                    data = JsonConvert.DeserializeObject<T>(await sessionStorage.GetItemAsync<string>(cachingKey));
                }

                return data;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
                return data;
            }
           
        }


        public async static Task UpdateCachedData(T data, string cachingKey, ILocalStorageService sessionStorage)
        {
            try
            {
                await sessionStorage.RemoveItemAsync(cachingKey);

                
               await sessionStorage.SetItemAsync(cachingKey, JsonConvert.SerializeObject(data));
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caching exception: " + ex.Message);
                Console.WriteLine("Caching exception: " + ex?.InnerException?.Message);
            }
        }

        public async static Task RemoveFromCache(T cachingKey, ILocalStorageService sessionStorage)
        {
            try
            {
                var storagedData = await sessionStorage.GetItemAsync<string>(cachingKey.ToString());

                if (storagedData != null)
                {
                    await sessionStorage.RemoveItemAsync(cachingKey.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Caching exception: " + ex.Message);
                Console.WriteLine("Caching exception: " + ex?.InnerException?.Message);
            }
        }


    }
}
