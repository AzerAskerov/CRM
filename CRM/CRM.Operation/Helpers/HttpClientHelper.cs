using System;
using System.Net.Http;
using Zircon.Core.Config;

namespace CRM.Operation.Helpers
{
    public static class HttpClientHelper
    {
        /// <summary>
        /// Returns http client that provides baseUrl of 'WebIms', and default authentication.
        /// </summary>
        /// <remarks>
        /// This Helper cannot be used in Client side blazor.
        /// </remarks>
        /// <returns></returns>
        public static HttpClient GetHttpClientForWebIms()
        {
            string BaseUrl = Settings.GetString("BaseUrl"); 
            string username = Settings.GetString("CONFIG_WEBSERVICES_USER");
            string password = Settings.GetString("CONFIG_WEBSERVICES_PASSWORD");

            HttpClient httpClient = new HttpClient {BaseAddress = new Uri(BaseUrl)};
            
            httpClient.DefaultRequestHeaders.Add("username",username);
            httpClient.DefaultRequestHeaders.Add("password",password);

            return httpClient;
        }
    }
}