using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CRM.Operation.Extensions
{
    public static class LambdaExtensions
    {
        public static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda, TValue value)
        {
            if (memberLamda.Body is MemberExpression memberSelectorExpression)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }
    }
}