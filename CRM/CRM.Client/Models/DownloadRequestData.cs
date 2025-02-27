using System.Text.Json.Serialization;

namespace CRM.Client.Models
{
    public class DownloadRequestData
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}