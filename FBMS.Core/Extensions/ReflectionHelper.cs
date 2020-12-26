using FBMS.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace FBMS.Core.Extensions
{
    public class ReflectionHelper
    {
        public static string GetEntityExpression<T>()
        {
            var entityAttribute = (typeof(T)).GetCustomAttribute<CrawlerEntityAttribute>();
            if (entityAttribute == null || string.IsNullOrWhiteSpace(entityAttribute.XPath))
                throw new Exception("This entity should be xpath");

            return entityAttribute.XPath;
        }

        public static Dictionary<string, Tuple<SelectorType, string, string>> GetPropertyAttributes<T>()
        {
            var attributeDictionary = new Dictionary<string, Tuple<SelectorType, string, string>>();

            PropertyInfo[] props = typeof(T).GetProperties();
            var propList = props.Where(p => p.CustomAttributes.Count() > 0);

            foreach (PropertyInfo prop in propList)
            {
                var attr = prop.GetCustomAttribute<CrawlerFieldAttribute>();
                if (attr != null)
                {
                    attributeDictionary.Add(prop.Name, Tuple.Create(attr.SelectorType, attr.Expression, attr.HtmlAttribute));
                }
            }
            return attributeDictionary;
        }

        public static Dictionary<string, string> GetDescriptionAttributes<T>()
        {
            var attributeDictionary = new Dictionary<string, string>();

            PropertyInfo[] props = typeof(T).GetProperties();
            var propList = props.Where(p => p.CustomAttributes.Count() > 0);

            foreach (PropertyInfo prop in propList)
            {
                var attr = prop.GetCustomAttribute<DescriptionAttribute>();
                if (attr != null)
                {
                    attributeDictionary.Add(prop.Name, attr.Description);
                }
            }
            return attributeDictionary;
        }

        public static object CreateNewEntity<T>()
        {
            object instance = Activator.CreateInstance(typeof(T));
            return instance;
        }

        public static void TrySetProperty(object obj, string property, object value)
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
