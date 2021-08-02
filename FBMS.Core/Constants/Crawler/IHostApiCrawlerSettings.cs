using System.Threading.Tasks;

namespace FBMS.Core.Constants.Crawler
{
    public interface IHostApiCrawlerSettings
    {
        string Url { get; set; }

        string UserName { get; }

        string Password { get; }

        string AuthUrl { get; set; }

        string AllClientListUrl { get; set; }

        string ClientListUrl { get; set; }

        string ClientTransactionsUrl { get; set; }

        string TimeZone { get; set; }

        int TransitionHour { get; set; }

        Task InitializeAsync();
    }
}
