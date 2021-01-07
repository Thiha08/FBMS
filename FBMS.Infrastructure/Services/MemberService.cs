using Ardalis.GuardClauses;
using AutoMapper;
using FBMS.Core.Constants.Crawler;
using FBMS.Core.Ctos;
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
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.Services
{
    public class MemberService : IMemberService
    {
        private readonly ICrawlerDownloader _downloader;
        private readonly ICrawlerProcessor _processor;
        private readonly ICrawlerPipeline _pipeline;
        private readonly ICrawlerAuthorization _crawlerAuthorization;
        private readonly IHostApiCrawlerSettings _hostApiCrawlerSettings;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public MemberService(ICrawlerDownloader downloader, ICrawlerProcessor processor, ICrawlerPipeline pipeline, ICrawlerAuthorization crawlerAuthorization, IHostApiCrawlerSettings hostApiCrawlerSettings, IRepository repository, IMapper mapper)
        {
            _downloader = downloader;
            _processor = processor;
            _pipeline = pipeline;
            _crawlerAuthorization = crawlerAuthorization;
            _hostApiCrawlerSettings = hostApiCrawlerSettings;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMember(int memberId)
        {
            var member = await _repository.GetByIdAsync<Member>(memberId);

            Guard.Against.Null(member, nameof(member));

            return _mapper.Map<MemberDto>(member);
        }

        public async Task<MemberDto> GetMember(string userName)
        {
            var member = await _repository.GetBySpecificationAsync(new MemberByUserNameSpecification(userName));

            Guard.Against.Null(member, nameof(member));

            return _mapper.Map<MemberDto>(member);
        }

        public async Task<MemberTransactionTemplateDto> GetMemberWithTransactionTemplate(int memberId)
        {
            var member = await _repository.GetBySpecificationAsync(new MemberWithTransactionTemplateSpecification(memberId));
            Guard.Against.Null(member, nameof(member));
            var output = _mapper.Map<MemberTransactionTemplateDto>(member.TransactionTemplate);
            output.MemberName = member.UserName;
            return output;
        }

        public async Task<List<MemberDto>> GetMembers()
        {
            var members = await _repository.ListAsync<Member>();

            return _mapper.Map<List<MemberDto>>(members);
        }

        public async Task<List<MemberDto>> GetMembers(MemberFilterDto filterDto)
        {
            var specification = new MemberSpecification(_mapper.Map<MemberFilter>(filterDto));
            var members = await _repository.ListAsync(specification);

            return _mapper.Map<List<MemberDto>>(members);
        }

        public async Task EnableMember(int memberId)
        {
            var member = await _repository.GetByIdAsync<Member>(memberId);
            Guard.Against.Null(member, nameof(member));
            member.Status = true;
            await _repository.UpdateAsync(member);
        }

        public async Task DisableMember(int memberId)
        {
            var member = await _repository.GetByIdAsync<Member>(memberId);

            Guard.Against.Null(member, nameof(member));

            member.Status = false;
            await _repository.UpdateAsync(member);
        }

        public async Task DeleteMember(int clientId)
        {
            var member = await _repository.GetByIdAsync<Member>(clientId);

            Guard.Against.Null(member, nameof(member));

            member.Status = false;
            await _repository.UpdateAsync(member);
        }

        public async Task DeleteMembers(MemberFilterDto filterDto)
        {
            var specification = new MemberSpecification(_mapper.Map<MemberFilter>(filterDto));
            var members = await _repository.ListAsync(specification);

            foreach (var member in members)
            {
                member.Status = false;
                await _repository.UpdateAsync(member);
            }
        }

        public async Task CrawlMembers()
        {
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

            var request = new CrawlerRequest
            {
                BaseUrl = _hostApiCrawlerSettings.AllClientListUrl,
                Cookies = authResponse.Cookies
            };

            var document = await _downloader.DownloadAsync(request);
            var memberCtos = _processor.Process<MemberCto>(document);

            var existingMembers = await _repository.ListAsync<Member>();
            var existingMemberNames = existingMembers.Select(x => x.UserName).ToList();

            memberCtos = memberCtos.Where(x => !string.IsNullOrWhiteSpace(x.UserName) && !existingMemberNames.Contains(x.UserName));

            var members = _mapper.Map<List<Member>>(memberCtos);

            foreach (var member in members)
            {
                var transactionTemplate = new TransactionTemplate();
                member.TransactionTemplate = transactionTemplate.GetDefaultTransactionTemplate();
            }
            await _pipeline.RunAsync(members);
        }

        public async Task<List<int>> CrawlActiveMembers()
        {
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

            var request = new CrawlerRequest
            {
                BaseUrl = _hostApiCrawlerSettings.ClientListUrl,
                Cookies = authResponse.Cookies
            };

            var document = await _downloader.DownloadAsync(request);
            var memberCtos = _processor.Process<ActiveMemberCto>(document);
            var activerMemberNames = memberCtos.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName.Replace("*", "")).ToList();

            return (await _repository.ListAsync(new AcitveMembersSpecification(activerMemberNames)))
                .Select(x => x.Id)
                .ToList();
        }
    }
}
