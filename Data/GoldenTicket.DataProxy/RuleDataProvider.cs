using System.Collections.Generic;
using System.Data;
using System.Linq;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Logger.Log4Net;
using Parse;
using Rule = GoldenTicket.Model.Rule;

namespace GoldenTicket.DataProxy.Parse
{
    public class RuleDataProvider : IRuleDataProvider<ParseObject>
    {
        #region Get

        public Rule GetRuleBySource(string concertId)
        {
            var query = from ruleResult in ParseObject.GetQuery("Rule")
                        where ruleResult.Get<string>("SourceConcertId") == concertId
                        select ruleResult;

            var result = query.FindAsync().Result.ToList();
            if (!result.Any())
                throw new DataException(string.Format("Rule with ConcertId #{0} does not existed", concertId));

            var item = Convert(result.Single());

            return item;
        }

        public IEnumerable<Rule> GetRulesBySources(IEnumerable<string> sourcesIds)
        {
            var query = ParseObject.GetQuery("Rule").WhereContainedIn("SourceConcertId", sourcesIds);

            var result = query.FindAsync().Result;
            var convertedResult = result.Select(Convert);

            return convertedResult;
        }

        #endregion

        #region Save

        public void Save(Rule item)
        {
            var prsRule = Convert(item);

            prsRule.SaveAsync().Wait();

            item.UniqueID = prsRule.ObjectId;
        }

        public void UpdateRates(Rule item)
        {
            var prsConcert = ParseObject.CreateWithoutData("Rule", item.UniqueID);

            prsConcert["TargetConcertIds"] = item.TargetConcertIds;
            prsConcert["Support"] = item.Support;
            prsConcert["Confidence"] = item.Confidence;

            prsConcert.SaveAsync().Wait();
            LogFactory.Log.InfoFormat("Rule #{0} successfuly saved", item.UniqueID);
        }

        #endregion

        #region Delete

        public void Delete(Rule item)
        {
            var prsUser = Convert(item);

            prsUser.DeleteAsync().Wait();

            LogFactory.Log.InfoFormat("Rule #{0} successfuly deleted", item.UniqueID);
        }

        public void DeleteMany(IEnumerable<Rule> items)
        {
            foreach (var item in items)
            {
                Delete(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsRuleExisted(string sourceConcert)
        {
            var query = from ruleResult in ParseObject.GetQuery("Rule")
                        where ruleResult.Get<string>("SourceConcertId") == sourceConcert
                        select ruleResult;

            var result = query.FindAsync().Result;
            return result.Any();
        }

        #endregion

        #region Convert

        public ParseObject Convert(Rule item)
        {
            var concert = new ParseObject("Rule")
            {
                ObjectId = item.UniqueID
            };

            concert["SourceConcertId"] = item.SourceConcertId;
            concert["TargetConcertIds"] = item.TargetConcertIds;
            concert["Support"] = item.Support;
            concert["Confidence"] = item.Confidence;

            return concert;
        }

        public Rule Convert(ParseObject item)
        {
            var suggestionResult = new Rule
            {
                UniqueID = item.ObjectId,
                SourceConcertId = item.Get<string>("SourceConcertId"),
                TargetConcertIds = item.Get<string>("TargetConcertIds"),
                Support = item.Get<double>("Support"),
                Confidence = item.Get<double>("Confidence")
            };

            return suggestionResult;
        }

        #endregion

    }
}
