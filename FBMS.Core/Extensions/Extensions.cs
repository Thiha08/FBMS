using FBMS.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FBMS.Core.Extensions
{
    public static class Extensions
    {
        public static Dictionary<string, string> ToDictionary(this ICookie cookie)
        {
            return cookie
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.GetAttribute<DescriptionAttribute>().Description, prop => (string)prop.GetValue(cookie, null));
        }

        public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return (Attribute.GetCustomAttribute(property, typeof(T)) as T);
        }

        public static string ToDescription(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
