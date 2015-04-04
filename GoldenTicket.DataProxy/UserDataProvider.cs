using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Logger.Log4Net;
using GoldenTicket.Model;
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
            throw new System.NotImplementedException();
        }

        public void SaveMany(IEnumerable<User> items)
        {
            foreach (var item in items)
            {
                Thread.Sleep(150);
                //LogFactory.Log.Info("Save response #" + item.UniqueID);
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
            var user = new ParseObject("User");

            user["Username"] = item.Username;

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
