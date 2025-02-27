using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CRM.Client.States
{
    public static class HttpClientHelper
    {
        public static async Task<Tout> PostJsonAsync<T, Tout>(this HttpClient httpClient, string url, T content)
        {
            var response = await httpClient.PostAsJsonAsync<T>(url, content);

            Tout result = await response.Content.ReadFromJsonAsync<Tout>();
            return result;
        }
    }
}
