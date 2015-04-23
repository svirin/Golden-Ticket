using System;
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
    public class ConcertDataProvider : IConcertDataProvider<ParseObject>
    {
        #region Get

        public Concert Get(string objectId)
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

        public IEnumerable<Concert> GetAll()
        {
            var query = from concert in ParseObject.GetQuery("Concert")
                        orderby concert.Get<string>("ConcertName")
                        select concert;

            var restult = query.FindAsync().Result;
            var searchResultSet = restult.Select(Convert);

            return searchResultSet;
        }

        public IEnumerable<Concert> GetByUserRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Concert> GetConcertsSuggestedToUser(string username)
        {
            var suggestProvider = DI.Factory.GetInstance<ISuggestDataProvider<ParseObject>>();
            var suggests = suggestProvider.GetSuggestByUser(username).ToList();
            var suggestsIds = suggests.Select(item => item.ConcertId);
            var concerts = GetByIds(suggestsIds);
            return concerts;
        }

        public IEnumerable<Concert> GetConcertsSuggestedToConcert(string concertId)
        {
            var ruleProvider = DI.Factory.GetInstance<IRuleDataProvider<ParseObject>>();
            var rule = ruleProvider.GetRuleBySource(concertId);
            var targets = rule.TargetConcertIds.Split(new[] {","}, StringSplitOptions.None);
            var concerts = GetByIds(targets);
            return concerts;
        }

        //public IEnumerable<Concert> GetSuggestsToUser(string username)
        //{
        //    var recientProvider = DI.Factory.GetInstance<IRecientDataProvider<ParseObject>>();
        //    var recients = recientProvider.GetRecientItems(username).ToList();
        //    var recientIdents = recients.Select(item => item.ConcertId);
        //    var rulesProvider = DI.Factory.GetInstance<IRuleDataProvider<ParseObject>>();
        //    var rules = rulesProvider.GetRulesBySources(recientIdents);
        //    var rulesTargetIdents = rules.Select(item => item.TargetConcertIds);
        //    var rulesTargetIdentsAsOne = string.Join(",", rulesTargetIdents);
        //    var concertIds = rulesTargetIdentsAsOne.Split(new[] { "," }, StringSplitOptions.None);
        //    var concerts = GetByIds(concertIds);
        //    return concerts;
        //}

        private IEnumerable<Concert> GetByIds(IEnumerable<string> ids)
        {
            var query = ParseObject.GetQuery("Concert").WhereContainedIn("objectId", ids);
           
            var result = query.FindAsync().Result;
            var convertedResult = result.Select(Convert);

            return convertedResult;
        }

        #endregion

        #region Save

        public void Save(Concert item)
        {
            var prsConcert = Convert(item);

            prsConcert.SaveAsync().Wait();

            item.UniqueID = prsConcert.ObjectId;

            LogFactory.Log.InfoFormat("Search result #{0} saved", item.UniqueID);
        }

        public void SaveMany(IEnumerable<Concert> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region Delete

        public void Delete(Concert item)
        {
            var prsUser = Convert(item);

            prsUser.DeleteAsync().Wait();

            LogFactory.Log.InfoFormat("Concert #{0} successfuly deleted", item.UniqueID);
        }

        public void DeleteMany(IEnumerable<Concert> items)
        {
            foreach (var item in items)
            {
                Delete(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(Concert item)
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

        public ParseObject Convert(Concert item)
        {
            var concert = new ParseObject("Concert")
            {
                ObjectId = item.UniqueID
            };

            concert["ConcertName"] = item.ConcertName.ToCustomLower();
            concert["Abstract"] = item.Abstract.ToCustomLower();
            concert["Genre"] = item.Genre.ToCustomLower();
            concert["Artist"] = item.Artist.ToCustomLower();
            concert["Region"] = item.Region.ToCustomLower();
            concert["Country"] = item.Country.ToCustomLower();
            concert["City"] = item.City.ToCustomLower();
            concert["Arena"] = item.Arena.ToCustomLower();
            concert["Description"] = item.Description.ToCustomLower();
            concert["DateStart"] = item.DateStart;
            concert["DateEnd"] = item.DateEnd;
            concert["CrawlerName"] = item.CrawlerName.ToCustomLower();
            concert["CrawlerURL"] = item.CrawlerURL.ToCustomLower();
            concert["ImageURL"] = item.ImageURL.ToCustomLower();
            concert["NormalPrice"] = item.NormalPrice.ToCustomLower();
            concert["DiscountPrice"] = item.DiscountPrice.ToCustomLower();

            return concert;
        }

        public Concert Convert(ParseObject item)
        {
            var searchResult = new Concert
            {
                UniqueID = item.ObjectId,
                ConcertName = item.Get<string>("ConcertName"),
                Abstract = item.Get<string>("Abstract"),
                Genre = item.Get<string>("Genre"),
                Artist = item.Get<string>("Artist"),
                Region = item.Get<string>("Region"),
                Country = item.Get<string>("Country"),
                City = item.Get<string>("City"),
                Arena = item.Get<string>("Arena"),
                Description = item.Get<string>("Description"),
                DateStart = item.Get<DateTime>("DateStart"),
                DateEnd = item.Get<DateTime>("DateEnd"),
                CrawlerName = item.Get<string>("CrawlerName"),
                CrawlerURL = item.Get<string>("CrawlerURL"),
                ImageURL = item.Get<string>("ImageURL"),
                NormalPrice = item.Get<string>("NormalPrice"),
                DiscountPrice = item.Get<string>("DiscountPrice"),
            };

            return searchResult;
        }

        #endregion
    }
}
