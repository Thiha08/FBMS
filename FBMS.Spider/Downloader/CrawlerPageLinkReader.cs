using FBMS.Core.Constants.Crawler;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FBMS.Spider.Downloader
{
    /// <summary>
    /// Get Urls
    /// https://codereview.stackexchange.com/questions/139783/web-crawler-that-uses-task-parallel-library 
    /// </summary>
    public class CrawlerPageLinkReader : ICrawlerPageLinkReader
    {
        private Regex _regex;

        public async Task<IEnumerable<string>> GetLinksAsync(string url, string regex, int level = 0)
        {
            if (!string.IsNullOrWhiteSpace(regex))
            {
                _regex = new Regex(regex);
            }

            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            var rootUrls = await GetPageLinksAsync(url, false);

            if (level == 0)
                return rootUrls;

            var links = await GetAllPagesLinksAsync(rootUrls);

            --level;
            var tasks = await Task.WhenAll(links.Select(link => GetLinksAsync(link, regex, level)));
            return tasks.SelectMany(l => l);
        }

        private async Task<IEnumerable<string>> GetPageLinksAsync(string url, bool needMatch = true)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDocument = await web.LoadFromWebAsync(url);

                var linkList = htmlDocument.DocumentNode
                                   .Descendants("a")
                                   .Select(a => a.GetAttributeValue("href", null))
                                   .Where(u => !string.IsNullOrEmpty(u))
                                   .Distinct();

                if (_regex != null)
                    linkList = linkList.Where(x => _regex.IsMatch(x));

                return linkList;
            }
            catch (Exception exception)
            {
                return Enumerable.Empty<string>();
            }
        }

        private async Task<IEnumerable<string>> GetAllPagesLinksAsync(IEnumerable<string> rootUrls)
        {
            var result = await Task.WhenAll(rootUrls.Select(url => GetPageLinksAsync(url)));

            return result.SelectMany(x => x).Distinct();
        }
    }
}
