using FBMS.Core.Dtos.Auth;
using FBMS.Core.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FBMS.Spider.Auth
{
    public class CrawlerAuthorization : ICrawlerAuthorization
    {
        private readonly IMemoryCache memoryCache;

        public CrawlerAuthorization(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public async Task<AuthResponse> IsSignedInAsync(string baseUrl, string cacheKey)
        {
            using (var webClient = new WebClient())
            {
                var uri = new Uri(baseUrl);
                var webRequest = (HttpWebRequest)WebRequest.Create(uri);

                if (!memoryCache.TryGetValue(cacheKey, out List<Cookie> authCookies))
                {
                    webRequest.CookieContainer = new CookieContainer();
                }
                else
                {
                    foreach (var cookie in authCookies)
                    {
                        webRequest.TryAddCookie(cookie);
                    }
                }
                var authResponse = new AuthResponse();
                using (var response = (HttpWebResponse)webRequest.GetResponse())
                {
                    authResponse.BaseUrl = response.ResponseUri;
                    if (authResponse.BaseUrl.AbsoluteUri == baseUrl)
                    {
                        authResponse.isSignedIn = true;
                    }
                    authResponse.Cookies = new List<Cookie>();
                    foreach (Cookie cookie in response.Cookies)
                    {
                        authResponse.Cookies.Add(cookie);
                    }
                    var receiveStream = response.GetResponseStream();
                    var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    authResponse.HtmlCode = await readStream.ReadToEndAsync();
                }
                return authResponse;
            }
        }

        public async Task<AuthResponse> SignInAsync(AuthRequest request)
        {
            using (var webClient = new WebClient())
            {
                var uri = new Uri(request.AuthUrl);
                var webRequest = (HttpWebRequest)WebRequest.Create(uri);
                foreach (var cookie in request.Cookies)
                {
                    webRequest.TryAddCookie(cookie);
                }
                webRequest.TryAddFormData(request.RequestForm);

                var authResponse = new AuthResponse();

                using (var response = (HttpWebResponse)webRequest.GetResponse())
                {
                    authResponse.BaseUrl = response.ResponseUri;
                    authResponse.Cookies = new List<Cookie>();
                    foreach (Cookie cookie in response.Cookies)
                    {
                        if (cookie.Name == ".ASPXAUTH")
                        {
                            authResponse.isSignedIn = true;
                        }
                        authResponse.Cookies.Add(cookie);
                    }
                    if (authResponse.isSignedIn)
                    {
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            Priority = CacheItemPriority.NeverRemove
                        };
                        memoryCache.Set(response.ResponseUri.Host, authResponse.Cookies, cacheExpiryOptions);
                    }
                    var receiveStream = response.GetResponseStream();
                    var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    authResponse.HtmlCode = await readStream.ReadToEndAsync();
                }
                return authResponse;
            }
        }
    }
}
