
using System.ComponentModel.DataAnnotations;
using CRM.Operation.Localization;


namespace CRM.Operation.Attributes
{
    /// <summary>
    /// Same as [Required], but provides error message translations from DB.
    /// </summary>
    public class RequiredLocalizedAttribute : RequiredAttribute
    {
        /// <summary>
        /// Gets or sets the value, whether field value is required. Useful, when you need to make property optional in submodel.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <value>
        /// The name of the member.
        /// </value>
        protected string DisplayName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public RequiredLocalizedAttribute()
            : base()
        {
            IsRequired = true;
            ErrorMessage = "BlazorValidationMessages.PropertyValueRequired";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                ErrorMessage.Translate("The field '{0}' is required."),
                DisplayName == null
                    ? name 
                    : DisplayName.Translate());
        }

        /// <summary>
        /// Checks that the value of the required data field is not empty.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            return !IsRequired || base.IsValid(value);
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