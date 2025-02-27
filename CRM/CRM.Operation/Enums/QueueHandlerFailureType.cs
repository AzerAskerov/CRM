namespace CRM.Operation.Enums
{
    public enum QueueHandlerFailureType
    {
        /// <summary>
        /// Handler failure will be handled as specified in handler's configuration
        /// </summary>
        Default = 0,

        /// <summary>
        /// Shedule one more try regardless of configuration
        /// </summary>
        ForceRetry = QueueItemStatus.Retry,

        /// <summary>
        /// Fail for good regardless of configuration
        /// </summary>
        ForceFail = QueueItemStatus.Failed,
    }
}
