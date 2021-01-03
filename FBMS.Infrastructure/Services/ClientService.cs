using Ardalis.GuardClauses;
using AutoMapper;
using FBMS.Core.Constants.Crawler;
using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Entities;
using FBMS.Core.Interfaces;
using FBMS.Core.Specifications;
using FBMS.Core.Specifications.Filters;
using FBMS.SharedKernel.Interfaces;
using FBMS.Spider.Downloader;
using FBMS.Spider.Pipeline;
using FBMS.Spider.Processor;
using System;
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
        private readonly IMapper _mapper;

        public ClientService(ICrawlerDownloader downloader, ICrawlerProcessor processor, ICrawlerPipeline pipeline, IHostApiCrawlerSettings hostApiCrawlerSettings, IRepository repository, IMapper mapper)
        {
            _downloader = downloader;
            _processor = processor;
            _pipeline = pipeline;
            _hostApiCrawlerSettings = hostApiCrawlerSettings;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ClientDto> GetClient(int clientId)
        {
            var client = await _repository.GetByIdAsync<Client>(clientId);

            Guard.Against.Null(client, nameof(client));

            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> GetClient(string accountName)
        {
            var client = await _repository.GetBySpecificationAsync<Client>(new ClientByAccountNameSpecification(accountName));

            Guard.Against.Null(client, nameof(client));

            return _mapper.Map<ClientDto>(client);
        }

        public async Task<List<ClientDto>> GetClients()
        {
            var clients = await _repository.ListAsync<Client>();

            return _mapper.Map<List<ClientDto>>(clients);
        }

        public async Task<List<ClientDto>> GetClients(ClientFilterDto filterDto)
        {
            var specification = new ClientSpecification(_mapper.Map<ClientFilter>(filterDto));
            var clients = await _repository.ListAsync(specification);

            return _mapper.Map<List<ClientDto>>(clients);
        }

        public async Task EnableClient(int clientId)
        {
            var client = await _repository.GetByIdAsync<Client>(clientId);

            Guard.Against.Null(client, nameof(client));

            client.Status = true;
            client.DateUpdated = DateTime.Now;

            await _repository.UpdateAsync(client);
        }

        public async Task DisableClient(int clientId)
        {
            var client = await _repository.GetByIdAsync<Client>(clientId);

            Guard.Against.Null(client, nameof(client));

            client.Status = false;
            client.DateUpdated = DateTime.Now;

            await _repository.UpdateAsync(client);
        }

        public async Task DeleteClient(int clientId)
        {
            var client = await _repository.GetByIdAsync<Client>(clientId);

            Guard.Against.Null(client, nameof(client));

            client.Status = false;
            await _repository.UpdateAsync(client);
        }

        public async Task DeleteClients(ClientFilterDto filterDto)
        {
            var specification = new ClientSpecification(_mapper.Map<ClientFilter>(filterDto));
            var clients = await _repository.ListAsync(specification);

            foreach (var client in clients)
            {
                client.Status = false;
                await _repository.UpdateAsync(client);
            }
        }

        public async Task CrawlClients()
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
    }
}
