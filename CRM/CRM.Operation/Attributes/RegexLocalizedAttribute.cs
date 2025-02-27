using System.ComponentModel.DataAnnotations;
using CRM.Operation.Localization;

namespace CRM.Operation.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class RegexLocalizedAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        protected string DisplayName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexLocalizedAttribute"/> class.
        /// </summary>
        /// <param name="pattern">The regular expression that is used to validate the data field value.</param>
        /// <param name="errorMessage"></param>
        public RegexLocalizedAttribute(string pattern, string errorMessage)
            : base(pattern)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Formats the error message to display if the regular expression validation fails.
        /// </summary>
        /// <param name="name">The name of the field that caused the validation failure.</param>
        /// <returns>
        /// The formatted error message.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                ErrorMessage.Translate("The field '{0}' must match the regular expression {1}."),
                DisplayName == null
                    ? name
                    : DisplayName.Translate(),
                Pattern);
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DisplayName = validationContext.GetMemberDisplayName();

            return base.IsValid(value, validationContext);
        }
    }
}