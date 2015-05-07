using System.Collections.Generic;
using System.Data;
using System.Linq;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using GoldenTicket.Utilities;
using Parse;

namespace GoldenTicket.DataProxy.Parse
{
    public class JoinDataProvider : IJoinDataProvider<ParseObject>
    {
        #region Get

        public Join Get(string objectId)
        {
            var query = from concert in ParseObject.GetQuery("Join")
                        where concert.Get<string>("objectId") == objectId
                        select concert;

            var result = query.FindAsync().Result;

            var resultList = result as IList<ParseObject> ?? result.ToList();

            if (!resultList.Any())
                throw new DataException(string.Format("Visit with id #{0} does not existed", objectId));

            var item = Convert(resultList.Single());

            return item;
        }

        public IEnumerable<Join> GetVisitsByUser(User user)
        {
            var query = from visit in ParseObject.GetQuery("Join")
                        where visit.Get<string>("Username") == user.Username
                        select visit;

            var result = query.FindAsync().Result;
            var visitResultSet = result.Select(Convert);
            return visitResultSet;
        }

        public IEnumerable<Join> GetMany()
        {
            return null;
        }

        #endregion

        #region Save

        public void Save(Join item)
        {
            var prsVisit = Convert(item);

            prsVisit.SaveAsync().Wait();

            item.UniqueID = prsVisit.ObjectId;
        }

        public void SaveMany(IEnumerable<Join> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(Join item)
        {
            var query = from visit in ParseObject.GetQuery("Join")
                        where visit.Get<string>("Username") == item.Username &&
                      visit.Get<string>("ConcertId") == item.ConcertId
                        select visit;

            var result = query.FindAsync().Result;

            return result.Any();
        }

        #endregion

        #region Convert

        public ParseObject Convert(Join item)
        {
            var concert = new ParseObject("Join");

            concert["Username"] = item.Username.ToCustomLower();
            concert["ConcertId"] = item.ConcertId;
            
            return concert;
        }

        public Join Convert(ParseObject item)
        {
            var visit = new Join
            {
                UniqueID = item.ObjectId,
                Username = item.Get<string>("Username"),
                ConcertId = item.Get<string>("ConcertId")
            };

            return visit;
        }

        #endregion
    }
}
