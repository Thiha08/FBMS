using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Extensions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FBMS.Spider.Downloader
{
    public class CrawlerDownloader : ICrawlerDownloader
    {
        public async Task<HtmlDocument> DownloadAsync(CrawlerRequest request)
        {
            var htmlDocument = new HtmlDocument();

            using (var webClient = new WebClient())
            {
                var uri = new Uri(request.BaseUrl);
                var webRequest = WebRequest.Create(uri);
                foreach (var cookie in request.Cookies)
                {
                    webRequest.TryAddCookie(cookie);
                }
                var response = webRequest.GetResponse();
                var receiveStream = response.GetResponseStream();
                var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var htmlCode = await readStream.ReadToEndAsync();
                htmlDocument.LoadHtml(htmlCode);
                return htmlDocument;
            }

            throw new InvalidOperationException("Can not load html file from given source.");
        }
    }
}
