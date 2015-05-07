using System.Collections.Generic;
using System.Data;
using System.Linq;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Logger.Log4Net;
using GoldenTicket.Model;
using GoldenTicket.Utilities;
using Parse;

namespace GoldenTicket.DataProxy.Parse
{
    public class SuggestDataProvider : ISuggestDataProvider<ParseObject>
    {
        #region Get

        public Suggest Get(string objectId)
        {
            var query = from concert in ParseObject.GetQuery("Suggest")
                        where concert.Get<string>("objectId") == objectId
                        select concert;

            var result = query.FindAsync().Result;

            var resultList = result as IList<ParseObject> ?? result.ToList();

            if (!resultList.Any())
                throw new DataException(string.Format("Concert with id #{0} does not existed", objectId));

            var item = Convert(resultList.Single());

            return item;
        }

        public IEnumerable<Suggest> GetSuggestByUser(string username)
        {
            var query = from suggest in ParseObject.GetQuery("Suggest")
                        where suggest.Get<string>("Username") == username
                        select suggest;

            var result = query.FindAsync().Result;
            var suggestionResultSet = result.Select(Convert);
            return suggestionResultSet;
        }

        public IEnumerable<Suggest> GetSuggests()
        {
            var query = from suggest in ParseObject.GetQuery("Suggest")
                        orderby suggest.Get<string>("UniqueID"), suggest.Get<string>("UniqueID")
                        select suggest;

            var result = query.FindAsync().Result;
            var suggestionResultSet = result.Select(Convert);
            return suggestionResultSet;
        }

        public IEnumerable<Suggest> GetMany()
        {
            return GetSuggests();
        }

        #endregion

        #region Save

        public void Save(Suggest item)
        {
            var prsSuggest = Convert(item);

            prsSuggest.SaveAsync().Wait();

            item.UniqueID = prsSuggest.ObjectId;
        }

        public void SaveMany(IEnumerable<Suggest> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region Delete

        public void Delete(Suggest item)
        {
            var prsUser = Convert(item);
            prsUser.DeleteAsync().Wait();
            LogFactory.Log.InfoFormat("Suggest #{0} successfuly deleted", item.UniqueID);
        }

        public void DeleteMany(IEnumerable<Suggest> items)
        {
            foreach (var item in items)
            {
                Delete(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(Suggest item)
        {
            var query = from suggestionResult in ParseObject.GetQuery("Suggest")
                where suggestionResult.Get<string>("Username") == item.Username &&
                      suggestionResult.Get<string>("ConcertId") == item.ConcertId
                select suggestionResult;

            var result = query.FindAsync().Result;

            return result.Any();
        }

        #endregion

        #region Convert

        public ParseObject Convert(Suggest item)
        {
            var suggest = new ParseObject("Suggest")
            {
                ObjectId = item.UniqueID
            };

            suggest["Username"] = item.Username.ToCustomLower();
            suggest["ConcertId"] = item.ConcertId;

            return suggest;
        }

        public Suggest Convert(ParseObject item)
        {
            var suggestionResult = new Suggest
            {
                UniqueID = item.ObjectId,
                Username = item.Get<string>("Username"),
                ConcertId = item.Get<string>("ConcertId")
            };

            return suggestionResult;
        }

        #endregion
    }
}
