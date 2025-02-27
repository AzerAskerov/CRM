using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Zircon.Core.Extensions;
using Zircon.Core.OperationModel;

using CRM.Data.Database;
using CRM.Operation.Enums;
using CRM.Operation.Queue;
using Zircon.Core.DB;

namespace CRM.Operation.Jobs
{
    public class QueueJob : AbstractJob
    {
        private string emailTo;
        private int? maxCount;
        private Dictionary<QueueItemType, QueueItemHandlerInfo> handlers;
        private IServiceProvider _serviceProvider;

        public QueueJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Start(IEnumerable<IConfigurationSection> config, DbContext dbContext)
        {
            emailTo = config.FirstOrDefault(x => x.Key == "EmailTo")?.Value;//GetValue<string>("EmailTo");
            maxCount = config.FirstOrDefault(x => x.Key == "MaxCount") != null ?
                        Convert.ToInt32(config.FirstOrDefault(x => x.Key == "MaxCount").Value) : (int?)null;//GetValue<int>("MaxCount");

            handlers = new Dictionary<QueueItemType, QueueItemHandlerInfo>();


            foreach (IConfigurationSection handler in config.FirstOrDefault(x => x.Key == "QueueHandlers").GetChildren())
            {
                Dictionary<string, string> configs = new Dictionary<string, string>();
                handler.GetChildren().ToList().ForEach(x => configs.Add(x.Key, x.Value));

                QueueItemHandlerInfo handlerConfig = JsonConvert.DeserializeObject<QueueItemHandlerInfo>(JsonConvert.SerializeObject(configs));

                if (handlerConfig.MaxRetryCount == 0)
                    handlerConfig.MaxRetryCount = int.MaxValue;
                if (handlerConfig.MaxRetryPeriod == 0)
                    handlerConfig.MaxRetryPeriod = int.MaxValue;
                if (handlerConfig.RetryCoefficient == 0.0)
                    handlerConfig.RetryCoefficient = 1.0;

                var operationType = Type.GetType(handlerConfig.OperationTypeName);
                if (operationType == null)
                {
                    throw new TypeLoadException(string.Format(CultureInfo.InvariantCulture, "Type {0} cannot be resolved", handlerConfig.OperationTypeName));
                }

                //handlerConfig.Operation = Activator.CreateInstance(operationType, dbContext) as Zircon.Core.OperationModel.Operation;
                //if (handlerConfig.Operation == null)
                //{
                //    throw new InvalidCastException(operationType + " must inherit from Operation");
                //}

                handlerConfig.ParameterType = operationType.BaseType.GetGenericArguments()[0];
                handlerConfig.Method = operationType.GetMethod("Execute", new[] { handlerConfig.ParameterType });
                if (handlerConfig.Method == null)
                {
                    throw new MissingMethodException(operationType.Name, "Execute(T param)");
                }

                handlerConfig.HandlerNode = handler;
                handlers.Add(handlerConfig.ItemType, handlerConfig);
            }
        }

        public override bool DoRun(bool isTestMode, IEnumerable<IConfigurationSection> config)
        {

            Start(config, null);

            var optionsBuilder = new DbContextOptionsBuilder<CRMDbContext>();
            optionsBuilder.UseSqlServer(DBConnectionManager.ConnectionString);
            CRMDbContext queueDb = new CRMDbContext(optionsBuilder.Options);

            var errorMessage = new StringBuilder();
            try
            {
                var configuredTypeOids = handlers.Keys.Select(k => (int)k);
                //var configuredSybTypeOids = handlers.Values.SelectMany(k => (k.ItemSubTypes ?? "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                //                                           .Distinct()
                //                                           .Select(k => (int)(QueueItemSubtype)Enum.Parse(typeof(QueueItemSubtype), k));


                var queueItems = queueDb.Queues.Where(x => configuredTypeOids.Contains(x.TypeOid) && /*configuredSybTypeOids.Contains(x.SubtypeOid) &&*/
(x.StatusOid == 1 || x.StatusOid == 2) && (x.ProcessAfter == null || x.ProcessAfter < DateTime.Now)
).OrderBy(x => x.Priority).ThenByDescending(x => x.Id).Take((int)50).ToList();



                foreach (var queueItem in queueItems)
                {
                    //Log.Write(string.Format("Processing queue item '{0}'", queueItem.Id));

                    var optionsBuilder1 = new DbContextOptionsBuilder<CRMDbContext>();
                    optionsBuilder1.UseSqlServer(DBConnectionManager.ConnectionString);

                    using (CRMDbContext db = new CRMDbContext(optionsBuilder1.Options))
                    {
                        try
                        {
                            var handlerInfo = handlers[(QueueItemType)queueItem.TypeOid];

                            var parameters = QueueMessageSerializer.DeserializeMessage(queueItem.PayloadData, handlerInfo.ParameterType);

                            //here queueid setted as payload property to use in operations
                            handlerInfo.ParameterType.GetProperty("QueueId").SetValue(parameters, queueItem.Id);

                            if (queueItem.Payload == null)
                            {
                                parameters = Activator.CreateInstance(handlerInfo.ParameterType);
                            }
                            else if (parameters == null)
                            {
                                var errorText = string.Format("Unable to deserialize payload[{1}] for QueueItem {0}", queueItem.Id, handlerInfo.ParameterType.Name);
                                //Log.WriteError(errorText);
                                errorMessage.AppendLine(errorText);
                                continue;
                            }

                            var operationType = Type.GetType(handlerInfo.OperationTypeName);
                            var operation = Activator.CreateInstance(operationType, db) as Zircon.Core.OperationModel.Operation;

                            IQueueItemHandler handler = operation as IQueueItemHandler;
                            handler.QueueItem = queueItem;
                            handler.FailureType = QueueHandlerFailureType.Default;
                            handler.HandlerNode = handlerInfo.HandlerNode;

                            // Executing handler operation with a deserialized parameters
                            var result = (OperationResult)handlerInfo.Method.Invoke(operation, new[] { parameters });

                            UpdateMessage(queueItem, handlerInfo, handler, result);
                            FinalizeMessage(queueItem, handlerInfo, queueDb);
                            queueDb.SaveChanges();
                            //transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            var exceptionText = string.Format("Exception: \n\r{0}", ex);
                            //Log.WriteError(exceptionText);
                            errorMessage.AppendLine(exceptionText);
                            //transaction.Rollback();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                var exceptionText = string.Format("Exception: \n\r{0}", ex);
                //Log.WriteError(exceptionText);
                errorMessage.AppendLine(exceptionText);
                //transaction.Rollback();
            }
            finally
            {
                //var finalMessage = errorMessage.ToString().Trim();
                //if (!string.IsNullOrEmpty(emailTo) && !string.IsNullOrEmpty(finalMessage))
                //{
                //    var emailFrom = Settings.GetString("BGRDSERVICES_MAIL_FROM");
                //    var mailer = new Mailer(emailFrom, emailTo, "QueueDispatcher information.", false);
                //    mailer.Send(finalMessage);
                //}
            }
            //}
            //}
            //}
            return true;
        }

        private void FinalizeMessage(CRM.Data.Database.Queue message, QueueItemHandlerInfo handlerInfo, CRMDbContext queueDb)
        {
            message.LastProcessedOn = DateTime.Now;
            if (message.StatusOid.In((byte)QueueItemStatus.Failed, (byte)QueueItemStatus.Processed) && handlerInfo.RemoveCompleted)
            {
                queueDb.Queues.Remove(message);
            }
        }

        ///// <summary>
        ///// Deals with deciding if message processing was succesfull, or not. 
        ///// In future handlers themselves might be able to decide that, because they have more knowledge of processing specifics.
        ///// </summary>
        private void UpdateMessage(CRM.Data.Database.Queue message, QueueItemHandlerInfo handlerInfo, IQueueItemHandler handler, OperationResult result)
        {
            var maxRetryCount = handlerInfo.MaxRetryCount;
            // Updating message item in the queue
            if (result.Success)
            {
                message.StatusOid = (byte)QueueItemStatus.Processed;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (IIssue issue in result.Issues)
                {
                    if (issue.Severity.In(IssueSeverity.Error, IssueSeverity.ValidationError, IssueSeverity.PermissionError))
                        sb.AppendLine(issue.Message.ToString());
                    if (issue.Severity == IssueSeverity.Exception)
                    {
                        sb.AppendLine(issue.Exception.Message);
                        sb.AppendLine(issue.Exception?.InnerException?.Message);
                    }
                }
                message.LastErrorText = sb.ToString().TrimEnd();

                switch (handler.FailureType)
                {
                    // Handler forces us to fail item for good
                    case QueueHandlerFailureType.ForceFail:
                        message.StatusOid = (byte)QueueItemStatus.Failed;
                        handler.OnFailure();
                        break;
                    // Handler forces us to keep trying
                    case QueueHandlerFailureType.ForceRetry:
                        message.StatusOid = (byte)QueueItemStatus.Retry;
                        message.RetryCount++;
                        var retryPeriod = Math.Pow(message.RetryCount, 2 * handlerInfo.RetryCoefficient);
                        message.ProcessAfter = DateTime.Now.AddMinutes(Math.Min(handlerInfo.MaxRetryPeriod, retryPeriod));
                        break;
                    // Handler doesn't mind handling failure in default way as configured
                    case QueueHandlerFailureType.Default:
                        var isFatal = result.Issues.Any(i => i.Severity == IssueSeverity.Exception);
                        if (!isFatal && message.RetryCount < maxRetryCount)
                            goto case QueueHandlerFailureType.ForceRetry;
                        goto case QueueHandlerFailureType.ForceFail;
                }
            }
        }
    }
}
