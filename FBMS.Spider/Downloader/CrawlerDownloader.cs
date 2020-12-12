using FBMS.Core.Constants.Crawler;
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

        public async Task<HtmlDocument> DownloadAsync(string crawlUrl, CrawlerDownloaderType downloderType, string downloadPath)
        {
            // if exist dont download again
            PrepareFilePath(crawlUrl, downloadPath);

            var existing = GetExistingFile(_localFilePath);
            if (existing != null)
                return existing;

            return await DownloadInternalAsync(crawlUrl, downloderType);
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
                    var cookieNameValues = new Dictionary<string, string>();
                    cookieNameValues.Add("__cfduid", "d789ac8ae5a12e34f2fc00f52d50024db1606743671");
                    cookieNameValues.Add("ASP.NET_SessionId", "3nrdpknbbdukyd5uacib3jqv");
                    cookieNameValues.Add("BPX-STICKY-SESSION", "77");
                    cookieNameValues.Add("IGA_Agent_v8_jxy83", "EEEF49F1327FA52A36241FFEA2042AAADE7D8DC0373C27A9F81B5EFD7027557062847D03DD518F3FE9B237751BBB4F14B066063388EAED28DD1AD14375BCAC55B7AD2F091E6E0D875E273ED94AE50C205AD656598B03DE77D82E4D3170E20791764DC54655FE341A18D467A2FC7AD9D87D9E6B4B598CE986D4AFC0C485CD2E9CD551C5F65AFE43FF0BF02448E6F005EF");
                    cookieNameValues.Add(".ASPXAUTH", "888708B5C8AF500E104AEB02108832E0F33CAD5AB497D65393DA5ABF4EE51495A074B8546A04F8FF3CA4AB5DF1F826C75A5EBF3BC5D407A80735F480B06D16516B42ED259ABD12B25080B18A7127F2914BE641E7BE2430E9E5471C14C5B137320E1F2BE520EC2B7E8C586A13D32EF9450AFA0E85F5DFF4990ED974769F797C89");
                    var htmlCode2 = DownloadStringWithCookies(crawlUrl, Encoding.UTF8, cookieNameValues);
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

        private string DownloadStringWithCookies(string url, Encoding encoding, IDictionary<string, string> cookieNameValues)
        {
            using (var webClient = new WebClient())
            {
                var uri = new Uri(url);
                var webRequest = WebRequest.Create(uri);
                foreach (var nameValue in cookieNameValues)
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
