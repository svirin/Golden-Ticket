using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Logger.Log4Net;
using GoldenTicket.Model;
using Parse;
using System;

namespace GoldenTicket.DataProxy.Parse
{
    public class SuggestionResultDataProvider : ISuggestionResultDataProvider<ParseObject>
    {
        #region Get

        public SuggestionResult Get(string objectId)
        {
            var query = from concert in ParseObject.GetQuery("Suggestion")
                        where concert.Get<string>("objectId") == objectId
                        select concert;

            var result = query.FindAsync().Result;

            var resultList = result as IList<ParseObject> ?? result.ToList();

            if (!resultList.Any())
                throw new DataException(string.Format("Concert with id #{0} does not existed", objectId));

            var item = Convert(resultList.Single());

            return item;
        }

        public IEnumerable<SuggestionResult> GetSuggestionResultByUser(User user)
        {
            var query = from suggestionResult in ParseObject.GetQuery("SuggestionResult")
                        where suggestionResult.Get<Boolean>("SuggestionResult")
                        select suggestionResult;

            var result = query.FindAsync().Result;

            var suggestionResultSet = result.Select(Convert);

            return suggestionResultSet;
        }

        #endregion

        #region Save

        public void Save(SuggestionResult item)
        {
            throw new System.NotImplementedException();
        }

        public void SaveMany(IEnumerable<SuggestionResult> items)
        {
            foreach (var item in items)
            {
                Thread.Sleep(150);
                //LogFactory.Log.Info("Save response #" + item.UniqueID);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(SuggestionResult item)
        {
            var query = from suggestionResult in ParseObject.GetQuery("SuggestionResult")
                        where suggestionResult.Get<string>("ConcertName") == item.ConcertName &&
                        suggestionResult.Get<string>("Abstract") == item.Abstract &&
                        suggestionResult.Get<string>("Genre") == item.Genre &&
                        suggestionResult.Get<string>("Artist") == item.Artist &&
                        suggestionResult.Get<string>("Region") == item.Region &&
                        suggestionResult.Get<string>("Country") == item.Country &&
                        suggestionResult.Get<string>("Arena") == item.Arena &&
                        suggestionResult.Get<string>("Description") == item.Description &&
                        suggestionResult.Get<DateTime>("DateStart") == item.DateStart &&
                        suggestionResult.Get<DateTime>("DateEnd") == item.DateEnd
                        select suggestionResult;

            var task = query.FindAsync();
            var resultSet = task.GetAwaiter().GetResult();

            return resultSet.Any();
        }

        #endregion

        #region Convert

        public ParseObject Convert(SuggestionResult item)
        {
            var concert = new ParseObject("SuggestionResult");

            concert["ConcertName"] = item.ConcertName;
            concert["Abstract"] = item.Abstract;
            concert["Genre"] = item.Genre;
            concert["Artist"] = item.Artist;
            concert["Region"] = item.Region;
            concert["Country"] = item.Country;
            concert["Arena"] = item.Arena;
            concert["Description"] = item.Description;
            concert["DateStart"] = item.DateStart;
            concert["DateEnd"] = item.DateEnd;
            concert["UserName"] = item.UserName;
            
            return concert;
        }

        public SuggestionResult Convert(ParseObject item)
        {
            var suggestionResult = new SuggestionResult
            {
                UniqueID = item.ObjectId,
                ConcertName = item.Get<string>("ConcertName"),
                Abstract = item.Get<string>("Abstract"),
                Genre = item.Get<string>("Genre"),
                Artist = item.Get<string>("Artist"),
                Region = item.Get<string>("Region"),
                Country = item.Get<string>("Country"),
                Arena = item.Get<string>("Arena"),
                Description = item.Get<string>("Description"),
                DateStart = item.Get<DateTime>("DateStart"),
                DateEnd = item.Get<DateTime>("DateEnd"),
                UserName = item.Get<string>("UserName"),
            };

            return suggestionResult;
        }

        #endregion
    }
}
