using GoldenTicket.Model;
using System.Collections.Generic;

namespace GoldenTicket.Data.Interfaces
{
    public interface ISettingsDataProvider<TRawEntity> : IDataProvider<SettingsItem, TRawEntity>
         where TRawEntity : class
    {
        IDictionary<string, SettingsItem> GetOfflineSettings();
        SettingsItem GetSettingsItemByName(string name);
        void Save(SettingsItem item);
    }
}
