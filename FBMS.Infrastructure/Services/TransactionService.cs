using FBMS.Core.Constants.Crawler;
using FBMS.Core.Dtos.Auth;
using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Entities;
using FBMS.Core.Extensions;
using FBMS.Core.Interfaces;
using FBMS.Core.Specifications;
using FBMS.SharedKernel.Interfaces;
using FBMS.Spider.Auth;
using FBMS.Spider.Downloader;
using FBMS.Spider.Pipeline;
using FBMS.Spider.Processor;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ICrawlerDownloader _downloader;
        private readonly ICrawlerProcessor _processor;
        private readonly ICrawlerPipeline _pipeline;
        private readonly ICrawlerAuthorization _crawlerAuthorization;
        private readonly IHostApiCrawlerSettings _hostApiCrawlerSettings;
        private readonly IRepository _repository;

        public TransactionService(ICrawlerDownloader downloader, ICrawlerProcessor processor, ICrawlerPipeline pipeline, ICrawlerAuthorization crawlerAuthorization, IHostApiCrawlerSettings hostApiCrawlerSettings, IRepository repository)
        {
            _downloader = downloader;
            _processor = processor;
            _pipeline = pipeline;
            _crawlerAuthorization = crawlerAuthorization;
            _hostApiCrawlerSettings = hostApiCrawlerSettings;
            _repository = repository;
        }

        public async Task CrawlAsync(int clientId)
        {
            var startDate = DateTime.UtcNow.ToString("MM/dd/yyyy");
            var endDate = DateTime.UtcNow.ToString("MM/dd/yyyy");
            var client = await _repository.GetByIdAsync<Client>(clientId);

            var request = new CrawlerRequest
            {
                BaseUrl = _hostApiCrawlerSettings.ClientTransactionsUrl
                    .Replace("{{CLIENT_NAME}}", client.Account.Replace("*", ""))
                    .Replace("{{START_DATE}}", startDate)
                    .Replace("{{END_DATE}}", endDate),
                Cookies = new List<Cookie>()
            };
            var document = await _downloader.DownloadAsync(request);
            var transactions = _processor.Process<Transaction>(document);
            transactions = transactions.Where(x => !string.IsNullOrWhiteSpace(x.Account));
            foreach (var item in transactions)
            {
                item.Client = client;
            }
            await _pipeline.RunAsync(transactions);
        }

        public async Task CrawlAsync()
        {
            var startDate = DateTime.UtcNow.ToString("MM/dd/yyyy");
            var endDate = DateTime.UtcNow.ToString("MM/dd/yyyy");
            var clients = await _repository.ListAsync<Client>();

            var authResponse = await _crawlerAuthorization.IsSignedInAsync(_hostApiCrawlerSettings.Url);

            if (!authResponse.isSignedIn)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(authResponse.HtmlCode);
                var formData = (_processor.Process<SignInCrawlingDto>(htmlDocument)).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(formData.AuthUrl))
                {
                    var authRequest = new AuthRequest
                    {
                        AuthUrl = _hostApiCrawlerSettings.AuthUrl + formData.AuthUrl.Replace("./", ""),
                        Cookies = authResponse.Cookies
                    };

                    authRequest.RequestForm = new SignInDto
                    {
                        EventTarget = "btnSignIn",
                        EventArgument = "",
                        EventValidation = formData.EventValidation,
                        ViewState = formData.ViewState,
                        ViewStateGenerator = formData.ViewStateGenerator,
                        TxtUserName = _hostApiCrawlerSettings.UserName,
                        TxtPassword = _hostApiCrawlerSettings.Password
                    };

                    authResponse = await _crawlerAuthorization.SignInAsync(authRequest);
                    if (!authResponse.isSignedIn)
                    {
                        throw new AuthenticationException(authResponse.HtmlCode);
                    }
                }
            }

            foreach (var client in clients)
            {
                var request = new CrawlerRequest
                {
                    BaseUrl = _hostApiCrawlerSettings.ClientTransactionsUrl
                    .Replace("{{CLIENT_NAME}}", client.Account.Replace("*", ""))
                    .Replace("{{START_DATE}}", startDate)
                    .Replace("{{END_DATE}}", endDate),
                    Cookies = authResponse.Cookies
                };
                var document = await _downloader.DownloadAsync(request);
                var transactions = _processor.Process<Transaction>(document);
                transactions = transactions.Where(x => !string.IsNullOrWhiteSpace(x.Account));
                foreach (var item in transactions)
                {
                    item.Client = client;
                }
                await _pipeline.RunAsync(transactions);
            }
        }

        public async Task<List<Transaction>> ListAsync(int clientId)
        {
            var transactionOfClientSpec = new TransactionSpecification(clientId);
            return await _repository.ListAsync(transactionOfClientSpec);
        }

        public async Task<List<Transaction>> ListAsync()
        {
            return await _repository.ListAsync<Transaction>();
        }

        public async Task DeleteAllAsync(int clientId)
        {
            var transactionOfClientSpec = new TransactionSpecification(clientId);
            var items = await _repository.ListAsync(transactionOfClientSpec);
            foreach (var item in items)
            {
                await _repository.DeleteAsync(item);
            }
        }

        public async Task DeleteAllAsync()
        {
            var items = await _repository.ListAsync<Transaction>();
            foreach (var item in items)
            {
                await _repository.DeleteAsync(item);
            }
        }
    }
}
