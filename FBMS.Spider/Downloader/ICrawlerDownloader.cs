using FBMS.Core.Dtos.Crawler;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace FBMS.Spider.Downloader
{
    public interface ICrawlerDownloader
    {
        Task<HtmlDocument> DownloadAsync(CrawlerRequest request);
    }
}
