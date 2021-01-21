using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Interfaces;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using FBMS.Spider.Downloader;
using FBMS.Spider.Pipeline;
using FBMS.Spider.Processor;
using FBMS.Spider.Scheduler;
using System.Threading.Tasks;

namespace FBMS.Infrastructure
{
    public class CrawlerService : ICrawlerService
    {
        public readonly ICrawlerDownloader _downloader;
        public readonly ICrawlerPageLinkReader _linkReader;
        public readonly ICrawlerProcessor _processor;
        public readonly ICrawlerScheduler _sheduler;
        public readonly ICrawlerPipeline _pipeline;

        public CrawlerService(ICrawlerDownloader downloader, ICrawlerPageLinkReader linkReader, ICrawlerProcessor processor, ICrawlerScheduler sheduler, ICrawlerPipeline pipeline)
        {
            _downloader = downloader;
            _linkReader = linkReader;
            _processor = processor;
            _sheduler = sheduler;
            _pipeline = pipeline;
        }

        public Task CrawlAsync<TEntity>(CrawlerRequest request) where TEntity : BaseEntity, IAggregateRoot
        {
            //var links = await _linkReader.GetLinksAsync(request.Url, request.Regex, 0);

            //var links = new List<string> { request.BaseUrl };

            //foreach (var url in links)
            //{
            //    var document = await _downloader.DownloadAsync(url, request.DownloderType, request.DownloadPath);
            //    var entity = _processor.Process<TEntity>(document);
            //    await _pipeline.RunAsync(entity);
            //}

            return Task.CompletedTask;
        }
    }
}
