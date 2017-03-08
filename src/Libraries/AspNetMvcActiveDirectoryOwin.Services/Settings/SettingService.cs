using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using AspNetMvcActiveDirectoryOwin.Core.Caching;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Settings
{
    public class SettingService : ISettingService
    {
        private const string SettingsAllKey = "setting.all";
        private readonly IRepository<Setting> _repository;
        private readonly ICacheService _cacheService;

        public SettingService(ICacheService cacheService,
            IRepository<Setting> repository)
        {
            _cacheService = cacheService;
            _repository = repository;
        }

        public Setting GetSettingById(int settingId)
        {
            if (settingId == 0)
                return null;

            var query = _repository.Entities
                .Where(b => b.Id == settingId);

            return query.FirstOrDefault();
        }

        public string GetSettingByKey(string key, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
                return defaultValue;

            var settings = GetAllSettings();
            key = key.Trim().ToLowerInvariant();

            var setting = settings.FirstOrDefault(x => x.Name == key);

            if (setting != null)
                return setting.Value;

            return defaultValue;
        }

        public T GetSettingByKey<T>(string key, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
                return defaultValue;

            var settings = GetAllSettings();
            key = key.Trim().ToLowerInvariant();

            var setting = settings.FirstOrDefault(x => x.Name == key);

            if (setting != null)
                return (T) Convert.ChangeType(setting.Value, typeof (T));

            return defaultValue;
        }

        public IList<Setting> GetAllSettings()
        {
            string key = string.Format(SettingsAllKey);
            return _cacheService.Get(key, () =>
            {
                var query = _repository.Entities
                    .OrderBy(s => s.Name);

                return query.ToList();
            });
        }

        public void UpdateSetting(Setting setting)
        {
            _repository.Entities.AddOrUpdate(setting);
            _repository.SaveChangesAsync();

            _cacheService.Remove(SettingsAllKey);
        }
    }
}