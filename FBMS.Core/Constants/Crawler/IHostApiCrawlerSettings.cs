using FBMS.Core.Dtos.Cookies;

namespace FBMS.Core.Constants.Crawler
{
    public interface IHostApiCrawlerSettings
    {
        string Url { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        int TriggerPerMinutes { get; set; }

        string AuthUrl { get; set; }

        string ClientListUrl { get; set; }

        string ClientTransactionsUrl { get; set; }

        IBetCookie IBetCookie { get; set; }
    }
}
