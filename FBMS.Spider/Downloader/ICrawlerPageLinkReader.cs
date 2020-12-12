using FBMS.Core.Constants.Crawler;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FBMS.Spider.Downloader
{
    public interface ICrawlerPageLinkReader
    {
        Task<IEnumerable<string>> GetLinksAsync(string url, string regex, int level = 0);
    }
}
