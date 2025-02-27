
using System;
using System.ComponentModel;
using CRM.Operation.Localization;

namespace CRM.Operation.Attributes
{
    /// <summary>
    /// Same as [DisplayName], but provides error message translations from DB.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public class DisplayNameLocalizedAttribute : DisplayNameAttribute
    {
        /// <summary>
        /// Same as [DisplayName], but provides error message translations from DB.
        /// </summary>
        /// <param name="displayNameKey">Translation key</param>
        public DisplayNameLocalizedAttribute(string displayNameKey)
            : base(displayNameKey)
        {
        }

        /// <summary>
        /// Normally it would be display text, but in this case it is translation key.
        /// </summary>
        public override string DisplayName
        {
            
            get
            {
                return base.DisplayNameValue.Translate();
            }
        }
    }
}