using FBMS.Core.Constants.Crawler;
using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Extensions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FBMS.Spider.Downloader
{
    public class CrawlerDownloader : ICrawlerDownloader
    {
        private string _localFilePath;
        private IDictionary<string, string> _cookies;

        public async Task<HtmlDocument> DownloadAsync(CrawlerRequest request)
        {
            // if exist dont download again
            PrepareFilePath(request.BaseUrl, request.DownloadPath);
            _cookies = request.Cookies;

            var existing = GetExistingFile(_localFilePath);
            if (existing != null)
                return existing;

            return await DownloadInternalAsync(request.BaseUrl, request.DownloderType);
        }

        private async Task<HtmlDocument> DownloadInternalAsync(string crawlUrl, CrawlerDownloaderType downloderType)
        {
            switch (downloderType)
            {
                case CrawlerDownloaderType.FromFile:
                    using (WebClient client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(crawlUrl, _localFilePath);
                    }
                    return GetExistingFile(_localFilePath);

                case CrawlerDownloaderType.FromMemory:
                    var htmlDocument = new HtmlDocument();
                    using (WebClient client = new WebClient())
                    {
                        string htmlCode = await client.DownloadStringTaskAsync(crawlUrl);
                        htmlDocument.LoadHtml(htmlCode);
                    }
                    return htmlDocument;

                case CrawlerDownloaderType.FromWeb:
                    //HtmlWeb web = new HtmlWeb();
                    //return await web.LoadFromWebAsync(crawlUrl);
                    var htmlDocument2 = new HtmlDocument();
                    var htmlCode2 = DownloadStringWithCookies(crawlUrl, Encoding.UTF8);
                    htmlDocument2.LoadHtml(htmlCode2);
                    return htmlDocument2;
            }

            throw new InvalidOperationException("Can not load html file from given source.");
        }

        private void PrepareFilePath(string fileName, string downloadPath)
        {
            var parts = fileName.Split('/');
            parts = parts.Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
            var htmlpage = string.Empty;
            if (parts.Length > 0)
            {
                htmlpage = parts[parts.Length - 1];
            }

            if (!htmlpage.Contains(".html"))
            {
                htmlpage = htmlpage + ".html";
            }
            htmlpage = htmlpage.Replace("=", "").Replace("?", "");

            _localFilePath = $"{downloadPath}{htmlpage}";
        }

        private HtmlDocument GetExistingFile(string fullPath)
        {
            try
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.Load(fullPath);
                return htmlDocument;
            }
            catch (Exception exception)
            {
            }
            return null;
        }

        private string DownloadStringWithCookies(string url, Encoding encoding)
        {
            using (var webClient = new WebClient())
            {
                var uri = new Uri(url);
                var webRequest = WebRequest.Create(uri);
                foreach (var nameValue in _cookies)
                {
                    webRequest.TryAddCookie(new Cookie(nameValue.Key, nameValue.Value, "/", uri.Host));
                }
                var response = webRequest.GetResponse();
                var receiveStream = response.GetResponseStream();
                var readStream = new StreamReader(receiveStream, encoding);
                var htmlCode = readStream.ReadToEnd();
                return htmlCode;
            }
        }
    }
}
