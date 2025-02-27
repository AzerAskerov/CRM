using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CRM.Client.States
{
    /// <summary>
    /// Composite operation result.
    /// (contains multiple results, e.g. multiple validation errors).
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        public OperationResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public OperationResult(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the issues.
        /// </summary>
        /// <value>The issues.</value>
        public List<Issue> Issues { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="OperationResult" /> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get
            {
                return !Issues.Any(
                    c => c.Severity != IssueSeverity.Success
                        && c.Severity != IssueSeverity.Warning
                        && c.Severity != IssueSeverity.Information);
            }
        }


        /// <summary>
        /// Adds the results from the specified operation result. Use when operation calls other operations.
        /// </summary>
        /// <param name="innerResult">The result of the inner operation.</param>
        /// <returns>This operationResult object.</returns>
        public void Merge(OperationResult result)
        {
            if (result != null)
            {
                Issues.AddRange(result.Issues);
            }
        }


        /// <summary>
        /// Adds the results from the specified operation result. Use when operation calls other operations.
        /// </summary>
        /// <param name="innerResult">The result of the inner operation.</param>
        /// <returns>This operationResult object.</returns>
        public void Merge<TModel>(OperationResult<TModel> result)
        {
            if (result != null)
            {
                Issues.AddRange(result.Issues);
            }
        }

        /// <summary>
        /// Anies the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public bool Any(string code)
        {
            return Issues.Any(i => string.Equals(i.Code, code));
        }
    }

    public class OperationResult<TModel> : OperationResult
    {
        public OperationResult()
        {
        }

        public OperationResult(TModel model)
        {
            Model = model;
        }

        public TModel Model { get; set; }
    }
}
