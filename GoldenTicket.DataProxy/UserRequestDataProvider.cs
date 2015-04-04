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
    public class UserRequestDataProvider : IUserRequestDataProvider<ParseObject>
    {
        #region Get

        public UserRequest Get(string objectId)
        {
            var query = from concert in ParseObject.GetQuery("UserRequest")
                        where concert.Get<string>("objectId") == objectId
                        select concert;

            var task = query.FindAsync();
            var resultSet = task.GetAwaiter().GetResult().ToList();

            if (!resultSet.Any())
                throw new DataException(string.Format("UserRequest with id #{0} does not existed", objectId));

            var item = Convert(resultSet.Single());

            return item;
        }


        /// <summary>Select only active requests</summary>
        public IEnumerable<UserRequest> GetActiveRequests()
        {
            var query = from request in ParseObject.GetQuery("UserRequest")
                        where request.Get<Boolean>("IsActive")
                        select request;

            var task = query.FindAsync();
            var resultSet = task.GetAwaiter().GetResult();

            var requestsSet = resultSet.Select(Convert);
            return requestsSet;
        }

        #endregion

        #region Save

        public void Save(UserRequest item)
        {
            var prsConcert = Convert(item);

            prsConcert.SaveAsync().Wait();

            item.UniqueID = prsConcert.ObjectId;

            LogFactory.Log.InfoFormat("User request #{0} successfuly saved", item.UniqueID);
        }

        public void SaveMany(IEnumerable<UserRequest> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(UserRequest identity)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Convert

        public ParseObject Convert(UserRequest item)
        {
            var request = new ParseObject("Request");

            request["DateCreated"] = item.DateCreated;
            request["Genre"] = item.Genre;
            request["Username"] = item.Username;
            request["Artist"] = item.Artist;
            request["Country"] = item.Country;
            request["City"] = item.City;
            request["DateStart"] = item.DateStart;
            request["DateEnd"] = item.DateEnd;
            request["IsNotActive"] = item.IsNotActive;
            
            return request;
        }

        public UserRequest Convert(ParseObject item)
        {
            var userRequest = new UserRequest
            {
                UniqueID = item.ObjectId,
                DateCreated = item.Get<DateTime>("DateCreated"),
                Genre = item.Get<string>("Genre"),
                Username = item.Get<string>("Username"),
                Artist = item.Get<string>("Artist"),
                Country = item.Get<string>("Country"),
                DateStart = item.Get<DateTime>("DateStart"),
                DateEnd = item.Get<DateTime>("DateEnd"),
                IsNotActive = item.Get<Boolean>("IsNotActive")
                
            };

            return userRequest;
        }

        #endregion
    }
}
