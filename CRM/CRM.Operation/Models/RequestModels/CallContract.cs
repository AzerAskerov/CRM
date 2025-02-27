using CRM.Operation.Attributes;
using CRM.Operation.SourceLoaders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Zircon.Core.Attributes;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.RequestModels
{
    [DataContract]
    public class CallContract : IModel
    { 
        [Key]
        [DataMember(Name = "id")]
        [IgnoreDataMember]
        public int? Id { get; set; }

        [DataMember(Name = "calltimestamp")]
        [CustomSource(typeof(ContactTypeSourceLoader))]
        public DateTime CallTimestamp { get; set; }

        [DataMember(Name = "responsibleperson")]
        public string ResponsibleNumber { get; set; }

        [DataMember(Name = "direction")]
        public string Direction { get; set; }
    }
}
