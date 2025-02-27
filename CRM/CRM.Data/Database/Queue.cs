using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace CRM.Data.Database
{
    public partial class Queue
    {
        public int Id { get; set; }
        public byte TypeOid { get; set; }
        public short SubtypeOid { get; set; }
        public byte Priority { get; set; }
        public short RetryCount { get; set; }
        public byte StatusOid { get; set; }
        public byte SystemOid { get; set; }
        public string Recipient { get; set; }
        public Guid? RelatedObjectId { get; set; }
        public DateTime? ProcessAfter { get; set; }
        public DateTime? LastProcessedOn { get; set; }
        public string LastErrorText { get; set; }
        public string Payload { get; set; }
        public short? Version { get; set; }
        //always set this
        [NotMapped]
        public XmlDocument PayloadData
        {
            get
            {
                if (string.IsNullOrEmpty(Payload)) return null;
                var xDoc = new XmlDocument();
                xDoc.LoadXml(Payload);
                return xDoc;
            }
            set
            {
                if (value == null) return;
                Payload = value.OuterXml;

            }
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedTimestamp { get; set; }
    }
}
