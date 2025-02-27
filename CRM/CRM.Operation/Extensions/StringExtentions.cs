using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CRM.Operation.Extensions
{
    public static class StringExtensions
    {
        private static Regex messageParameterRegex = new Regex(@"\{(?<prop>[^:}]*)(?::(?<format>[^}]*))?\}");

        /// <summary>
        /// This pseudo dictionary is used to represent properties and fields of the object, but it doesn't actually contains all of them,
        /// but instead looks for a property/field value, only when one is requested. This helps in situation, when we have a big object, 
        /// but need only one, or two properties, or fields.
        /// </summary>
        private class ClassMembersDictionary : IDictionary
        {
            private readonly object obj;
            private readonly Type objectType;

            public ClassMembersDictionary(object obj)
            {
                this.obj = obj;
                objectType = obj.GetType();
            }

            public object this[object key]
            {
                get
                {
                    PropertyInfo prop = objectType.GetProperty((string)key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (prop != null)
                        return prop.GetValue(obj, null);
                    FieldInfo field = objectType.GetField((string)key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (field != null)
                        return field.GetValue(obj);
                    throw new ArgumentOutOfRangeException(key.ToString(), $"Property or field {key} doesn't exists on an object, or is inaccessible.");
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public int Count { get { return 0; } }

            public bool IsFixedSize { get { return false; } }

            public bool IsReadOnly { get { return true; } }

            public bool IsSynchronized { get { return false; } }

            public ICollection Keys { get { throw new NotImplementedException(); } }

            public object SyncRoot { get { throw new NotImplementedException(); } }

            public ICollection Values { get { throw new NotImplementedException(); } }

            public void Add(object key, object value)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(object key)
            {
                MemberInfo[] member = objectType.GetMember((string)key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                return member.Length > 0 && member[0].GetCustomAttribute<XmlIgnoreAttribute>() == null;
            }

            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public IDictionaryEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public void Remove(object key)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        static string EscapeCurlyBracesInPlaceholders(string input)
        {
            var str = string.Empty;
            var openCurlyBrace = '{';
            var closeCurlyBrace = '}';
            var check = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == openCurlyBrace)
                {
                    if (int.TryParse(input[i + 1].ToString(), out var resOpen))
                    {
                        str += input[i];
                        continue;
                    }
                    else
                    {
                        str += input[i] + openCurlyBrace.ToString();
                        check = true;
                    }



                }
                else if (input[i] == closeCurlyBrace)
                {
                    if (int.TryParse(input[i - 1].ToString(), out var resClose))
                    {
                        str += input[i];
                        continue;
                    }
                    else
                    {
                        str += input[i] + closeCurlyBrace.ToString();
                        check = true;
                    }
                }

                if (!check)
                {
                    str += input[i];
                }

                check = false;
            }

            return str;
        }



        /// <summary>
        /// Replaces parameters in a template <paramref name="text"/> with values of the properties of object <paramref name="objectMap"/>
        /// </summary>
        /// <param name="text">Formatting template</param>
        /// <param name="objectMap">Object with template parameter values</param>
        /// <returns></returns>
        public static string ParametrizeExtended(this string text, object objectMap)
        {
            if (!string.IsNullOrEmpty(text) && objectMap != null)
            {
                //var escapedText = EscapeCurlyBraces(text);
                IDictionary values = null;
                if (objectMap is string)
                {
                    var escapedText = EscapeCurlyBracesInPlaceholders(text);
                    text = string.Format(escapedText, objectMap);
                }
                // If passed objectMap is a list, then assume 'text' to be String.Format template (for compatibility with old localizations)
                else if (objectMap is IList)
                {
                    text = string.Format(text, new ArrayList(objectMap as IList).ToArray());
                }
                // key;value pair dictionary for the "Field {FieldName} is invalid" style messages
                else if (objectMap is IDictionary)
                {
                    values = objectMap as IDictionary;
                }
               
                // For IDataRecord (usually SqlDataReader), create dictionary from all record fields
                else if (objectMap is IDataRecord)
                {
                    IDataRecord reader = (IDataRecord)objectMap;
                    values = Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue);
                }
                // otherwise treat it as property bag object
                else
                {
                    values = new ClassMembersDictionary(objectMap);
                }

                if (values != null)
                {
                    return messageParameterRegex.Replace(text, m =>
                    {
                        string prop = m.Groups["prop"].Value;
                        if (!values.Contains(prop))
                            return "{"+prop+"}";

                        if (m.Groups["format"].Success)
                            return string.Format("{0:" + m.Groups["format"].Value + "}", values[prop] ?? " ");
                        else
                            return string.Format("{0}", values[prop] ?? " ");
                    });
                }

                //replace remaining parameter names with empty string
                return text;
            }

            return text;
        }

        /// <summary>
        /// Replaces parameters in a template <paramref name="text"/> with values of the properties of object <paramref name="objectMap"/>
        /// </summary>
        /// <param name="text">Formatting template</param>
        /// <param name="objectMap">Object with template parameter values</param>
        /// <returns></returns>
        public static IDictionary ExtractParameters(this string text, object objectMap)
        {
            if (!string.IsNullOrEmpty(text) && objectMap != null)
            {
                IDictionary values = null;
                // key;value pair dictionary for the "Field {FieldName} is invalid" style messages
                if (objectMap is IDictionary)
                {
                    values = objectMap as IDictionary;
                }
               
                // otherwise treat it as property bag object
                else
                {
                    values = new ClassMembersDictionary(objectMap);
                }

                if (values != null)
                {
                    Dictionary<string, string> captures = new Dictionary<string, string>();
                    foreach (Match m in messageParameterRegex.Matches(text))
                    {
                        string key = m.Groups["prop"].Value;
                        if (m.Groups["format"].Success)
                            captures.Add(key, string.Format("{0:" + m.Groups["format"].Value + "}", values[key]));
                        else
                            captures.Add(key, string.Format("{0}", values[key]));
                    }
                    if (captures.Count == 0)
                        return null;
                    else
                        return captures;
                }
                return values;
            }

            return null;
        }

        public static string FirstCharToUpper(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string AzeriToLatin(this string msg)
        {
            // "ə", "ü", "ö","ı" ,"ç","ş", "ğ"
            char[] a = { 'ə', 'ü', 'ö', 'ı', 'ç', 'ş', 'ğ', 'Ə', 'Ü', 'Ö', 'I', 'Ç', 'Ş', 'Ğ', 'Ə' };
            char[] b = { 'e', 'u', 'o', 'i', 'c', 's', 'g', 'E', 'U', 'O', 'I', 'C', 'S', 'G', 'E' };

            for (int i = 0; i < a.Length; i++)
            {
                msg = msg.Replace(a[i], b[i]);
            }

            return msg;
        }
        public static bool IsNumber (this string s)
        {
            BigInteger n;
            return BigInteger.TryParse(s, out n);
        }
    }
}
