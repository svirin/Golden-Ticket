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
    public class UserDataProvider : IUserDataProvider<ParseObject>
    {
        #region Get

        public User Get(string objectId)
        {
            var query = from concert in ParseObject.GetQuery("User")
                        where concert.Get<string>("objectId") == objectId
                        select concert;

            var task = query.FindAsync();
            var resultSet = task.GetAwaiter().GetResult().ToList();

            if (!resultSet.Any())
                throw new DataException(string.Format("User with id #{0} does not existed", objectId));

            var item = Convert(resultSet.Single());

            return item;
        }

        public IEnumerable<User> GetAcctualUsers()
        {
            var query = from user in ParseObject.GetQuery("User")
                        orderby user.Get<string>("Name"), user.Get<string>("Name")
                        select user;

            var task = query.FindAsync();
            var resultSet = task.GetAwaiter().GetResult();

            var usersSet = resultSet.Select(Convert);
            return usersSet;
        }

        #endregion

        #region Save

        public void Save(User item)
        {
            var prsUser = Convert(item);

            prsUser.SaveAsync().Wait();

            item.UniqueID = prsUser.ObjectId;

            LogFactory.Log.InfoFormat("User #{0} successfuly saved", item.UniqueID);
        }

        public void SaveMany(IEnumerable<User> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region Delete

        public void Delete(User item)
        {
            var prsUser = Convert(item);

            prsUser.DeleteAsync().Wait();

            LogFactory.Log.InfoFormat("User #{0} successfuly deleted", item.UniqueID);
        }

        public void DeleteMany(IEnumerable<User> items)
        {
            foreach (var item in items)
            {
                Delete(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(User identity)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Convert

        public ParseObject Convert(User item)
        {
            var user = new ParseObject("User")
            {
                ObjectId = item.UniqueID
            };

            user["Username"] = item.Username.ToCustomLower();

            return user;
        }

        public User Convert(ParseObject item)
        {
            var user = new User
            {
                UniqueID = item.ObjectId,
                Username = item.Get<string>("Username")
            };

            return user;
        }

        #endregion

    }
}
