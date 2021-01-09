namespace FBMS.Core.Constants.Crawler
{
    public interface IClientApiCrawlerSettings
    {
        string Url { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        string AuthUrl { get; set; }

        string MatchScheduleBaseUrl { get; set; }

        string MatchScheduleUrl { get; set; }
    }
}
