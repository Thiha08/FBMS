using System.Collections.Generic;
using System.Net;

namespace FBMS.Core.Dtos.Crawler
{
    public class CrawlerRequest
    {
        public string BaseUrl { get; set; }

        public string Regex { get; set; }

        public long TimeOut { get; set; }

        public List<Cookie> Cookies { get; set; } = new List<Cookie>();
    }
}
