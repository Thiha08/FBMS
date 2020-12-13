using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants.Crawler
{
    public class IBetCrawlerConfig
    {
        public string IBetUrl { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string FullName { get; set; }

        public int TriggerPerMinutes { get; set; } = 2;

        public string ClientListUrl { get; set; }

        public string ClientTransactionsUrl { get; set; }
    }
}
