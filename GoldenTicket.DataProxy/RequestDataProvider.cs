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
    public class RequestDataProvider : IRequestDataProvider<ParseObject>
    {
        #region Get

        public Request Get(string objectId)
        {
            var query = from concert in ParseObject.GetQuery("Request")
                        where concert.Get<string>("objectId") == objectId
                        select concert;

            var task = query.FindAsync();
            var resultSet = task.Result.ToList();

            if (!resultSet.Any())
                throw new DataException(string.Format("Request with id #{0} does not existed", objectId));

            var item = Convert(resultSet.Single());

            return item;
        }


        /// <summary>Select only active requests</summary>
        public IEnumerable<Request> GetActivatedRequests()
        {
            var query = from request in ParseObject.GetQuery("Request")
                        where request.Get<int>("Status") == (int)RequestStatus.NotActivated
                        select request;

            var task = query.FindAsync();
            var resultSet = task.Result;

            var requestsSet = resultSet.Select(Convert);
            return requestsSet;
        }

        #endregion

        #region Save

        public void Save(Request item)
        {
            var prsConcert = Convert(item);

            prsConcert.SaveAsync().Wait();

            item.UniqueID = prsConcert.ObjectId;

            LogFactory.Log.InfoFormat("Request #{0} successfuly saved", item.UniqueID);
        }

        public void ActivateRequest(Request item)
        {
            var prsConcert = ParseObject.CreateWithoutData("Request", item.UniqueID);

            prsConcert["Status"] = (int)RequestStatus.Activated;

            prsConcert.SaveAsync().Wait();

            LogFactory.Log.InfoFormat("User request #{0} successfuly saved", item.UniqueID);
        }

        public void SaveMany(IEnumerable<Request> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(Request identity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Convert

        public ParseObject Convert(Request item)
        {
            var request = new ParseObject("Request");

            request["DateCreated"] = item.DateCreated;
            request["Genre"] = item.Genre.ToCustomLower();
            request["Username"] = item.Username.ToCustomLower();
            request["Artist"] = item.Artist.ToCustomLower();
            request["Country"] = item.Country.ToCustomLower();
            request["City"] = item.City.ToCustomLower();
            request["DateStart"] = item.DateStart;
            request["DateEnd"] = item.DateEnd;
            request["Status"] = (int)item.Status;
            
            return request;
        }

        public Request Convert(ParseObject item)
        {
            var request = new Request
            {
                UniqueID = item.ObjectId,
                DateCreated = item.Get<DateTime>("DateCreated"),
                Genre = item.Get<string>("Genre"),
                Username = item.Get<string>("Username"),
                Artist = item.Get<string>("Artist"),
                Country = item.Get<string>("Country"),
                DateStart = item.Get<DateTime>("DateStart"),
                DateEnd = item.Get<DateTime>("DateEnd"),
                Status = (RequestStatus)item.Get<int>("Status")
                
            };

            return request;
        }

        #endregion
    }
}
