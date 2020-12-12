using FBMS.Core.Constants.Crawler;
using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net;
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
                    HtmlWeb web = new HtmlWeb();
                    return await web.LoadFromWebAsync(crawlUrl);
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
    }
}
