
using System;

namespace CRM.Operation.Localization
{
    public static class LocalizationExtension
    {
        public static string Translate(this string resourceId)
        {
            return ResourceManager.GetText(resourceId, resourceId);
        }

        public static string Translate(this string resourceId, string defaultText)
        {
            return ResourceManager.GetText(resourceId, defaultText);
        }
    }
}
