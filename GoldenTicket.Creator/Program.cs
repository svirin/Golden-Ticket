using System;
using System.Collections.Generic;
using System.Linq;
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

            //CreateRequests();

            //GetRequest();

            //CreateConcert();

            //CreateConcerts();

            //CreateSuggests();

            //CreateJoins();

            //CreateLikes();

            //GetConcertBySuggest();

            //GetConcertByVisits();

            //GetConcertByLikes();

            //LoadArtistsIdents();

            //CheckIsExisted();

            //ExecuteStandartSearch();

            //LoadSingleArtistById();

            //ActivateRequest();

            //int period = ConfigurationManager.Config.Settings.QueryAnalizerPeriod;
            //string period = ConfigurationManager.Config.Settings.QueryAnalizerPeriod;
            //string period = ConfigurationManager.Config.Settings.QueryAnalizerPeriod1;

            //SaveNewSettingsItem();

            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void GetRequest()
        {
            var concertProvider = DI.Factory.GetInstance<IRequestDataProvider<ParseObject>>();

            var result = concertProvider.GetActivatedRequests().ToList();
        }

        private static void CreateLikes()
        {
            var concertProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();

            var concerts = concertProvider.GetAll();

            var list = concerts.Select(concert => new Like
            {
                ConcertId = concert.UniqueID,
                Username = "vasiliy"
            });

            var likeProvider = DI.Factory.GetInstance<ILikeDataProvider<ParseObject>>();
            likeProvider.SaveMany(list);
        }

        private static void CreateJoins()
        {
            var concertProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();

            var concerts = concertProvider.GetAll();

            var list = concerts.Select(concert => new Join
            {
                ConcertId = concert.UniqueID,
                Username = "vasiliy"
            });

            var joinProvider = DI.Factory.GetInstance<IJoinDataProvider<ParseObject>>();
            joinProvider.SaveMany(list);
        }

        private static void GetConcertBySuggest()
        {
            var concertProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();

            var suggestedList = concertProvider.GetSuggestToUser("Unknown");
        }

        private static void GetConcertByVisits()
        {
            var concertProvider = DI.Factory.GetInstance<IJoinDataProvider<ParseObject>>();

            var user = new User
            {
                Username = "vasiliy"
            };

            var result = concertProvider.GetVisitsByUser(user);
        }

        private static void GetConcertByLikes()
        {
            var concertProvider = DI.Factory.GetInstance<ILikeDataProvider<ParseObject>>();

            var user = new User
            {
                Username = "vasiliy"
            };

            var result = concertProvider.GetLikesByUser(user);
        }

        private static void CreateSuggests()
        {
            var concertProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();

            var concerts = concertProvider.GetAll();

            var list = concerts.Select(concert => new Suggest
            {
                ConcertId = concert.UniqueID, 
                Username = "Unknown"

            });

            var suggestProvider = DI.Factory.GetInstance<ISuggestDataProvider<ParseObject>>();
            suggestProvider.SaveMany(list);

        }

        private static void CreateConcerts()
        {
            var concertProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();
            var list = new List<Concert>
            {
                new Concert
                {
                    ConcertName = "Katty Perry Show",
                    Abstract = "Katty Perry Show",
                    Genre = "POP",
                    Artist = "Katty Perry",
                    Region = "Middle East",
                    Country = "Israel",
                    City = "Tel-Aviv",
                    Arena = "Blumfield stadium",
                    Description = "Katty Perry Show",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "http://crawlertest.com/eminem/concerto/x34343434",
                    ImageURL = "http://cdn01.cdn.justjared.com/wp-content/uploads/headlines/2013/09/katy-perry-roar-music-video.jpg",
                    NormalPrice = "500.0",
                    DiscountPrice = "450.00"
                },

                new Concert
                {
                    ConcertName = "Pitbull - Rap in the Hole",
                    Abstract = "Pitbull - Rap in the Hole",
                    Genre = "RAP",
                    Artist = "Pitbull",
                    Region = "Middle East",
                    Country = "Israel",
                    City = "Tel-Aviv",
                    Arena = "Park Hayarkon",
                    Description = "Pitbull - Rap in the Hole",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "http://crawlertest.com/eminem/concerto/x34343434",
                    ImageURL = "http://www.mtv.com/shared/promoimages/bands/p/pitbull/a_z/1201x1600.jpg",
                    NormalPrice = "300.0",
                    DiscountPrice = "250.00"
                },

                new Concert
                {
                    ConcertName = "David Guetta - Guetta Show",
                    Abstract = "Pitbull - Rap in the Hole",
                    Genre = "Trance",
                    Artist = "David Guetta",
                    Region = "Middle East",
                    Country = "Israel",
                    City = "Tel-Aviv",
                    Arena = "Nokia",
                    Description = "David Guetta - Guetta Show",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "http://crawlertest.com/eminem/concerto/x34343434",
                    ImageURL = "http://www.josepvinaixa.com/blog/wp-content/uploads/2013/09/David-Guetta-One-Voice-Promo.png",
                    NormalPrice = "800.0",
                    DiscountPrice = "650.00"
                },

                new Concert
                {
                    ConcertName = "Eminem - Super Music Performance",
                    Abstract = "Eminem - Super Music Performance",
                    Genre = "RAP",
                    Artist = "Eminem",
                    Region = "Middle East",
                    Country = "Israel",
                    City = "Ramat Gan",
                    Arena = "Ramat Gan Stadium",
                    Description = "Eminem - Super Music Performance",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "http://crawlertest.com/eminem/concerto/x34343434",
                    ImageURL = "http://thumbs.hh.ulximg.com/public/img/avatar/500_1346353449_eminem.jpg",
                    NormalPrice = "800.0",
                    DiscountPrice = "650.00"
                },

                new Concert
                {
                    ConcertName = "Tokimonsta - Japanika",
                    Abstract = "Tokimonsta - Japanika",
                    Genre = "POP",
                    Artist = "Tokimonsta",
                    Region = "Middle East",
                    Country = "Israel",
                    City = "Bat-Yam",
                    Arena = "Gagarin Club",
                    Description = "Tokimonsta - Japanika",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "http://crawlertest.com/eminem/concerto/x34343434",
                    ImageURL = "https://a3-images.myspacecdn.com/images03/35/e8a0923f208b4a6ba8ad98bfecd34ec4/300x300.jpg",
                    NormalPrice = "800.0",
                    DiscountPrice = "650.00"
                }
            };

            concertProvider.SaveMany(list);
        }

        private static void CreateConcert()
        {
            var concertProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();
            var item = new Concert
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
                CrawlerName = "Crawler Test",
                CrawlerURL = "http://crawlertest.com/eminem/concerto/x34343434",
                ImageURL = "http://something.com",
                NormalPrice =  "400.50",
                DiscountPrice = "300.29"
            };

            concertProvider.Save(item);
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
            var searchResultProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();
           
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
            var searchResult = searchResultProvider.GetByUserRequest(request);

            // Load suggestions by user
            var suggestionResult = searchResultProvider.GetSuggestToUser(user.Username);
        }

        private static void CheckIsExisted()
        {
            var requestProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();

            var item = new Concert
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
                    Status = RequestStatus.NotActivated,
                    Username = "vasiliy"
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
