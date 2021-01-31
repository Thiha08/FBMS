using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Extensions
{
    public static class StringExtensions
    {
        public static string TrimAndUpper(this string value)
        {
            return value.Trim().ToUpper();
        }

        public static string ConcatSuffix(this string value, string suffix)
        {
            value = value + " " + suffix;
            return value.TrimAndUpper();
        }
    }
}
