using FBMS.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface ISettingService
    {
        Task<SettingDto> GetSetting(int settingId);

        Task<List<SettingDto>> GetSettings();

        Task EnableSetting(int settingId);

        Task DisableSetting(int settingId);

        Task CreateSetting(SettingDto settingDto);

        Task UpdateSetting(SettingDto settingDto);

        Task DeleteSetting(int settingId);

        Task<List<SettingDto>> GetHostApiCrawlerSettings();

        Task<List<SettingDto>> GetClientApiCrawlerSettings();
    }
}
