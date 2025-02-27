using Ababil.Components.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.States
{



    public static class OperationResultBoxBase
    {
        static OperationResult Result { get; set; }


        public static void SetResult(IMessageBoxService service, OperationResult result)
        {
            service.Clear();
            Result = result;
            Console.WriteLine("IMessageBoxService: " + service);
            ErrorMessage()?.ForEach(x => service.ShowError(x));
            WarningMessage()?.ForEach(x => service.ShowWarning(x));
            InformationMessage()?.ForEach(x => service.ShowInfo(x));
            SuccessMessage()?.ForEach(x => service.ShowSuccess(x));
        }

        /// <summary>
        /// Shows current success messages
        /// </summary>
        /// <returns>
        /// div with success messages (if any)
        /// </returns>
        private static List<string> InformationMessage()
        {
            return GenerateMessage(IssueSeverity.Information);
        }

        /// <summary>
        /// Shows current success messages
        /// </summary>
        /// <returns>
        /// div with success messages (if any)
        /// </returns>
        private static List<string> SuccessMessage()
        {
            return GenerateMessage(IssueSeverity.Success);
        }

        /// <summary>
        /// Shows current success messages
        /// </summary>
        /// <returns>
        /// div with success messages (if any)
        /// </returns>
        private static List<string> WarningMessage()
        {
            return GenerateMessage(IssueSeverity.Warning);
        }

        /// <summary>
        /// Shows current error messages
        /// </summary>
        /// <returns>
        /// div with success messages (if any)
        /// </returns>
        private static List<string> ErrorMessage()
        {
            return GenerateMessage(IssueSeverity.Error);
        }

        /// <summary>
        /// Generates the message Html (information, warnings, etc).
        /// </summary>
        /// <param name="helper">The Html helper.</param>
        /// <param name="severity">The issue severity to generate message for.</param>
        /// <param name="allowShowExceptionDetails">if set to <c>true</c> allow to show exception details in debug mode.</param>
        /// <returns>
        /// Html for screen level message.
        /// </returns>

        private static List<string> GenerateMessage(IssueSeverity severity)
        {
            var issues = Result?.Issues;
            //Console.WriteLine("GenerateMessageeee:", Result?.Issues?.Count);
            if (issues == null)
            {
                return null;
            }

            var filteredIssues = issues
                    .Where(i => i.Severity == severity
                        || (severity == IssueSeverity.Error
                            && (i.Severity == IssueSeverity.Exception || i.Severity == IssueSeverity.ValidationError)))
                    .ToList();

            if (!filteredIssues.Any())
            {
                return null;
            }

            var list = new List<string>();

            foreach (Issue i in filteredIssues)
            {
                Console.WriteLine("filteredIssues: " + i.Message.ToString());
                list.Add(i.Message.ToString());
            }

            return list;
        }
    }
}
