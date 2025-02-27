using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Helpers
{
    public static class RemoteApiHelper
    {
        public static string GetValidParameter(string country)
        {
            if (!string.IsNullOrEmpty(country))
            {
                string json = @"{""country"": """ + country.Split(',')[0] + @"""}";

                return json;
            }

            return string.Empty;
        }
    }
}
