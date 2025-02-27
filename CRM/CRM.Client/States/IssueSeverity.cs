namespace CRM.Client.States
{
    /// <summary>
    ///     One of the operation result constants
    /// </summary>
    public enum IssueSeverity
    {
        /// <summary>
        ///     Severity of the result is not defined.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Operation successful
        /// </summary>
        Success = 1,

        /// <summary>
        ///     Informative message
        /// </summary>
        Information = 2,

        /// <summary>
        ///     Operation continue with warning
        /// </summary>
        Warning = 3,

        /// <summary>
        ///     Validation error
        /// </summary>
        ValidationError = 4,

        /// <summary>
        ///     Operation error
        /// </summary>
        Error = 5,

        /// <summary>
        ///     Unexpected exception
        /// </summary>
        Exception = 6,

        /// <summary>
        /// Insufficient rights
        /// </summary>
        PermissionError = 7
    }
}
