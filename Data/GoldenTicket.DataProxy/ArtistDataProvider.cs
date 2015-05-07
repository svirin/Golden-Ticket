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
    public class ArtistDataProvider : IArtistDataProvider<ParseObject>
    {
        #region Get

        public Artist Get(string objectId)
        {
            var query = from concert in ParseObject.GetQuery("Artist")
                        where concert.Get<string>("objectId") == objectId
                        select concert;

            var resultSet = query.FindAsync().Result.ToList();

            if (!resultSet.Any())
                throw new DataException(string.Format("Artist with id #{0} does not existed", objectId));

            var item = Convert(resultSet.Single());

            return item;
        }

        // Get all artists order by name
        public IEnumerable<Artist> GetAcctualArtists()
        {
            var query = from artist in ParseObject.GetQuery("Artist")
                        orderby artist.Get<string>("Name"), artist.Get<string>("Name")
                        select artist;

            var task = query.FindAsync();
            var resultSet = task.Result;

            var artistsList = resultSet.Select(Convert);
            return artistsList;
        }

        public IEnumerable<Artist> GetMany()
        {
            return GetAcctualArtists();
        }

        #endregion

        #region Save

        public void Save(Artist item)
        {
            var prsConcert = Convert(item);

            prsConcert.SaveAsync().Wait();

            item.UniqueID = prsConcert.ObjectId;

            LogFactory.Log.InfoFormat("Artist #{0} saved successfuly", item.UniqueID);
        }

        public void SaveMany(IEnumerable<Artist> items)
        {
            foreach (var item in items)
            {
                Save(item);
            }
        }

        #endregion

        #region IsExist

        public bool IsExisted(Artist item)
        {
            var query = from artist in ParseObject.GetQuery("Artist")
                        where artist.Get<string>("Name") == item.Name &&
                        artist.Get<string>("Abstract") == item.Abstract &&
                        artist.Get<string>("ImageURL") == item.ImageURL
                        select artist;

            var task = query.FindAsync();
            var resultSet = task.Result;

            return resultSet.Any();
        }

        public bool IsExisted(string name)
        {
            var query = from artist in ParseObject.GetQuery("Artist")
                        where artist.Get<string>("Name") == name
                        select artist;

            var task = query.FindAsync();
            var resultSet = task.Result;

            return resultSet.Any();
        }

        #endregion

        #region Convert

        public ParseObject Convert(Artist item)
        {
            var artist = new ParseObject("Artist");

            artist["Name"] = item.Name.ToCustomLower();
            artist["Abstract"] = item.Abstract.ToCustomLower();
            artist["ImageURL"] = item.ImageURL.ToCustomLower();
           
            return artist;
        }

        public Artist Convert(ParseObject item)
        {
            var artist = new Artist
            {
                UniqueID = item.ObjectId,
                Name = item.Get<string>("Name"),
                Abstract = item.Get<string>("Abstract"),
                ImageURL = item.Get<string>("ImageURL")
            };

            return artist;
        }

        #endregion
    }
}
