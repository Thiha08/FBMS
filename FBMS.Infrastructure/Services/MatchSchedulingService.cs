using FBMS.Core.Constants;
using FBMS.Core.Constants.Crawler;
using FBMS.Core.Ctos;
using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Auth;
using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Extensions;
using FBMS.Core.Interfaces;
using FBMS.Spider.Auth;
using FBMS.Spider.Downloader;
using FBMS.Spider.Processor;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.Services
{
    public class MatchSchedulingService : IMatchSchedulingService
    {
        private readonly ICrawlerDownloader _downloader;
        private readonly ICrawlerProcessor _processor;
        private readonly ICrawlerAuthorization _crawlerAuthorization;
        private readonly IClientApiCrawlerSettings _clientApiCrawlerSettings;

        public MatchSchedulingService(ICrawlerDownloader downloader, ICrawlerProcessor processor, ICrawlerAuthorization crawlerAuthorization, IClientApiCrawlerSettings clientApiCrawlerSettings)
        {
            _downloader = downloader;
            _processor = processor;
            _crawlerAuthorization = crawlerAuthorization;
            _clientApiCrawlerSettings = clientApiCrawlerSettings;
        }

        public async Task<List<MatchDto>> GetMatchSchedule()
        {
            var authResponse = await GetClientApiAuthentication();
            var scheduleUrl = await GetMatchScheduleUrl(authResponse);

            var request = new CrawlerRequest
            {
                BaseUrl = _clientApiCrawlerSettings.MatchScheduleBaseUrl + scheduleUrl,
                Cookies = authResponse.Cookies
            };
            var document = await _downloader.DownloadAsync(request);
            return DeserializeMatchSchedule(document.Text);
        }

        public async Task<MatchDetailDto> GetMatchDetail(string matchUrl)
        {
            var authResponse = await GetClientApiAuthentication();
            var request = new CrawlerRequest
            {
                BaseUrl = _clientApiCrawlerSettings.MatchDetailBaseUrl + matchUrl,
                Cookies = authResponse.Cookies
            };
            var document = await _downloader.DownloadAsync(request);
            return DeserializeMatchDetail(document.Text);
        }

        public async Task<string> SubmitMatchTransaction(MatchBetDto dto)
        {
            var authResponse = await GetClientApiAuthentication();
            var request = new CrawlerRequest
            {
                BaseUrl = _clientApiCrawlerSettings.MatchDetailBaseUrl + dto.GetBetUrl(),
                Cookies = authResponse.Cookies
            };
            var document = await _downloader.DownloadAsync(request);
            return document.Text;
        }

        private async Task<AuthResponse> GetClientApiAuthentication()
        {
            var authResponse = await _crawlerAuthorization.IsSignedInAsync(_clientApiCrawlerSettings.Url, CacheKeys.SodeAuthCookies);

            if (!authResponse.isSignedIn)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(authResponse.HtmlCode);
                var formData = (_processor.Process<SignInCto>(htmlDocument)).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(formData.AuthUrl))
                {
                    var authRequest = new AuthRequest
                    {
                        AuthUrl = _clientApiCrawlerSettings.AuthUrl + formData.AuthUrl.Replace("./", ""),
                        Cookies = authResponse.Cookies
                    };

                    authRequest.RequestForm = new SignInDto
                    {
                        EventTarget = "btnSignIn",
                        EventArgument = "",
                        EventValidation = formData.EventValidation,
                        ViewState = formData.ViewState,
                        ViewStateGenerator = formData.ViewStateGenerator,
                        TxtUserName = _clientApiCrawlerSettings.UserName,
                        Password = _clientApiCrawlerSettings.Password
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

        private async Task<string> GetMatchScheduleUrl(AuthResponse authResponse)
        {
            var request = new CrawlerRequest
            {
                BaseUrl = _clientApiCrawlerSettings.MatchScheduleUrl,
                Cookies = authResponse.Cookies
            };
            var document = await _downloader.DownloadAsync(request);
            HtmlNode head = document.DocumentNode.SelectSingleNode("/html/head");

            // Grab the content of the first script element
            var script = head.Descendants()
                             .Where(n => n.Name == "script")
                             .LastOrDefault()
                             .InnerText;

            var todayUrl = Regex.Match(script, @"timerToday\('(.+?)',").Groups[1].Value;
            return todayUrl;
        }

        private List<MatchDto> DeserializeMatchSchedule(string scheduleJson)
        {
            try
            {
                IEnumerable<object> firstLevelList = Enumerable.Empty<object>();
                IEnumerable<object> secondLevelList = Enumerable.Empty<object>();
                IEnumerable<object> thirdLevelList = Enumerable.Empty<object>();
                IEnumerable<object> fouthLevelList = Enumerable.Empty<object>();

                var obj = JsonConvert.DeserializeObject<object>(scheduleJson);
                firstLevelList = (IEnumerable<object>)obj;
                secondLevelList = firstLevelList.GetEnumerableByDepth(2);
                foreach (var item in secondLevelList)
                {
                    thirdLevelList = (IEnumerable<object>)item;
                    fouthLevelList = fouthLevelList.Concat(thirdLevelList.GetEnumerableByDepth(1));
                }

                var matchList = new List<MatchDto>();

                int index = 0;
                foreach (IEnumerable<object> item in fouthLevelList)
                {
                    var entityList = item.ToList();
                    var match = new MatchDto();
                    match.Oddsid = Convert.ToInt64(entityList[0].ToString());
                    match.League = entityList[1].ToString();
                    match.Soclid = Convert.ToInt64(entityList[2].ToString());
                    match.EventKey = entityList[7].ToString();
                    match.Ep = Convert.ToInt32(entityList[18].ToString());
                    match.HomeTeam = entityList[19].ToString();
                    match.AwayTeam = entityList[20].ToString();
                    match.FtHdpPricing = Convert.ToDecimal(entityList[24].ToString());
                    match.HomeAmount = Convert.ToDecimal(entityList[26].ToString());
                    match.AwayAmount = Convert.ToDecimal(entityList[27].ToString());
                    match.FtOuPricing = Convert.ToDecimal(entityList[29].ToString());
                    match.OverAmount = Convert.ToDecimal(entityList[33].ToString());
                    match.UnderAmount = Convert.ToDecimal(entityList[34].ToString());
                    match.MatchDate = DateTime.ParseExact(entityList[63].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                    matchList.Add(match);
                    index++;
                }
                return matchList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MatchDetailDto DeserializeMatchDetail(string detailJson)
        {
            try
            {
                var matchDetail = JsonConvert.DeserializeObject<MatchDetailDto>(detailJson);
                return matchDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
