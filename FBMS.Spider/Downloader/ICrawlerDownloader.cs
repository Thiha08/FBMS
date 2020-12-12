using FBMS.Core.Constants.Crawler;
using FBMS.Core.Dtos.Crawler;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace FBMS.Spider.Downloader
{
    public interface ICrawlerDownloader
    {
        Task<HtmlDocument> DownloadAsync(string crawlUrl, CrawlerDownloaderType downloderType, string downloadPath);
    }
}
