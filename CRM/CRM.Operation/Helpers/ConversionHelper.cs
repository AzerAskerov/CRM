using System.Collections.Generic;
using System.Text.RegularExpressions;
using Zircon.Core.Enums;

namespace CRM.Operation.Helpers
{
    public static class ConversionHelper
    {

        private static Regex PhoneNumberRegex = new Regex(
            @"(994|0|)" +                               // Calling prefix. 994 - international prefix, 0 - local prefix for calls inside the country. Sometimes local prefix is omitted
            @"((?<op>(40|50|51|55|60|70|77|99|10)\d{7})|" +    // Mobile operator. 40-Fonex, 50,51-Azercell, 70,77-Azerfon, 55-Bakcell, 60-Nakhtel CDMA, 70-Nar Mobile 
                                                               //   , followed by 7 digits
            @"(?<st>\d{9}))\b");                        // If mobile operator doesn't match, then it probably is a stationary phone. Region and town codes have different lengths, 
                                                        //   so it's hard to put them into regex. 


        public static ParseResult ParsePhoneNumber(string phone)
        {
            phone = phone?.Trim();
            phone = phone.Replace("+", "")
                .Replace("-", "")
                .Replace("(","")
                .Replace(")","")
                .Replace(" ", string.Empty);

            //some known cases

            //case 1
            if (phone.Length>6 && phone.Substring(0, 6) == "944994")
                phone = phone.Replace("944994", "994");

            //case 2
            if (phone.Length > 3 && phone.Substring(0, 3) == "944")
                phone = phone.Replace("944", "994");


            ParseResult formattedPhone = new ParseResult();

            formattedPhone.IsMatched = false;
            ContactInfoType contactInfoType;
            var result = string.Empty;

            // ParseResult<ContactInfo> formattedPhone = new ParseResult<ContactInfo>();
            // formattedPhone.Result = new ContactInfo();
            // formattedPhone.Result.Info = phone;
            // formattedPhone.IsMatched = false;
            Match m = PhoneNumberRegex.Match(phone);
            if (m.Success)
            {
                if (m.Groups["op"].Success)
                {
                    formattedPhone.IsMatched = true;
                    contactInfoType = ContactInfoType.MobilePhone;
                }
                else if (m.Groups["st"].Success)
                {
                    formattedPhone.IsMatched = true;
                    contactInfoType = ContactInfoType.MainPhone;
                }
                if (formattedPhone.IsMatched)
                {
                    formattedPhone.Result = PhoneNumberRegex.Replace(phone, "994${op}${st}");
                }
            }
            return formattedPhone;
        }

        public struct ParseResult
        {
            /// <summary>
            /// Parsed object
            /// </summary>
           // public T Result;

            public Dictionary<string, string> Parts;

            /// <summary>
            /// Input data is succesfully parsed
            /// </summary>
            public bool IsMatched;

            /// <summary>
            /// Part of the input that wasn't parsed
            /// </summary>
            public string UnparsedPart;

            public string Result;
        }
    }
}