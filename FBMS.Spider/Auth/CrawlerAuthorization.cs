using FBMS.Core.Dtos.Auth;
using FBMS.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FBMS.Spider.Auth
{
    public class CrawlerAuthorization : ICrawlerAuthorization
    {
        public async Task<AuthResponse> IsSignedInAsync(string baseUrl)
        {
            using (var webClient = new WebClient())
            {
                var uri = new Uri(baseUrl);
                var webRequest = (HttpWebRequest)WebRequest.Create(uri);
                webRequest.CookieContainer = new CookieContainer();

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
                    var receiveStream = response.GetResponseStream();
                    var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    authResponse.HtmlCode = await readStream.ReadToEndAsync();
                }
                return authResponse;
            }
        }
    }
}
