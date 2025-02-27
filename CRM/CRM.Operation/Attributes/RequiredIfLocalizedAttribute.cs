using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using CRM.Operation.Localization;


namespace CRM.Operation.Attributes
{
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "Class has children")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class RequiredIfLocalizedAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets the value, whether field value is required. Useful, when you need to make property optional in submodel.
        /// </summary>
        public bool IsRequired { get; set; }

        protected string DisplayName { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "RequiredIfLocalizedAttribute" /> class.
        /// </summary>
        /// <param name = "otherProperty">The property depending upon which this property is checked.</param>
        public RequiredIfLocalizedAttribute(string otherProperty)
        {
            IsRequired = true;

            // default error message depends on other parameters (empty/non empty/equal).
            ErrorMessage = "The field '{0}' is required.";
            OtherProperty = otherProperty;
        }

        /// <summary>
        ///   Gets other property name
        /// </summary>
        public string OtherProperty { get; private set; }

        /// <summary>
        ///   Gets or sets other property value to be equal to
        /// </summary>
        public object EqualTo { get; set; }

        /// <summary>
        ///   Gets or sets other property value not to be equal to
        /// </summary>
        public object NotEqualTo { get; set; }

        /// <summary>
        /// Gets or sets the other object's empty status. When set to true, property is required when other is empty. When set to false, property is required when other is non-empty.
        /// </summary>
        public object Empty { get; set; }

        /// <summary>
        /// Gets or sets the prefix for field OtherProperty. If value is not set, then attribute will fetch prefix from Controller.ViewData.TemplateInfo.HtmlFieldPrefix
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets the 'equal to' parameter value (resolves from configuration if necessary).
        /// </summary>
        public virtual object EqualToValue
        {
            get { return EqualTo; }
        }

        /// <summary>
        /// Gets the 'not equal to' parameter value (resolves from configuration if necessary).
        /// </summary>
        public virtual object NotEqualToValue
        {
            get { return NotEqualTo; }
        }

        /// <summary>
        /// Gets the error message key.
        /// </summary>
        protected string ErrorMessageKey
        {
            get
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    return ErrorMessage;
                }

                if (EqualTo != null)
                {
                    return "BlazorValidationMessage.RequiredIfEqual";
                }

                if (NotEqualTo != null)
                {
                    return "BlazorValidationMessage.RequiredIfNotEqual";
                }

                if (Empty is bool)
                {
                    if ((bool) Empty)
                    {
                        return "BlazorValidationMessage.RequiredIfEmpty";
                    }

                    return "BlazorValidationMessage.RequiredIfNotEmpty";
                }

                return "BlazorValidationMessage.RequiredIf";
            }
        }

        /// <summary>
        ///   Applies formatting to an error message based on the data field where the compare error occurred.
        /// </summary>
        /// <param name = "name">The name of the field that caused the validation failure.</param>
        /// <returns>
        ///   The formatted error message.
        /// </returns>
        /// <remarks>OtherProperty(-ies) will not be translated</remarks>
        public override string FormatErrorMessage(string name)
        {
            return FormatErrorMessage(name, null, null);
        }

        /// <summary>
        /// Applies formatting to an error message based on the data field where the compare error occurred.
        /// </summary>
        /// <param name="memberName">Name of the member. Missing if this is called from MVC</param>
        /// <param name="displayName">The display name. May be alredy localized when this is called from MVC</param>
        /// <param name="container">The type of object being validated. If value is Null, then dependency properties will not be translated</param>
        /// <returns>
        /// The formatted error message.
        /// </returns>
        /// <remarks>
        ///  When called from MVC we get display name already localized. But we don't always get memberName!
        ///  When called by DataAnnotations, we get memberName equal to displayName and they are not localized so we need to do it ourselves 
        ///  Error message is created by string.Format function. Next parameters are provided to the function: 
        ///  {0} - Field display name
        ///  {1} - Comma separated string of display names of "Other Properties"
        ///  {2} - "EqualToValue" - value that Other Property(-ies) must have 
        ///  {3} - "NotEqualToValue" - value that Other Property(-ies) must avoid
        /// </remarks>
        public string FormatErrorMessage(string memberName, Type container, string displayName)
        {

            return string.Format(
                ("BlazorValidationMessages.PropertyValueRequired").Translate(),
                DisplayName == null
                    ? memberName
                    : DisplayName.Translate());
        }

        /// <summary>
        ///   Performs validation.
        /// </summary>
        /// <param name = "value">The object to validate.</param>
        /// <param name = "validationContext">An object that contains information about the validation request.</param>
        /// <returns>
        ///   Validation result.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DisplayName = validationContext.GetMemberDisplayName();

            if (validationContext == null)
            {
                return new ValidationResult("validationContext cannot be null!");
            }

            if (!IsRequired)
            {
                return ValidationResult.Success;
            }

            foreach (string propertyName in
                OtherProperty.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                PropertyInfo propertyInfo = validationContext.ObjectType.GetProperty(propertyName.Trim());
                if (propertyInfo == null)
                {
                    return new ValidationResult("OtherProperty contain an invlaid property name");
                }

                bool condition = false;

                // convert Enums to ints, so that we can specifiy either in parameters (have to specify int for drop down values, Enum for displayed values)

                object otherValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
                Enum otherValueEnum = otherValue as Enum;
                if (otherValueEnum != null)
                {
                    otherValue = GetEnumValue(otherValueEnum);
                }

                object compareTo = this.EqualToValue ?? this.NotEqualToValue;
                Enum compareToEnum = compareTo as Enum;
                if (compareToEnum != null)
                {
                    compareTo = GetEnumValue(compareToEnum);
                }

                if (EqualToValue != null)
                {
                    condition = compareTo.Equals(otherValue);
                }
                else if (NotEqualToValue != null)
                {
                    condition = !compareTo.Equals(otherValue);
                }
                else if (Empty is bool)
                {
                    // either Empty=true & other=null or vice versa
                    condition = (bool) Empty == (otherValue == null || otherValue.Equals(string.Empty));
                }

                if (condition && IsEmpty(value))
                {
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.MemberName, validationContext.ObjectType,
                            validationContext.DisplayName),
                        new[] {validationContext.MemberName});
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Determines whether the specified value is empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is null or empty string.
        /// </returns>
        private bool IsEmpty(object value)
        {
            if (value == null)
            {
                return true;
            }

            string stringValue = value as string;
            return stringValue != null && stringValue.Length == 0;
        }

        /// <summary>
        /// Convert a value to an alternative javascript string (Enums to ints, all other to strings)
        /// </summary>
        /// <param name="value">The original value.</param>
        /// <returns>Javascript string</returns>
        /// <remarks>
        /// This is needed because we don't know whether int or string value of enum is used on the client.
        /// we use ints in combos and enum strings in radio buttons
        /// </remarks>
        private string ToJavaScriptAltValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            Enum enumValue = value as Enum;

            return enumValue != null
                ? GetEnumValue(enumValue).ToString(CultureInfo.InvariantCulture)
                : value.ToString();
        }

        private static int GetEnumValue(Enum enumeration)
        {
            return (int) Enum.Parse(enumeration.GetType(), enumeration.ToString());
        }
    }
}