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
    public class RecientDataProvider : IRecientDataProvider<ParseObject>
    {
        #region Get

        public Recient Get(string objectId)
        {
            var query = from concert in ParseObject.GetQuery("Recent")
                        where concert.Get<string>("objectId") == objectId
                        select concert;

            var result = query.FindAsync().Result;

            var resultList = result as IList<ParseObject> ?? result.ToList();

            if (!resultList.Any())
                throw new DataException(string.Format("Concert with id #{0} does not existed", objectId));

            var item = Convert(resultList.Single());

            return item;
        }

        public IEnumerable<Recient> GetRecientItems()
        {
            var query = from recient in ParseObject.GetQuery("Recent")
                        orderby recient.Get<string>("Username"), recient.Get<string>("Username")
                        select recient;

            var result = query.FindAsync().Result;
            var recientResultSet = result.Select(Convert);
            return recientResultSet;
        }

        public IEnumerable<Recient> GetRecientItems(string username)
        {
            var query = from recient in ParseObject.GetQuery("Recent")
                        where recient.Get<string>("Username") == username
                        select recient;

            var result = query.FindAsync().Result;
            var recientResultSet = result.Select(Convert);
            return recientResultSet;
        }

        #endregion

        #region Save

        public void Save(Recient item)
        {
            var prsSuggest = Convert(item);

            prsSuggest.SaveAsync().Wait();

            item.UniqueID = prsSuggest.ObjectId;
        }

        public void SaveMany(IEnumerable<Recient> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region Delete

        public void Delete(Recient item)
        {
            var prsUser = Convert(item);

            prsUser.DeleteAsync().Wait();

            LogFactory.Log.InfoFormat("Recient #{0} successfuly deleted", item.UniqueID);
        }

        public void DeleteMany(IEnumerable<Recient> items)
        {
            foreach (var item in items)
            {
                Delete(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(Recient item)
        {
            var query = from suggestionResult in ParseObject.GetQuery("Recent")
                where suggestionResult.Get<string>("Username") == item.Username &&
                      suggestionResult.Get<string>("ConcertId") == item.ConcertId
                select suggestionResult;

            var result = query.FindAsync().Result;

            return result.Any();
        }

        #endregion

        #region Convert

        public ParseObject Convert(Recient item)
        {
            var concert = new ParseObject("Recent")
            {
                ObjectId = item.UniqueID
            };

            concert["Username"] = item.Username.ToCustomLower();
            concert["ConcertId"] = item.ConcertId;
            
            return concert;
        }

        public Recient Convert(ParseObject item)
        {
            var recientResult = new Recient
            {
                UniqueID = item.ObjectId,
                Username = item.Get<string>("Username"),
                ConcertId = item.Get<string>("ConcertId")
            };

            return recientResult;
        }

        #endregion
    }
}
