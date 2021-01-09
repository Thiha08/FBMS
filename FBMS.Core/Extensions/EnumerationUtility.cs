using System;
using System.Collections.Generic;
using System.Linq;

namespace FBMS.Core.Extensions
{
    public static class EnumerationUtility
    {
        public static IEnumerable<T> GetValues<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }

            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static IEnumerable<T> GetEnumerableByDepth<T>(this IEnumerable<T> enumerable, int depth)
        {
            int index = 0;
            foreach (var item in enumerable)
            {
                if (index == depth)
                {
                    return (IEnumerable<T>)item;
                }
                index++;
            }
            return Enumerable.Empty<T>();
        }
    }
}
