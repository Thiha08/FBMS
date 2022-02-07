using FBMS.Core.Dtos;
using FBMS.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Core.Constants.Crawler
{
    public class ClientApiCrawlerSettings : IClientApiCrawlerSettings
    {
        private readonly ISettingService _settingService;

        private readonly IConfiguration _configuration;

        //private static readonly object LockObject = new object();

        private bool _initialized = false;

        public string Url { get; set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string AuthUrl { get; set; }

        public string MatchScheduleBaseUrl { get; set; }

        public string MatchScheduleUrl { get; set; }

        public string MatchDetailBaseUrl { get; set; }

        public string TimeZone { get; set; }

        public int AcceptablePassedMinute { get; set; }

        public int AcceptableDischargedCount { get; set; }

        public bool IsTestingStack { get; set; } // if testing stack, stack = 1

        public ClientApiCrawlerSettings(ISettingService settingService, IConfiguration configuration)
        {
            _settingService = settingService;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;

            var settings = await _settingService.GetClientApiCrawlerSettings();

            SetUserName(settings);
            SetPassword(settings);
            SetAcceptablePassedMinute(settings);
            SetAcceptableDischargedCount(settings);
            SetIsTestingStack(settings);

            SetSettingsFromAppConfig();
            _initialized = true;
        }

        private void SetUserName(List<SettingDto> settings)
        {
            var setting = settings.FirstOrDefault(x => x.Key.ToLower() == nameof(UserName).ToLower());
            UserName = setting?.Value;

            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new UnauthorizedAccessException($"ClientApiCrawlerSettings:UserName is Empty!");
            }
        }

        private void SetPassword(List<SettingDto> settings)
        {
            var setting = settings.FirstOrDefault(x => x.Key.ToLower() == nameof(Password).ToLower());
            Password = setting?.Value;

            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new UnauthorizedAccessException($"ClientApiCrawlerSettings:Password is Empty!");
            }
        }

        private void SetAcceptablePassedMinute(List<SettingDto> settings)
        {
            try
            {
                var setting = settings.FirstOrDefault(x => x.Key.ToLower() == nameof(AcceptablePassedMinute).ToLower());
                AcceptablePassedMinute = int.Parse(setting.Value);
            }
            catch
            {
                AcceptablePassedMinute = CoreConstants.DefaultAcceptablePassedMinute;
            }
        }

        private void SetAcceptableDischargedCount(List<SettingDto> settings)
        {
            try
            {
                var setting = settings.FirstOrDefault(x => x.Key.ToLower() == nameof(AcceptableDischargedCount).ToLower());
                AcceptableDischargedCount = int.Parse(setting.Value);
            }
            catch
            {
                AcceptableDischargedCount = CoreConstants.DefaultAcceptableDischargedCount;
            }
        }

        private void SetIsTestingStack(List<SettingDto> settings)
        {
            try
            {
                var setting = settings.FirstOrDefault(x => x.Key.ToLower() == nameof(IsTestingStack).ToLower());
                IsTestingStack = bool.Parse(setting.Value);
            }
            catch
            {
                IsTestingStack = CoreConstants.DefaultIsTestingStack;
            }
        }


        private void SetSettingsFromAppConfig()
        {
            Url = _configuration["ClientApiCrawlerSettings:Url"];
            AuthUrl = _configuration["ClientApiCrawlerSettings:AuthUrl"];
            MatchScheduleBaseUrl = _configuration["ClientApiCrawlerSettings:MatchScheduleBaseUrl"];
            MatchScheduleUrl = _configuration["ClientApiCrawlerSettings:MatchScheduleUrl"];
            MatchDetailBaseUrl = _configuration["ClientApiCrawlerSettings:MatchDetailBaseUrl"];
            TimeZone = _configuration["ClientApiCrawlerSettings:TimeZone"];
        }
    }
}
