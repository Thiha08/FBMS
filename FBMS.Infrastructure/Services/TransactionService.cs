using AutoMapper;
using FBMS.Core.Constants;
using FBMS.Core.Constants.Crawler;
using FBMS.Core.Ctos;
using FBMS.Core.Ctos.Filters;
using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Auth;
using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Entities;
using FBMS.Core.Extensions;
using FBMS.Core.Interfaces;
using FBMS.Core.Specifications;
using FBMS.Core.Specifications.Filters;
using FBMS.SharedKernel.Interfaces;
using FBMS.Spider.Auth;
using FBMS.Spider.Downloader;
using FBMS.Spider.Pipeline;
using FBMS.Spider.Processor;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IMapper _mapper;

        public TransactionService(ICrawlerDownloader downloader, ICrawlerProcessor processor, ICrawlerPipeline pipeline, ICrawlerAuthorization crawlerAuthorization, IHostApiCrawlerSettings hostApiCrawlerSettings, IRepository repository, IMapper mapper)
        {
            _downloader = downloader;
            _processor = processor;
            _pipeline = pipeline;
            _crawlerAuthorization = crawlerAuthorization;
            _hostApiCrawlerSettings = hostApiCrawlerSettings;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TransactionDto>> GetTransactions()
        {
            var transactions = await _repository.ListAsync<Transaction>();

            return _mapper.Map<List<TransactionDto>>(transactions);
        }

        public async Task<List<TransactionDto>> GetTransactions(TransactionFilterDto filterDto)
        {
            var specification = new TransactionSpecification(_mapper.Map<TransactionFilter>(filterDto));
            var transactions = await _repository.ListAsync(specification);

            return _mapper.Map<List<TransactionDto>>(transactions);
        }

        public async Task DeleteTransactions()
        {
            var transactions = await _repository.ListAsync<Transaction>();

            foreach (var transaction in transactions)
            {
                transaction.Status = false;
                await _repository.UpdateAsync(transaction);
            }
        }

        public async Task DeleteTransactions(TransactionFilterDto filterDto)
        {
            var specification = new TransactionSpecification(_mapper.Map<TransactionFilter>(filterDto));
            var transactions = await _repository.ListAsync(specification);

            foreach (var transaction in transactions)
            {
                transaction.Status = false;
                await _repository.UpdateAsync(transaction);
            }
        }

        public async Task CrawlTransactions()
        {
            var startDateUTC = DateTime.UtcNow.ToString("MM/dd/yyyy");
            var endDateUTC = DateTime.UtcNow.ToString("MM/dd/yyyy");
            var members = (await _repository.ListAsync<Member>())
                .Where(x => x.Status);

            var authResponse = await _crawlerAuthorization.IsSignedInAsync(_hostApiCrawlerSettings.Url);

            if (!authResponse.isSignedIn)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(authResponse.HtmlCode);
                var formData = (_processor.Process<SignInCto>(htmlDocument)).FirstOrDefault();

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

            var specification = new TransactionByDateSpecification();
            var existingTransactions = await _repository.ListAsync(specification);
            var existingTransactionNumbers = existingTransactions.Select(x => x.TransactionNumber).ToList();

            foreach (var member in members)
            {
                var request = new CrawlerRequest
                {
                    BaseUrl = _hostApiCrawlerSettings.ClientTransactionsUrl
                    .Replace("{{CLIENT_NAME}}", member.UserName)
                    .Replace("{{START_DATE}}", startDateUTC)
                    .Replace("{{END_DATE}}", endDateUTC),
                    Cookies = authResponse.Cookies
                };
                var document = await _downloader.DownloadAsync(request);
                var transactionCtos = _processor.Process<TransactionCto>(document);
                transactionCtos = transactionCtos.Where(x => !string.IsNullOrWhiteSpace(x.UserName) && !existingTransactionNumbers.Contains(x.TransactionNumber));

                var transactions = new List<Transaction>();
                foreach (var item in transactionCtos)
                {
                    var transaction = new Transaction();
                    transaction.MemberId = member.Id;
                    transaction.SerialNumber = item.SerialNumber;
                    transaction.TransactionNumber = item.TransactionNumber;
                    transaction.UserName = item.UserName;
                    transaction.League = item.League;
                    transaction.HomeTeam = item.HomeTeam;
                    transaction.AwayTeam = item.AwayTeam;
                    transaction.Pricing = item.Pricing;
                   
                    string iString = item.TransactionDate.ReplaceFirst(" ", "/" + DateTime.Now.Year.ToString() + " ");
                    transaction.TransactionDate = DateTime.ParseExact(iString, "dd/MM/yyyy h:mm:ss tt", null);

                    transaction.TransactionType = GetTransactionType(item.TransactionType, item.HomeTeam, item.AwayTeam);

                    transaction.Amount = Convert.ToDecimal(item.Amount);

                    transactions.Add(transaction);
                }
                await _pipeline.RunAsync(transactions);
            }
        }

        public async Task CrawlTransactions(TransactionFilterCto filterCto)
        {
            var startDateUTC = filterCto.StartDate.ToString("MM/dd/yyyy");
            var endDateUTC = filterCto.EndDate.ToString("MM/dd/yyyy");
            var members = await _repository.ListAsync(new MemberWithTransactionTemplateSpecification());

            if (filterCto.MemberIds.Any())
            {
                members = members.Where(x => filterCto.MemberIds.Contains(x.Id)).ToList();
            }

            var authResponse = await _crawlerAuthorization.IsSignedInAsync(_hostApiCrawlerSettings.Url);

            if (!authResponse.isSignedIn)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(authResponse.HtmlCode);
                var formData = (_processor.Process<SignInCto>(htmlDocument)).FirstOrDefault();

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

            var specification = new TransactionByDateSpecification();
            var existingTransactions = await _repository.ListAsync(specification);
            var existingTransactionNumbers = existingTransactions.Select(x => x.TransactionNumber).ToList();

            foreach (var member in members)
            {
                var request = new CrawlerRequest
                {
                    BaseUrl = _hostApiCrawlerSettings.ClientTransactionsUrl
                    .Replace("{{CLIENT_NAME}}", member.UserName)
                    .Replace("{{START_DATE}}", startDateUTC)
                    .Replace("{{END_DATE}}", endDateUTC),
                    Cookies = authResponse.Cookies
                };
                var document = await _downloader.DownloadAsync(request);
                var transactionCtos = _processor.Process<TransactionCto>(document);
                transactionCtos = transactionCtos.Where(x => !string.IsNullOrWhiteSpace(x.UserName) && !existingTransactionNumbers.Contains(x.TransactionNumber));

                var transactions = new List<Transaction>();
                foreach (var item in transactionCtos)
                {
                    var transaction = new Transaction();
                    transaction.MemberId = member.Id;
                    transaction.SerialNumber = item.SerialNumber;
                    transaction.TransactionNumber = item.TransactionNumber;
                    transaction.UserName = item.UserName;
                    transaction.League = item.League;
                    transaction.HomeTeam = item.HomeTeam;
                    transaction.AwayTeam = item.AwayTeam;
                    transaction.Pricing = item.Pricing;
                    string iString = item.TransactionDate.ReplaceFirst(" ", "/" + DateTime.Now.Year.ToString() + " ");
                    transaction.TransactionDate = DateTime.ParseExact(iString, "dd/MM/yyyy h:mm:ss tt", null);
                    transaction.TransactionType = GetTransactionType(item.TransactionType, item.HomeTeam, item.AwayTeam);
                    transaction.Amount = Convert.ToDecimal(item.Amount);
                    var convertedTransaction = member.TransactionTemplate.ApplyTransactionTemplate(transaction);
                    transactions.Add(convertedTransaction);
                }
                transactions = transactions.Where(x => x.Status).ToList();
                await _pipeline.RunAsync(transactions);
            }
        }

        private TransactionType GetTransactionType(string type, string homeTeam, string awayTeam)
        {
            var transactionType = TransactionType.Parlay;

            if (type == homeTeam)
            {
                transactionType = TransactionType.Home;
            }
            else if (type == awayTeam)
            {
                transactionType = TransactionType.Away;
            }
            else if (type == TransactionType.Parlay.ToDescription())
            {
                transactionType = TransactionType.Parlay;
            }
            else if (type == TransactionType.Over.ToDescription())
            {
                transactionType = TransactionType.Over;
            }
            else if (type == TransactionType.Under.ToDescription())
            {
                transactionType = TransactionType.Under;
            }

            return transactionType;
        }
    }
}
