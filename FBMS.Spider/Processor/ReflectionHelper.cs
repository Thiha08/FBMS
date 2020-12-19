using FBMS.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FBMS.Spider.Processor
{
    public class ReflectionHelper
    {
        internal static string GetEntityExpression<T>()
        {
            var entityAttribute = (typeof(T)).GetCustomAttribute<CrawlerEntityAttribute>();
            if (entityAttribute == null || string.IsNullOrWhiteSpace(entityAttribute.XPath))
                throw new Exception("This entity should be xpath");

            return entityAttribute.XPath;
        }

        public static Dictionary<string, Tuple<SelectorType, string>> GetPropertyAttributes<T>()
        {
            var attributeDictionary = new Dictionary<string, Tuple<SelectorType, string>>();

            PropertyInfo[] props = typeof(T).GetProperties();
            var propList = props.Where(p => p.CustomAttributes.Count() > 0);

            foreach (PropertyInfo prop in propList)
            {
                var attr = prop.GetCustomAttribute<CrawlerFieldAttribute>();
                if (attr != null)
                {
                    attributeDictionary.Add(prop.Name, Tuple.Create(attr.SelectorType, attr.Expression));
                }
            }
            return attributeDictionary;
        }

        internal static object CreateNewEntity<T>()
        {
            object instance = Activator.CreateInstance(typeof(T));
            return instance;
        }

        internal static void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                try
                {
                    prop.SetValue(obj, value, null);
                }
                catch (Exception e) { }
            }
        }
    }
}
