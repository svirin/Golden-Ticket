using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Logger.Log4Net;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.DataProxy.Parse
{
    public class SearchResultDataProvider : ISearchResultDataProvider<ParseObject>
    {
        #region Get

        public SearchResult Get(string objectId)
        {
            var query = from concert in ParseObject.GetQuery("Concert")
                        where concert.Get<string>("objectId") == objectId
                        select concert;

            var task = query.FindAsync();
            var resultSet = task.GetAwaiter().GetResult().ToList();

            if (!resultSet.Any())
                throw new DataException(string.Format("Concert with id #{0} does not existed", objectId));

            var item = Convert(resultSet.Single());

            return item;
        }

        public IEnumerable<SearchResult> GetMany()
        {
            return null;
        }

        public IEnumerable<SearchResult> SearchResultByUser(User user)
        {
            var query = from searchResult in ParseObject.GetQuery("SearchResult")
                        where searchResult.Get<Boolean>("SearchResult")
                        select searchResult;

            var task = query.FindAsync();
            var resultSet = task.GetAwaiter().GetResult();

            var searchResultSet = resultSet.Select(Convert);
            return searchResultSet;
        }

        #endregion

        #region Save

        public void Save(SearchResult item)
        {
            var prsConcert = Convert(item);

            prsConcert.SaveAsync().Wait();

            item.UniqueID = prsConcert.ObjectId;

            LogFactory.Log.InfoFormat("Search result #{0} saved", item.UniqueID);
        }

        public void SaveMany(IEnumerable<SearchResult> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(SearchResult item)
        {
            var query = from concert in ParseObject.GetQuery("Concert")
                        where concert.Get<string>("ConcertName") == item.ConcertName &&
                        concert.Get<string>("Abstract") == item.Abstract &&
                        concert.Get<string>("Genre") == item.Genre &&
                        concert.Get<string>("Artist") == item.Artist &&
                        concert.Get<string>("Region") == item.Region &&
                        concert.Get<string>("Country") == item.Country &&
                        concert.Get<string>("Arena") == item.Arena &&
                        concert.Get<string>("Description") == item.Description &&
                        concert.Get<DateTime>("DateStart") == item.DateStart &&
                        concert.Get<DateTime>("DateEnd") == item.DateEnd
                        select concert;

            var task = query.FindAsync();

            var resultSet = task.GetAwaiter().GetResult();

            return resultSet.Any();
        }

        #endregion

        #region Convert

        public ParseObject Convert(SearchResult item)
        {
            var concert = new ParseObject("Concert");

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
            concert["CrawlerName"] = item.CrawlerName;
            
            return concert;
        }

        public SearchResult Convert(ParseObject item)
        {
            var searchResult = new SearchResult
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
                CrawlerName = item.Get<string>("CrawlerName")
            };

            return searchResult;
        }

        #endregion
    }
}
