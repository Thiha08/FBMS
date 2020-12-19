using FBMS.Core.Constants.Crawler;
using FBMS.Core.Dtos.Cookies;
using System.Collections.Generic;

namespace FBMS.Core.Dtos.Crawler
{
    public class CrawlerRequest
    {
        public string BaseUrl { get; set; }

        public string Regex { get; set; }

        public long TimeOut { get; set; }

        public CrawlerDownloaderType DownloderType { get; set; }

        public string DownloadPath { get; set; }

        public Dictionary<string, string> Cookies { get; set; }
    }
}
