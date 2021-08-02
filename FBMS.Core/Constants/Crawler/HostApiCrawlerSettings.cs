using FBMS.Core.Dtos;
using FBMS.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Core.Constants.Crawler
{
    public class HostApiCrawlerSettings : IHostApiCrawlerSettings
    {
        private readonly ISettingService _settingService;

        private readonly IConfiguration _configuration;

        //private static readonly object LockObject = new object();

        private bool _initialized = false;

        public string Url { get; set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string AuthUrl { get; set; }

        public string AllClientListUrl { get; set; }

        public string ClientListUrl { get; set; }

        public string ClientTransactionsUrl { get; set; }

        public string TimeZone { get; set; }

        public int TransitionHour { get; set; }

        public HostApiCrawlerSettings(ISettingService settingService, IConfiguration configuration)
        {
            _settingService = settingService;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;

            var settings = await _settingService.GetHostApiCrawlerSettings();

            SetUserName(settings);
            SetPassword(settings);
            SetAllClientListUrl();
            SetClientListUrl();
            SetClientTransactionsUrl();

            Url = _configuration["HostApiCrawlerSettings:Url"];
            AuthUrl = _configuration["HostApiCrawlerSettings:AuthUrl"];
            TimeZone = _configuration["HostApiCrawlerSettings:TimeZone"];
            TransitionHour = Convert.ToInt32(_configuration["HostApiCrawlerSettings:TransitionHour"]);
        }

        private void SetUserName(List<SettingDto> settings)
        {
            var userSetting = settings.FirstOrDefault(x => x.Key.ToLower() == nameof(UserName).ToLower());
            UserName = userSetting?.Value;

            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new UnauthorizedAccessException("HostApiCrawlerSettings:UserName is Empty!");
            }
        }

        private void SetPassword(List<SettingDto> settings)
        {
            var passwordSetting = settings.FirstOrDefault(x => x.Key.ToLower() == nameof(Password).ToLower());
            Password = passwordSetting?.Value;

            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new UnauthorizedAccessException("HostApiCrawlerSettings:Password is Empty!");
            }
        }

        private void SetAllClientListUrl()
        {
            AllClientListUrl = _configuration["HostApiCrawlerSettings:AllClientListUrl"];

            if (string.IsNullOrWhiteSpace(AllClientListUrl))
            {
                throw new UnauthorizedAccessException("HostApiCrawlerSettings:AllClientListUrl is Empty!");
            }

            AllClientListUrl = AllClientListUrl.Replace("{{USER_SHORT_NAME}}", UserName.Substring(0, 5));
        }

        private void SetClientListUrl()
        {
            ClientListUrl = _configuration["HostApiCrawlerSettings:ClientListUrl"];

            if (string.IsNullOrWhiteSpace(ClientListUrl))
            {
                throw new UnauthorizedAccessException("HostApiCrawlerSettings:ClientListUrl is Empty!");
            }

            ClientListUrl = ClientListUrl.Replace("{{USER_SHORT_NAME}}", UserName.Substring(0, 5));
        }


        private void SetClientTransactionsUrl()
        {
            ClientTransactionsUrl = _configuration["HostApiCrawlerSettings:ClientTransactionsUrl"];

            if (string.IsNullOrWhiteSpace(ClientTransactionsUrl))
            {
                throw new UnauthorizedAccessException("HostApiCrawlerSettings:ClientTransactionsUrl is Empty!");
            }

            ClientTransactionsUrl = ClientTransactionsUrl.Replace("{{USER_SHORT_NAME}}", UserName.Substring(0, 5));
        }
    }
}
