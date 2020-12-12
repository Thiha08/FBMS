using FBMS.Core.Constants.Crawler;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Dtos.Crawler
{
    public class CrawlerRequestDto
    {
        public string Url { get; set; }

        public string Regex { get; set; }

        public long TimeOut { get; set; }

        public CrawlerDownloaderType DownloderType { get; set; }

        public string DownloadPath { get; set; }

    }
}
