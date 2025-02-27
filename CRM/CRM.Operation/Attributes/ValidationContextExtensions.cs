using System.Linq;
using System.ComponentModel.DataAnnotations;


namespace CRM.Operation.Attributes
{
    public static class ValidationContextExtensions
    {
        public static string GetMemberDisplayName(this ValidationContext validationContext)
        {
            if (validationContext.MemberName != null)
            {
                var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                if (property != null)
                {
                    var attribute = property
                        .GetCustomAttributes(typeof(DisplayNameLocalizedAttribute), true)
                        .FirstOrDefault();

                    if (attribute != null)
                    {
                        return ((DisplayNameLocalizedAttribute)attribute).DisplayName;
                    }
                }
            }

            return null;
        }
    }
}