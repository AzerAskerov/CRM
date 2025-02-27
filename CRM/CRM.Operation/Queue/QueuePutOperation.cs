using CRM.Data.Database;
using CRM.Operation.Enums;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Queue
{
    /// <summary>
    /// Puts a message into a queue
    /// </summary>
    /// <typeparam name="T">Specific type of item parameters</typeparam>
   
    public class QueuePutOperation<T> : BusinessOperation<T> where T : BaseQueuePayload
    {
        private CRMDbContext _db;
        public CRM.Data.Database.Queue Item { get; private set; }

        public QueuePutOperation(CRMDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// Check for duplicate
        /// </summary>
        protected override void Prepare()
        {
            Guid? relatedObjectId = Parameters.ObjectGuid ?? null;
            string recipient = Parameters.Recipient ?? "";

            //Item = _db.Queues.FirstOrDefault(q => q.TypeOid == (byte)Parameters.Type && q.SubtypeOid == (short)Parameters.SubtypeOid && (q.StatusOid == 1 || q.StatusOid == 2) &&
            //            q.RelatedObjectId == relatedObjectId && q.Recipient == recipient);

        }

        /// <summary>
        /// Put message into a queue
        /// </summary>
        protected override void DoExecute()
        {
            XmlDocument payload = QueueMessageSerializer.SerializeMessage(Parameters);
            //if (Item != null)
            //{
            //    if (Item.PayloadData == payload)
            //    {
            //        Result.AddWarning("QueueItem with the same data already added");
            //        return;
            //    }

            //}

            //int payloadFields = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length;

            Item = new CRM.Data.Database.Queue
            {
                TypeOid = (byte)Parameters.Type,
                SubtypeOid = (short)Parameters.SubtypeOid,
                Priority = Parameters.Priority,
                RetryCount = 0,
                StatusOid = (int)QueueItemStatus.Ready,
                SystemOid = Parameters.SystemOid,
                Recipient = Parameters.Recipient,
                RelatedObjectId = Parameters.ObjectGuid,
                ProcessAfter = Parameters.ProcessAfter,
                // if there are no custom fields in payload, then skip payload serialization altogether
                //Payload = payloadFields == 0 ? null : QueueMessageSerializer.SerializeMessage(Parameters)
                PayloadData = payload
            };

            _db.Queues.Add(Item);
            _db.SaveChanges();

            
        }

        protected override Guid? ObjectLoggingId
        {
            get { return Parameters.ObjectGuid; }
        }
    }
}