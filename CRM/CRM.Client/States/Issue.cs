using System;

namespace CRM.Client.States
{
    /// <summary>
    /// Result of any operation. Consists of result type, message and parameters.
    /// </summary>
   
    public class Issue
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// The severity.
        /// </summary>
        public IssueSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public Message Message { get; set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
