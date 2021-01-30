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
    }
}
