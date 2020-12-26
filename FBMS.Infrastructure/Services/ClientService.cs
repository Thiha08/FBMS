using FBMS.Core.Constants.Crawler;
using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Entities;
using FBMS.Core.Interfaces;
using FBMS.SharedKernel.Interfaces;
using FBMS.Spider.Downloader;
using FBMS.Spider.Pipeline;
using FBMS.Spider.Processor;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.Services
{
    public class ClientService : IClientService
    {
        private readonly ICrawlerDownloader _downloader;
        private readonly ICrawlerProcessor _processor;
        private readonly ICrawlerPipeline _pipeline;
        private readonly IHostApiCrawlerSettings _hostApiCrawlerSettings;
        private readonly IRepository _repository;

        public ClientService(ICrawlerDownloader downloader, ICrawlerProcessor processor, ICrawlerPipeline pipeline, IHostApiCrawlerSettings hostApiCrawlerSettings, IRepository repository)
        {
            _downloader = downloader;
            _processor = processor;
            _pipeline = pipeline;
            _hostApiCrawlerSettings = hostApiCrawlerSettings;
            _repository = repository;
        }

        public async Task CrawlAsync()
        {
            var request = new CrawlerRequest
            {
                BaseUrl = _hostApiCrawlerSettings.ClientListUrl,
                Cookies = new List<Cookie>()
            };
            var document = await _downloader.DownloadAsync(request);
            var clients = _processor.Process<Client>(document);
            var existingClients = await _repository.ListAsync<Client>();
            var existingClientNames = existingClients.Select(x => x.Account).ToList();
            clients = clients.Where(x => !string.IsNullOrWhiteSpace(x.Account) && !existingClientNames.Contains(x.Account));
            await _pipeline.RunAsync(clients);
        }

        public async Task<List<Client>> ListAsync()
        {
            return await _repository.ListAsync<Client>();
        }

        public async Task DeleteAllAsync()
        {
            var items = await _repository.ListAsync<Client>();

            foreach (var item in items)
            {
                await _repository.DeleteAsync(item);
            }
        }
    }
}
