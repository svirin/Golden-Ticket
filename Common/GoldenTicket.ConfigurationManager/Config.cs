using System.Configuration;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Parse;

namespace GoldenTicket.ConfigurationManager
{
    public class Config
    {

        private static Config _config = new Config();

        public static Config CurrentContext
        {
            get{ return _config; }
        }

        private static SettingsRetrieverProxy _retrieverSettings;

        public dynamic AppSettings
        {
            get { return _retrieverSettings ?? (_retrieverSettings = new SettingsRetrieverProxy()); }
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

        private class SettingsRetrieverProxy : DynamicObject
        {
            private readonly ISettingsDataProvider<ParseObject> _provider;
            private readonly IDictionary<string, SettingsItem> _dictionary;

            public SettingsRetrieverProxy()
            {
                _provider = DI.Factory.GetInstance<ISettingsDataProvider<ParseObject>>();
                _dictionary = _provider.GetOfflineSettings();
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var settingsResult = CheckOffline(binder.Name) ?? CheckOnline(binder.Name);

                var convertor = new SettingsConvertorProxy(settingsResult.Value);

                result = convertor;

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

        private class SettingsConvertorProxy : DynamicObject
        {
            private object _result;

            public SettingsConvertorProxy(object result)
            {
                _result = result;
            }

            public override bool TryConvert(ConvertBinder binder, out object result)
            {
                result = binder.ReturnType.BaseType == typeof(Enum) ? 
                    Enum.Parse(binder.ReturnType, _result.ToString()):
                    Convert.ChangeType(_result, binder.ReturnType);

 	             return true;
            }
        }
    }
}
