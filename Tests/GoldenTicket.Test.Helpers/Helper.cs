using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using Parse;

namespace GoldenTicket.Test.Helpers
{
    public static class Helper
    {
        public static Rule LoadRule(string concertId)
        {
            var dataProvider = DI.Factory.GetInstance<IRuleDataProvider<ParseObject>>();

            var rule = dataProvider.GetRuleBySource(concertId);

            return rule;
        }
        public static void ExecuteSuggestCommand(UserRecientBlock dataBlock)
        {
            var commandFactory = DI.Factory.GetInstance<ICommandFactory<UserRecientBlock>>();

            var command = commandFactory.CreateCommand();

            command.ExecuteCommand(dataBlock);
        }
        public static void EnqueueData(ConcurrentQueue<UserRecientBlock> queue)
        {
            var dataProvider = DI.Factory.GetInstance<IQueueProvider<UserRecientBlock>>();

            dataProvider.Enqueue(queue);
        }
        public static List<Concert> CreateConcerts()
        {
            var concertProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();
            var list = new List<Concert>
            {
                new Concert
                {
                    ConcertName = "Concert1",
                    Abstract = "Concert1",
                    Genre = "Concert1",
                    Artist = "Artist1",
                    Region = "",
                    Country = "Israel",
                    City = "Tel-Aviv",
                    Arena = "Blumfield stadium",
                    Description = "Concert1",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "",
                    ImageURL = "",
                    NormalPrice = "100.0",
                    DiscountPrice = "90.00"
                },
                new Concert
                {
                    ConcertName = "Concert2",
                    Abstract = "Concert2",
                    Genre = "Concert2",
                    Artist = "Artist2",
                    Region = "",
                    Country = "Israel",
                    City = "Tel-Aviv",
                    Arena = "Blumfield stadium",
                    Description = "Concert2",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "",
                    ImageURL = "",
                    NormalPrice = "100.0",
                    DiscountPrice = "90.00"
                },
                new Concert
                {
                    ConcertName = "Concert3",
                    Abstract = "Concert3",
                    Genre = "Concert3",
                    Artist = "Artist3",
                    Region = "",
                    Country = "Israel",
                    City = "Tel-Aviv",
                    Arena = "Blumfield stadium",
                    Description = "Concert3",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "",
                    ImageURL = "",
                    NormalPrice = "100.0",
                    DiscountPrice = "90.00"
                },
                new Concert
                {
                    ConcertName = "Concert4",
                    Abstract = "Concert4",
                    Genre = "Concert4",
                    Artist = "Artist4",
                    Region = "",
                    Country = "Israel",
                    City = "Tel-Aviv",
                    Arena = "Blumfield stadium",
                    Description = "Concert4",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "",
                    ImageURL = "",
                    NormalPrice = "100.0",
                    DiscountPrice = "90.00"
                },
                new Concert
                {
                    ConcertName = "Concert5",
                    Abstract = "Concert5",
                    Genre = "Concert5",
                    Artist = "Artist5",
                    Region = "",
                    Country = "Israel",
                    City = "Tel-Aviv",
                    Arena = "Blumfield stadium",
                    Description = "Concert5",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "",
                    ImageURL = "",
                    NormalPrice = "100.0",
                    DiscountPrice = "90.00"
                },
                new Concert
                {
                    ConcertName = "Concert6",
                    Abstract = "Concert6",
                    Genre = "Concert6",
                    Artist = "Artist6",
                    Region = "",
                    Country = "Israel",
                    City = "Tel-Aviv",
                    Arena = "Blumfield stadium",
                    Description = "Concert6",
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    CrawlerName = "Crawler Test",
                    CrawlerURL = "",
                    ImageURL = "",
                    NormalPrice = "100.0",
                    DiscountPrice = "90.00"
                },
            };

            concertProvider.SaveMany(list);

            return list;
        }
        public static List<User> CreateUsers()
        {
            var dataProvider = DI.Factory.GetInstance<IUserDataProvider<ParseObject>>();

            var users = new List<User>
            {
                new User {Username = "user1", Password = "1234"},
                new User {Username = "user2", Password = "1234"},
                new User {Username = "user3", Password = "1234"},
                new User {Username = "user4", Password = "1234"},
                new User {Username = "user5", Password = "1234"}
            };

            dataProvider.SaveMany(users);
            return users;
        }
        public static List<Concert> LoadSuggestByConcert(string concertId)
        {
            var dataProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();

            var concerts = dataProvider.GetSuggestToConcerts(concertId).ToList();

            return concerts;
        }
        public static List<Concert> LoadSuggestByUser(string username)
        {
            var dataProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();

            var concerts = dataProvider.GetSuggestToUser(username).ToList();

            return concerts;
        }
        public static void DeleteUsers(List<User> users)
        {
            var dataProvider = DI.Factory.GetInstance<IUserDataProvider<ParseObject>>();
            dataProvider.DeleteMany(users);
        }
        public static void DeleteConcerts(List<Concert> concerts)
        {
            var dataProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();
            dataProvider.DeleteMany(concerts);
        }
        public static void DeleteRecients(List<Recient> recients)
        {
            var dataProvider = DI.Factory.GetInstance<IRecientDataProvider<ParseObject>>();
            dataProvider.DeleteMany(recients);
        }
        public static void DeleteRules(List<Rule> rules)
        {
            var dataProvider = DI.Factory.GetInstance<IRuleDataProvider<ParseObject>>();
            dataProvider.DeleteMany(rules);
        }
    }
}
