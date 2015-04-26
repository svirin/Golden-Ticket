using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Logger.Log4Net;
using GoldenTicket.Model;
using GoldenTicket.Utilities;
using Parse;

namespace GoldenTicket.DataProxy.Parse
{
    public class SettingsDataProvider : ISettingsDataProvider<ParseObject>
    {
        #region Get

        public IDictionary<string, SettingsItem> GetOfflineSettings()
        {
            var query = from settingsItem in ParseObject.GetQuery("SettingsItem")
                        where settingsItem.Get<Boolean>("IsOnline") == false
                        select settingsItem;

            var result = query.FindAsync().Result;

            var settings = result.Select(Convert);

            var disctionary = settings.ToDictionary(settingsItem => settingsItem.Name);

            return disctionary;
        }

        public SettingsItem GetSettingsItemByName(string name)
        {
            var query = from settingsItem in ParseObject.GetQuery("SettingsItem")
                        where settingsItem.Get<string>("Name") == name
                        select settingsItem;

            var result = query.FindAsync().Result;

            var resultList = result as IList<ParseObject> ?? result.ToList();

            if (!resultList.Any())
                throw new DataException(string.Format("Settings with name #{0} does not existed", name));

            var item = Convert(resultList.Single());

            return item;


        }

        public void Save(SettingsItem item)
        {
            var prsConcert = Convert(item);

            prsConcert.SaveAsync().Wait();

            item.UniqueID = prsConcert.ObjectId;

            LogFactory.Log.InfoFormat("SettingsItem #{0} saved successfuly", item.UniqueID);
        }

        #endregion

        #region IDataProvider

        public ParseObject Convert(SettingsItem item)
        {
            var settingsItem = new ParseObject("SettingsItem");

            settingsItem["Name"] = item.Name.ToCustomLower();
            settingsItem["IsOnline"] = item.IsOnline;
            settingsItem["Block"] = item.Block.ToCustomLower();
            settingsItem["Type"] = item.Type.ToCustomLower();
            settingsItem["Value"] = item.Value.ToCustomLower();

            return settingsItem;
        }

        public SettingsItem Convert(ParseObject item)
        {
            var settingsItem = new SettingsItem
            {
                UniqueID = item.ObjectId,
                Name = item.Get<string>("Name"),
                IsOnline = item.Get<bool>("IsOnline"),
                Block = item.Get<string>("Block"),
                Type = item.Get<string>("Type"),
                Value = item.Get<string>("Value")
            };

            return settingsItem;
        }

        #endregion

    }
}
