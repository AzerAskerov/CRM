using System.Collections.Generic;
using Newtonsoft.Json;

namespace CRM.Operation.Models
{
    public class QueryResultItems<T>
    {
        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}
