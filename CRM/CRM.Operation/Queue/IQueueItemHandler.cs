using CRM.Data.Database;
using CRM.Operation.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Xml;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Queue
{
    /// <summary>
    /// Interface for tighter integration between queue item handler and QueueDispatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class QueueItemHandlerBase<T> : BusinessOperation<T>, IQueueItemHandler
        where T : BaseQueuePayload
    {

        public QueueItemHandlerBase(CRMDbContext db) : base(db)
        {

        }
        /// <summary>
        /// Queue item to be passed to the handler, if handler wants to manipulate it
        /// </summary>
        public CRM.Data.Database.Queue QueueItem { get; set; }

        /// <summary>
        /// Override configured failure action
        /// </summary>
        public QueueHandlerFailureType FailureType { get; set; }

        /// <summary>
        /// This method gets called when QueueDispatcher sets final status Failed to the queue item
        /// </summary>
        public virtual void OnFailure()
        {
        }

        /// <summary>
        /// ObjectId in dbo.TechnicalLog
        /// </summary>
        protected override Guid? ObjectLoggingId
        {
            get { return QueueItem.RelatedObjectId; }
        }

        /// <summary>
        /// Handler XML node to be passed to the handler, if handler wants to read specific configuration from it
        /// </summary>
        public IConfigurationSection HandlerNode { get; set; }
    }

    public interface IQueueItemHandler
    {
        Data.Database.Queue QueueItem { get; set; }

        QueueHandlerFailureType FailureType { get; set; }

        void OnFailure();

        IConfigurationSection HandlerNode { get; set; }
    }
}
