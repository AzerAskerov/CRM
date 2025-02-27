using System.Text.Json;
using Zircon.Core.Config;

namespace CRM.Operation.Helpers
{
    /// <summary>
    /// Returns the shorter version of a link.
    /// </summary>
    public static class ShortenLinkHelper
    {
        public static string EncodeLink(string originalLink)
        {
            using (var httpClient = HttpClientHelper.GetHttpClientForWebIms())
            {
                var response =
                    httpClient.GetAsync(
                        $"{Settings.GetString("BaseUrl")}ShortenLink/EncodeLink?originalLink={originalLink}");

                var result = JsonSerializer.Deserialize(response.Result.Content.ReadAsStringAsync().Result,typeof(string));

                return result!.ToString();
            }
        }
    }
}