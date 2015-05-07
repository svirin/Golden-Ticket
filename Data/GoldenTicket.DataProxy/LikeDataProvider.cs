using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Logger.Log4Net;
using GoldenTicket.Model;
using GoldenTicket.Utilities;
using Parse;
using System;

namespace GoldenTicket.DataProxy.Parse
{
    public class LikeDataProvider : ILikeDataProvider<ParseObject>
    {
        #region Get

        public Like Get(string objectId)
        {
            var query = from like in ParseObject.GetQuery("Like")
                        where like.Get<string>("objectId") == objectId
                        select like;

            var result = query.FindAsync().Result;
            var resultList = result as IList<ParseObject> ?? result.ToList();

            if (!resultList.Any())
                throw new DataException(string.Format("Like with id #{0} does not existed", objectId));

            var item = Convert(resultList.Single());

            return item;
        }

        public IEnumerable<Like> GetLikesByUser(User user)
        {
            var query = from like in ParseObject.GetQuery("Like")
                        where like.Get<string>("Username") == user.Username
                        select like;

            var result = query.FindAsync().Result;
            var liketSet = result.Select(Convert);
            return liketSet;
        }

        public IEnumerable<Like> GetMany()
        {
            return null;
        }

        #endregion

        #region Save

        public void Save(Like item)
        {
            var prsLike = Convert(item);
            prsLike.SaveAsync().Wait();
            item.UniqueID = prsLike.ObjectId;
        }

        public void SaveMany(IEnumerable<Like> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(Like item)
        {
            var query = from like in ParseObject.GetQuery("Suggest")
                where like.Get<string>("Username") == item.Username &&
                      like.Get<string>("ConcertId") == item.ConcertId
                select like;

            var result = query.FindAsync().Result;
            return result.Any();
        }

        #endregion

        #region Convert

        public ParseObject Convert(Like item)
        {
            var concert = new ParseObject("Like");

            concert["Username"] = item.Username.ToCustomLower();
            concert["ConcertId"] = item.ConcertId;
            
            return concert;
        }

        public Like Convert(ParseObject item)
        {
            var like = new Like
            {
                UniqueID = item.ObjectId,
                Username = item.Get<string>("Username"),
                ConcertId = item.Get<string>("ConcertId")
            };

            return like;
        }

        #endregion
    }
}
