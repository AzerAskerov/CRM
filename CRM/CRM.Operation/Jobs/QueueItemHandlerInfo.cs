using CRM.Operation.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml;


namespace CRM.Operation.Jobs
{
    [JsonObject]
    public class QueueItemHandlerInfo 
    {
        [JsonProperty("operation")]
        public string OperationTypeName { get; set; }

        internal Zircon.Core.OperationModel.Operation Operation { get; set; }

        internal MethodInfo Method { get; set; }

        [JsonProperty("type")]
        public QueueItemType ItemType { get; set; }

        [JsonProperty(PropertyName = "subTypes")]
        public string ItemSubTypes { get; set; }

        internal Type ParameterType { get; set; }

        [JsonProperty(PropertyName = "maxRetryCount")]
        public int MaxRetryCount { get; set; }

        [JsonProperty(PropertyName = "retryCoefficient")]
        public double RetryCoefficient { get; set; }

        [JsonProperty(PropertyName = "maxRetryPeriod")]
        public int MaxRetryPeriod { get; set; }

        [JsonProperty(PropertyName = "removeCompleted")]
        public bool RemoveCompleted { get; set; }

       
        public  IConfigurationSection HandlerNode { get; set; }
    }
}
