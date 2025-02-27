using Newtonsoft.Json;

namespace CRM.Client.Models
{
    public class ODataModel<T>
    {
        [JsonProperty("@odata.context")]
        public string MetaData { get; set; }

        [JsonProperty("@odata.count")]
        public int Count { get; set; }

        [JsonProperty("value")]
        public T Value;
    }
}
