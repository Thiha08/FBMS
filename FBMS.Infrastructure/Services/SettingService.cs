using Ardalis.GuardClauses;
using AutoMapper;
using FBMS.Core.Dtos;
using FBMS.Core.Entities;
using FBMS.Core.Interfaces;
using FBMS.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.Services
{

    public class SettingService : ISettingService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public SettingService(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<SettingDto> GetSetting(int settingId)
        {
            var setting = await _repository.GetByIdAsync<Setting>(settingId);
            return _mapper.Map<SettingDto>(setting);
        }

        public async Task<List<SettingDto>> GetSettings()
        {
            var settings = (await _repository.ListAsync<Setting>())
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Key)
                .ToList();

            return _mapper.Map<List<SettingDto>>(settings);
        }

        public async Task EnableSetting(int settingId)
        {
            var setting = await _repository.GetByIdAsync<Setting>(settingId);
            Guard.Against.Null(setting, nameof(setting));
            setting.Status = true;
            await _repository.UpdateAsync(setting);
        }

        public async Task DisableSetting(int settingId)
        {
            var setting = await _repository.GetByIdAsync<Setting>(settingId);

            Guard.Against.Null(setting, nameof(setting));

            setting.Status = false;
            await _repository.UpdateAsync(setting);
        }

        public async Task CreateSetting(SettingDto settingDto)
        {
            await _repository.AddAsync(_mapper.Map<Setting>(settingDto));
        }

        public async Task UpdateSetting(SettingDto settingDto)
        {
            var existingSetting = await _repository.GetByIdAsync<Setting>(settingDto.Id);

            Guard.Against.Null(existingSetting, nameof(existingSetting));

            //existingSetting.Name = settingDto.Name;
            //existingSetting.Key = settingDto.Key;
            existingSetting.Value = settingDto.Value;

            await _repository.UpdateAsync(existingSetting);
        }

        public async Task DeleteSetting(int settingId)
        {
            var setting = await _repository.GetByIdAsync<Setting>(settingId);

            Guard.Against.Null(setting, nameof(setting));
            await _repository.DeleteAsync(setting);
        }

        public async Task<List<SettingDto>> GetHostApiCrawlerSettings()
        {
            var settings = (await _repository.ListAsync<Setting>())
                .Where(x => x.Name == "HostApiCrawlerSettings" && x.Status)
                .ToList();

            return _mapper.Map<List<SettingDto>>(settings);
        }

        public async Task<List<SettingDto>> GetClientApiCrawlerSettings()
        {
            var settings = (await _repository.ListAsync<Setting>())
                .Where(x => x.Name == "ClientApiCrawlerSettings" && x.Status)
                .ToList();

            return _mapper.Map<List<SettingDto>>(settings);
        }
    }
}
