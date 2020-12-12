using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants.Crawler
{
    public class CrawlerConfigs
    {
        public string EscoApi { get; set; }

        public string CrawlerApi { get; set; }

        public string WebSites { get; set; }

        public float StartLookupDestinationPortAfterHours { get; set; }

        public float AllowCrawledDateBeforeKeyedInTimeInHours { get; set; }

        public float CrawlPerWebPeriodInMinutes { get; set; }

        public string EscoTerminal { get; set; }

        public bool IsTriggerCrawler { get; set; }

        public string TriggerJobsCrawlerUrl { get; set; }

        public int TriggerPerMinutes { get; set; } = 2;
    }
}
