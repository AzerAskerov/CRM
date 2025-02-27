
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CRM.Operation.Localization
{
    public static class ResourceManager
    {
        private static Localize localized;

        public static Localize Localized
        {
            get { return localized; }
        }

        public async static Task GetLocalize(HttpClient client, Blazored.SessionStorage.ISessionStorageService sessionStorage)
        {
            try
            {
                string storagedLocalized = await sessionStorage.GetItemAsync<string>("localized");
                if (storagedLocalized == null)
                {
                    localized = await Localize.Create(client);
                    await sessionStorage.SetItemAsync("localized", JsonConvert.SerializeObject(localized));
                }
                else
                {
                    if (localized == null)
                        localized = JsonConvert.DeserializeObject<Localize>(await sessionStorage.GetItemAsync<string>("localized"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("excecee " + ex.Message);
            }
        }

        public static string GetText(string resId, string defaultText)
        {
            return localized?.GetText(resId, defaultText);
        }

    }
}
