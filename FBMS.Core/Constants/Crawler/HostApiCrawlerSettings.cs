namespace FBMS.Core.Constants.Crawler
{
    public class HostApiCrawlerSettings : IHostApiCrawlerSettings
    {
        public string Url { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string AuthUrl { get; set; }

        public string AllClientListUrl { get; set; }

        public string ClientListUrl { get; set; }

        public string ClientTransactionsUrl { get; set; }

        public string TimeZone { get; set; }

        public int TransitionHour { get; set; }
    }
}
