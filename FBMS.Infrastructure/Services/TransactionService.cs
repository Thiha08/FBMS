using AutoMapper;
using FBMS.Core.Constants;
using FBMS.Core.Constants.Crawler;
using FBMS.Core.Constants.Email;
using FBMS.Core.Ctos;
using FBMS.Core.Ctos.Filters;
using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Auth;
using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Entities;
using FBMS.Core.Extensions;
using FBMS.Core.Interfaces;
using FBMS.Core.Mail;
using FBMS.Core.Specifications;
using FBMS.Core.Specifications.Filters;
using FBMS.SharedKernel.Interfaces;
using FBMS.Spider.Auth;
using FBMS.Spider.Downloader;
using FBMS.Spider.Pipeline;
using FBMS.Spider.Processor;
using HtmlAgilityPack;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
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
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSettings _emailSettings;

        public TransactionService(ICrawlerDownloader downloader, ICrawlerProcessor processor, ICrawlerPipeline pipeline, ICrawlerAuthorization crawlerAuthorization, IHostApiCrawlerSettings hostApiCrawlerSettings, IRepository repository, IMapper mapper, IEmailSender emailSender, IEmailTemplateProvider emailTemplateProvider, IEmailSettings emailSettings)
        {
            _downloader = downloader;
            _processor = processor;
            _pipeline = pipeline;
            _crawlerAuthorization = crawlerAuthorization;
            _hostApiCrawlerSettings = hostApiCrawlerSettings;
            _repository = repository;
            _mapper = mapper;
            _emailSender = emailSender;
            _emailTemplateProvider = emailTemplateProvider;
            _emailSettings = emailSettings;
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

        public async Task UpdateTransaction(TransactionDto transactionDto)
        {
            await _repository.UpdateAsync(_mapper.Map<Transaction>(transactionDto));
        }

        public async Task CompleteTransaction(int id, string pricing, string message)
        {
            var transaction = await _repository.GetByIdAsync<Transaction>(id);
            transaction.MarkComplete(pricing, message);
            await _repository.UpdateAsync(transaction);
        }

        public async Task DischargeTransaction(int id, string message)
        {
            var transaction = await _repository.GetByIdAsync<Transaction>(id);
            transaction.MarkDischarge(message);
            await _repository.UpdateAsync(transaction);
        }

        public async Task UpdateTransactions(List<TransactionDto> transactionDtos)
        {
            foreach (var transaction in _mapper.Map<List<Transaction>>(transactionDtos))
            {
                await _repository.UpdateAsync(transaction);
            }
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

        public async Task CrawlTransactions(TransactionFilterCto filterCto)
        {
            var timeZoneTime = DateTime.UtcNow.ToTimeZoneTime();
            var transitionTime = new DateTime(timeZoneTime.Year, timeZoneTime.Month, timeZoneTime.Day, _hostApiCrawlerSettings.TransitionHour, 0, 0);
            var transitionResult = DateTime.Compare(timeZoneTime, transitionTime);
            if (transitionResult < 0)
            {
                filterCto.StartDate = filterCto.StartDate.AddDays(-1);
                filterCto.EndDate = filterCto.EndDate.AddDays(-1);
            }

            var startDateUTC = filterCto.StartDate.ToTimeZoneTimeString("MM/dd/yyyy");
            var endDateUTC = filterCto.EndDate.ToTimeZoneTimeString("MM/dd/yyyy");

            var members = await _repository.ListAsync(new MemberWithTransactionTemplateSpecification(id: null, status: true));

            if (filterCto.MemberIds.Any())
            {
                members = members.Where(x => filterCto.MemberIds.Contains(x.Id)).ToList();
            }

            var authResponse = await GetHostApiAuthentication();

            var specification = new TransactionByDateSpecification();
            var existingTransactions = await _repository.ListAsync(specification);
            var existingTransactionNumbers = existingTransactions.AsParallel().Select(x => x.TransactionNumber).ToList();

            var transactions = new List<Transaction>();

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
                transactionCtos = transactionCtos.AsParallel().Where(x => !string.IsNullOrWhiteSpace(x.UserName) && !existingTransactionNumbers.Contains(x.TransactionNumber));

                foreach (var item in transactionCtos)
                {
                    string iString = item.TransactionDate.ReplaceFirst(" ", "/" + DateTime.UtcNow.Year.ToString() + " ");
                    var transactionDate = iString.ToUtcTime("dd/MM/yyyy h:mm:ss tt", _hostApiCrawlerSettings.TimeZone);

                    var transaction = new Transaction();
                    transaction.MemberId = member.Id;
                    transaction.SerialNumber = item.SerialNumber;
                    transaction.TransactionNumber = item.TransactionNumber;
                    transaction.UserName = item.UserName;
                    transaction.League = item.League;
                    transaction.HomeTeam = item.HomeTeam;
                    transaction.AwayTeam = item.AwayTeam;
                    transaction.Pricing = item.Pricing?.Replace("@", "");
                    transaction.TransactionDate = transactionDate; // UTC
                    transaction.TransactionType = GetTransactionType(item.TransactionType, item.HomeTeam, item.AwayTeam);
                    transaction.IsFirstHalf = !string.IsNullOrWhiteSpace(item.FirstHalf) && item.FirstHalf.Contains("(First Half)");
                    transaction.Amount = Convert.ToDecimal(item.Amount);
                    var convertedTransaction = member.TransactionTemplate.ApplyTransactionTemplate(transaction);
                    transactions.Add(convertedTransaction);
                }
            }
            transactions = transactions.AsParallel().Where(x => x.Status).ToList();
            await _pipeline.RunAsync(transactions);
        }

        private TransactionType GetTransactionType(string type, string homeTeam, string awayTeam)
        {
            var transactionType = TransactionType.Parlay;

            if (type?.TrimAndUpper() == homeTeam?.TrimAndUpper())
            {
                transactionType = TransactionType.Home;
            }
            else if (type?.TrimAndUpper() == awayTeam?.TrimAndUpper())
            {
                transactionType = TransactionType.Away;
            }
            else if (type?.TrimAndUpper() == TransactionType.Parlay.ToDescription().TrimAndUpper())
            {
                transactionType = TransactionType.Parlay;
            }
            else if (type?.TrimAndUpper() == TransactionType.Over.ToDescription().TrimAndUpper())
            {
                transactionType = TransactionType.Over;
            }
            else if (type?.TrimAndUpper() == TransactionType.Under.ToDescription().TrimAndUpper())
            {
                transactionType = TransactionType.Under;
            }

            return transactionType;
        }

        private async Task<AuthResponse> GetHostApiAuthentication()
        {
            var authResponse = await _crawlerAuthorization.IsSignedInAsync(_hostApiCrawlerSettings.Url, CacheKeys.IBetAuthCookies);

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

            return authResponse;
        }

        public async Task TestTransactionsCompletedEmail()
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTransactionCompletedEmailTemplate());
            List<string> recipients = _emailSettings.Recipients.Split(',').ToList<string>();

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
            recipients.ForEach(recipient => message.To.Add(MailboxAddress.Parse(recipient)));
            message.Subject = $"TR100001 was completed.";
            message.Body = new TextPart("html")
            {
                Text = emailTemplate.ToString()
            };
            await _emailSender.SendAsync(message);
        }
    }
}
