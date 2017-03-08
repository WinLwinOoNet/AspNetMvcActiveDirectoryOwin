using System.Collections.Generic;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Settings
{
    public interface ISettingService
    {
        Setting GetSettingById(int settingId);
        
        string GetSettingByKey(string key, string defaultValue);
        
        T GetSettingByKey<T>(string key, T defaultValue);
        
        IList<Setting> GetAllSettings();
        
        void UpdateSetting(Setting setting);
    }
}