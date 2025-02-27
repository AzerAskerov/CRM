using System;
using System.Runtime.Serialization;
using CRM.Operation.Attributes;
using CRM.Operation.Enums;

namespace CRM.Operation.Models.RequestModels
{
    [DataContract]
    public class EmailModel
    {
        [DataMember(Name = "from")]
        public string From { get; set; }

        [RequiredLocalized]
        [DataMember(Name = "to")]
        public string To { get; set; }

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [RequiredLocalized]
        [DataMember(Name = "body")]
        public string Body { get; set; }

        [RequiredLocalized]
        [DataMember(Name = "Isbodyhtml")]
        public bool IsBodyHtml { get; set; }

        [DataMember(Name = "system")]
        public byte SystemOid { get; set; }

        [DataMember(Name = "objectguid")]
        public Guid? ObjectGuid { get; set; }

        [DataMember(Name = "SubType")]
        public QueueItemSubtype? SubType { get; set; }
    }
}