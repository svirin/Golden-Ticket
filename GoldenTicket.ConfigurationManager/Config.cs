using System.Configuration;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Parse;

namespace GoldenTicket.ConfigurationManager
{
    public static class Config
    {

        private static ElementProxy _currentSettings;

        public static dynamic Settings
        {
            get { return _currentSettings ?? (_currentSettings = new ElementProxy()); }
        }

        public static string ApplicationId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationId");
            }
        }

        public static string DotNetKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("DotNetKey");
            }
        }

        private class ElementProxy : DynamicObject
        {
            private readonly ISettingsDataProvider<ParseObject> _provider;
            private readonly IDictionary<string, SettingsItem> _dictionary;

            public ElementProxy()
            {
                _provider = DI.Factory.GetInstance<ISettingsDataProvider<ParseObject>>();
                _dictionary = _provider.GetOfflineSettings();
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var settingsResult = CheckOffline(binder.Name) ?? CheckOnline(binder.Name);

                var typeOfSetting = Type.GetType(settingsResult.Type);

                if (typeOfSetting == null)

                    throw new FormatException(string.Format("Type of propety {0} not declared", binder.Name));

                result = Convert.ChangeType(settingsResult.Value, typeOfSetting);
                
                return true;
            }

            private SettingsItem CheckOffline(string name)
            {
                SettingsItem settingsItem;

                if (!_dictionary.TryGetValue(name, out settingsItem))

                    return null;

                return settingsItem;
            }

            private SettingsItem CheckOnline(string name)
            {
                var settingsItem = _provider.GetSettingsItemByName(name);

                return settingsItem;
            }
        }
    }
}
