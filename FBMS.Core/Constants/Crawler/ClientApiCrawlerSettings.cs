namespace FBMS.Core.Constants.Crawler
{
    public class ClientApiCrawlerSettings : IClientApiCrawlerSettings
    {
        public string Url { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string AuthUrl { get; set; }

        public string MatchScheduleBaseUrl { get; set; }

        public string MatchScheduleUrl { get; set; }

        public string MatchDetailBaseUrl { get; set; }

        public string TimeZone { get; set; }
    }
}
