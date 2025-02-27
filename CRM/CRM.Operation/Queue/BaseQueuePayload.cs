using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using CRM.Operation.Enums;

namespace CRM.Operation.Queue
{
    /// <summary>
    /// Base class for the specific queue message serialized classes
    /// </summary>
    public abstract class BaseQueuePayload
    {
        /// <summary>
        /// Gets or sets message priority. Reserverd for future use
        /// </summary>
        [XmlIgnore]
        public byte Priority { get; set; }

        /// <summary>
        /// Gets or sets type of the message
        /// </summary>
        [XmlIgnore]
        public QueueItemType Type { get; set; }

        /// <summary>
        /// Gets or sets subtype of the message
        /// </summary>
        [XmlIgnore]
        public QueueItemSubtype SubtypeOid { get; set; }

        /// <summary>
        /// Gets or sets sender system of the message. See Classifiers.Code table
        /// </summary>
        [XmlIgnore]
        public byte SystemOid { get; set; }

        /// <summary>
        /// Gets or sets recipient of the message
        /// </summary>
        [XmlIgnore]
        public string Recipient { get; set; }

        /// <summary>
        /// Gets or sets related object's guid of the message
        /// </summary>
        [XmlIgnore]
        public Guid? ObjectGuid { get; set; }

        /// <summary>
        /// Gets or sets a date and time after which queue monitor should start processing message
        /// </summary>
        [XmlIgnore]
        public DateTime? ProcessAfter { get; set; }

        [XmlElement("QueueId")]
        public long QueueId { get; set; }
    }
}
