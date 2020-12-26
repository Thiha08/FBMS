using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;

namespace FBMS.Core.Extensions
{
    public static class WebRequestExtensions
    {
        public static bool TryAddCookie(this WebRequest webRequest, Cookie cookie)
        {
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                return false;
            }

            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }

            httpRequest.CookieContainer.Add(cookie);
            return true;
        }

        public static bool TryAddFormData<TEntity>(this WebRequest webRequest, TEntity entity) where TEntity : class
        {
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                return false;
            }

            var formDataList = new List<string>();
            var propertyExpressions = ReflectionHelper.GetDescriptionAttributes<TEntity>();

            foreach (var expression in propertyExpressions)
            {
                var propertyValue = entity.GetType().GetProperty(expression.Key).GetValue(entity, null);
                formDataList.Add($"{expression.Value}={(propertyValue != null ? HttpUtility.UrlEncode(Convert.ToString(propertyValue)) : string.Empty)}");
            }
            var formData = string.Join("&",formDataList);
            var encodedFormData = Encoding.ASCII.GetBytes(formData);

            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.ContentLength = encodedFormData.Length;

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(encodedFormData, 0, encodedFormData.Length);
            }
            return true;
        }
    }
}
