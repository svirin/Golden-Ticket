using System;
using System.Collections.Generic;
using GoldenTicket.ConfigurationManager;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.DataProxy.Parse;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.Creator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Initialize the Parse client with your Application ID and .NET Key found on
            ParseClient.Initialize(Config.ApplicationId, Config.DotNetKey);

            //CreateArtists();

            CreateRequests();

            //LoadArtistsIdents();

            //CheckIsExisted();

            //ExecuteStandartSearch();

            //LoadSingleArtistById();

            //ActivateRequest();

            //int period = ConfigurationManager.Config.Settings.QueryAnalizerPeriod;
            //string period = ConfigurationManager.Config.Settings.QueryAnalizerPeriod;
            //string period = ConfigurationManager.Config.Settings.QueryAnalizerPeriod1;

            //SaveNewSettingsItem();

            Console.ReadKey();
        }

        private static void SaveNewSettingsItem()
        {
            var settingsItem = new SettingsItem
                {
                    Name = "QueryAnalizerPeriod",
                    Block = "QueryAnalizer",
                    IsOnline = true,
                    Type = typeof(int).ToString(),
                    Value = "60000"
                };
            

            var settingsProvider = DI.Factory.GetInstance<ISettingsDataProvider<ParseObject>>();
            settingsProvider.Save(settingsItem);
        }

        /// <summary>Imitation user search from mobile phone</summary>
        private static void ExecuteStandartSearch()
        {
            var requestProvider = DI.Factory.GetInstance<IRequestDataProvider<ParseObject>>();
            var searchResultProvider = DI.Factory.GetInstance<ISearchResultDataProvider<ParseObject>>();
            var suggestionProvider = DI.Factory.GetInstance<ISuggestionResultDataProvider<ParseObject>>();

            var user = new User
            {
                Username = "vasiliy"
            };

            var request = new Request
            {
                DateCreated = DateTime.Now,
                Genre = "Genre Ask",
                Artist = "Artist Ask",
                Country = "Country Ask",
                City = "City Ask",
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now,
                Username = user.Username
            };

            // Save request
            requestProvider.Save(request);
            
            // Load search results
            var searchResult = searchResultProvider.SearchResultByUser(user);

            // Load suggestions by user
            var suggestionResult = suggestionProvider.GetSuggestionResultByUser(user);
        }

        private static void CheckIsExisted()
        {
            var requestProvider = DI.Factory.GetInstance<ISearchResultDataProvider<ParseObject>>();

            var item = new SearchResult
            {
                ConcertName = "Gala test",
                Abstract = "Abstract test",
                Genre = "Genre test",
                Artist = "Artist test",
                Region = "Region test",
                Country = "Country test",
                Arena = "Arena test",
                Description = "Description test",
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now,
                CrawlerName = "Crawler Test"
            };

            requestProvider.Save(item);

            if (requestProvider.IsExisted(item))
            {
                Console.WriteLine("Existed");
            }

            item.ConcertName += "!";
            if (!requestProvider.IsExisted(item))
            {
                Console.WriteLine("Not Existed");
            }
        }

        private static void LoadArtistsIdents()
        {
            var requestProvider = DI.Factory.GetInstance<IArtistDataProvider<ParseObject>>();
            var artists = requestProvider.GetAcctualArtists();

            foreach (var artist in artists)
            {
                Console.WriteLine("Artist : ID {0}, Name {1}, Abstract {2}, ImageURL {3}", artist.UniqueID, artist.Name, artist.Abstract, artist.ImageURL);
            }
        }

        private static void CreateArtists()
        {
            var artists = new List<Artist>();

            for (var index = 0; index < 10; index++)
            {
                artists.Add(new Artist
                {
                    Name = "Artist " + index,
                    Abstract = "Abstract " + index,
                    ImageURL = "ImageURL " + index
                });
            }

            var requestProvider = DI.Factory.GetInstance<IArtistDataProvider<ParseObject>>();
            requestProvider.SaveMany(artists);
        }

        private static void CreateRequests()
        {
            var requests = new List<Request>();

            for (var index = 0; index < 100; index++)
            {
                requests.Add(new Request
                {
                    DateCreated = DateTime.Now,
                    Genre = "Genre " + index,
                    Artist = "Artist " + index,
                    Country = "Country " + index,
                    City = "City " + index,
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    IsActivated = false
                });
            }

            var requestProvider = DI.Factory.GetInstance<IRequestDataProvider<ParseObject>>();
            requestProvider.SaveMany(requests);
        }

        private static void ActivateRequest()
        {
            var request = new Request();
            request.UniqueID = "KEaMbzOV3O";
                
            var requestProvider = DI.Factory.GetInstance<IRequestDataProvider<ParseObject>>();

            requestProvider.ActivateRequest(request);
        }
        

        private static void LoadSingleArtistById()
        {
            var requestProvider = DI.Factory.GetInstance<IArtistDataProvider<ParseObject>>();
            var artist = requestProvider.Get("0re9qBR3p6");
            Console.WriteLine("Artist : ID {0}, Name {1}, Abstract {2}, ImageURL {3}", artist.UniqueID, artist.Name, artist.Abstract, artist.ImageURL);
        }
    }
}
