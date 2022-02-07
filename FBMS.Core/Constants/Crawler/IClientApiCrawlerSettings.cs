using System.Threading.Tasks;

namespace FBMS.Core.Constants.Crawler
{
    public interface IClientApiCrawlerSettings
    {
        string Url { get; set; }

        string UserName { get; }

        string Password { get; }

        string AuthUrl { get; set; }

        string MatchScheduleBaseUrl { get; set; }

        string MatchScheduleUrl { get; set; }

        string MatchDetailBaseUrl { get; set; }

        string TimeZone { get; set; }

        int AcceptablePassedMinute { get; set; }

        int AcceptableDischargedCount { get; set; }

        bool IsTestingStack { get; set; } // if testing stack, stack = 1

        Task InitializeAsync();
    }
}
