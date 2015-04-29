using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using GoldenTicket.Test.Helpers;
using NUnit.Framework;
using Parse;

namespace GoldenTicket.Test.ConcertIntegration
{
    [TestFixture]
    public class ConcertTest
    {
        private List<User> _users;
        private List<Concert> _concerts;
        private List<Recient> _recients;
        private List<Rule> _rules;

        [SetUp]
        public void BeforeRunningTest()
        {
            // Initialize the Parse client with your Application ID and .NET Key found on
            ParseClient.Initialize("or2d5UacADT5r1ioP6p1jBzs1pUbIxyDi5M5atPh", "CaqzPd32zmzl04nlkbgojVbn8apYYvYg2bnNDYBU");

            // Initialize users
            _users = Helper.CreateUsers();
            _concerts = Helper.CreateConcerts();
            _rules = new List<Rule>();
        }

        [Test]
        public void GetSuggestByConcert1()
        {
            var queue = new ConcurrentQueue<UserRecientBlock>();
            _recients = SetRecient1();
            Helper.EnqueueData(queue);
            UserRecientBlock dataBlock;
            queue.TryDequeue(out dataBlock);
            Helper.ExecuteRuleCommand(dataBlock);

            /**
             *  Arrival result
             **
               {concert2}
               {concert1}
               {concert2, concert3}
               {concert2, concert1, concert3}
               {concert5, concert2, concert3}
              
               {concert2} => {concert3} (support: 60%, confidence: 75%)
               {concert3} => {concert2} (support: 60%, confidence: 100%)
             */

            var rule2 = Helper.LoadRule(_concerts[1].UniqueID);
            var rule3 = Helper.LoadRule(_concerts[2].UniqueID);

            _rules.Add(rule2);
            _rules.Add(rule3);

            var concerts1 = Helper.LoadSuggestByConcert(_concerts[1].UniqueID);
            Assert.AreEqual(concerts1.Count, 1, "Loaded wrong values");
            Assert.AreEqual(concerts1.Single().UniqueID, _concerts[2].UniqueID, "Loaded wrong values");

            var concerts2 = Helper.LoadSuggestByConcert(_concerts[2].UniqueID);
            Assert.AreEqual(concerts2.Count, 1, "Loaded wrong values");
            Assert.AreEqual(concerts2.Single().UniqueID, _concerts[1].UniqueID, "Loaded wrong values");
        }
        private List<Recient> SetRecient1()
        {
            var recientDataProvider = DI.Factory.GetInstance<IRecientDataProvider<ParseObject>>();

            var recients = new List<Recient>
            {
                new Recient {Username = _users[0].Username, ConcertId = _concerts[1].UniqueID},
                new Recient {Username = _users[1].Username, ConcertId = _concerts[0].UniqueID},
                new Recient {Username = _users[2].Username, ConcertId = _concerts[1].UniqueID},
                new Recient {Username = _users[2].Username, ConcertId = _concerts[2].UniqueID},
                new Recient {Username = _users[3].Username, ConcertId = _concerts[1].UniqueID},
                new Recient {Username = _users[3].Username, ConcertId = _concerts[0].UniqueID},
                new Recient {Username = _users[3].Username, ConcertId = _concerts[2].UniqueID},
                new Recient {Username = _users[4].Username, ConcertId = _concerts[4].UniqueID},
                new Recient {Username = _users[4].Username, ConcertId = _concerts[1].UniqueID},
                new Recient {Username = _users[4].Username, ConcertId = _concerts[2].UniqueID},
            };
            recientDataProvider.SaveMany(recients);
            return recients;
        }

        [Test]
        public void GetSuggestByConcert2()
        {
            var queue = new ConcurrentQueue<UserRecientBlock>();
            _recients = SetRecient2();
            Helper.EnqueueData(queue);
            UserRecientBlock dataBlock;
            queue.TryDequeue(out dataBlock);
            Helper.ExecuteRuleCommand(dataBlock);

            /**
             *  Arrival result
             **
               {concert3, concert2}
               {concert1}
               {concert3, concert2, concert1}
               {concert1, concert2}
               {concert2}
              
               {concert3} => {concert2} (support: 40%, confidence: 100%)
             */

            var rule3 = Helper.LoadRule(_concerts[2].UniqueID);
            _rules.Add(rule3);

            var concerts1 = Helper.LoadSuggestByConcert(_concerts[2].UniqueID);
            Assert.AreEqual(concerts1.Count, 1, "Loaded wrong values");
            Assert.AreEqual(concerts1.Single().UniqueID, _concerts[1].UniqueID, "Loaded wrong values");
        }
        private List<Recient> SetRecient2()
        {
            var recientDataProvider = DI.Factory.GetInstance<IRecientDataProvider<ParseObject>>();

            var recients = new List<Recient>
            {
                new Recient {Username = _users[0].Username, ConcertId = _concerts[1].UniqueID},
                new Recient {Username = _users[0].Username, ConcertId = _concerts[2].UniqueID},

                new Recient {Username = _users[1].Username, ConcertId = _concerts[0].UniqueID},

                new Recient {Username = _users[2].Username, ConcertId = _concerts[2].UniqueID},
                new Recient {Username = _users[2].Username, ConcertId = _concerts[1].UniqueID},
                new Recient {Username = _users[2].Username, ConcertId = _concerts[0].UniqueID},

                new Recient {Username = _users[3].Username, ConcertId = _concerts[0].UniqueID},
                new Recient {Username = _users[3].Username, ConcertId = _concerts[1].UniqueID},

                new Recient {Username = _users[4].Username, ConcertId = _concerts[1].UniqueID},

            };
            recientDataProvider.SaveMany(recients);

            return recients;
        }

        [Test]
        public void GetSuggestByConcert3()
        {
            var queue = new ConcurrentQueue<UserRecientBlock>();
            _recients = SetRecient3();
            Helper.EnqueueData(queue);
            UserRecientBlock dataBlock;
            queue.TryDequeue(out dataBlock);
            Helper.ExecuteRuleCommand(dataBlock);

            /**
             *  Arrival result
             **
                {concert4, concert5, concert2, concert1}
                {concert2, concert1}
                {concert1}
                {concert2, concert1}
                {concert2, concert1}
              
                {concert1} => {concert2} (support: 80%, confidence: 80%)
                {concert2} => {concert1} (support: 80%, confidence: 100%)
             */


            var rule1 = Helper.LoadRule(_concerts[0].UniqueID);
            var rule2 = Helper.LoadRule(_concerts[1].UniqueID);

            _rules.Add(rule1);
            _rules.Add(rule2);

            var concerts1 = Helper.LoadSuggestByConcert(_concerts[0].UniqueID);
            Assert.AreEqual(concerts1.Count, 1, "Loaded wrong values");
            Assert.AreEqual(concerts1.Single().UniqueID, _concerts[1].UniqueID, "Loaded wrong values");

            var concerts2 = Helper.LoadSuggestByConcert(_concerts[1].UniqueID);
            Assert.AreEqual(concerts2.Count, 1, "Loaded wrong values");
            Assert.AreEqual(concerts2.Single().UniqueID, _concerts[0].UniqueID, "Loaded wrong values");
        }
        private List<Recient> SetRecient3()
        {
            var recientDataProvider = DI.Factory.GetInstance<IRecientDataProvider<ParseObject>>();

            var recients = new List<Recient>
            {
                new Recient {Username = _users[0].Username, ConcertId = _concerts[0].UniqueID},
                new Recient {Username = _users[0].Username, ConcertId = _concerts[1].UniqueID},
                new Recient {Username = _users[0].Username, ConcertId = _concerts[3].UniqueID},
                new Recient {Username = _users[0].Username, ConcertId = _concerts[4].UniqueID},

                new Recient {Username = _users[1].Username, ConcertId = _concerts[0].UniqueID},
                new Recient {Username = _users[1].Username, ConcertId = _concerts[1].UniqueID},

                new Recient {Username = _users[2].Username, ConcertId = _concerts[0].UniqueID},

                new Recient {Username = _users[3].Username, ConcertId = _concerts[0].UniqueID},
                new Recient {Username = _users[3].Username, ConcertId = _concerts[1].UniqueID},

                new Recient {Username = _users[4].Username, ConcertId = _concerts[0].UniqueID},
                new Recient {Username = _users[4].Username, ConcertId = _concerts[1].UniqueID},

            };
            recientDataProvider.SaveMany(recients);

            return recients;
        }

        [Test]
        public void GetSuggestByUser1()
        {
            var queue = new ConcurrentQueue<UserRecientBlock>();
            _recients = SetRecient1();
            Helper.EnqueueData(queue);
            UserRecientBlock dataBlock;
            queue.TryDequeue(out dataBlock);
            Helper.ExecuteRuleCommand(dataBlock);

            Helper.ExecuteSuggestCommand(_users[0]);
            Helper.ExecuteSuggestCommand(_users[2]);

            /**
             *  Arrival result
             **
               {concert2}
               {concert1}
               {concert2, concert3}
               {concert2, concert1, concert3}
               {concert5, concert2, concert3}

               {concert2} => {concert3} (support: 60%, confidence: 75%)
               {concert3} => {concert2} (support: 60%, confidence: 100%)
             */

            var rule2 = Helper.LoadRule(_concerts[1].UniqueID);
            var rule3 = Helper.LoadRule(_concerts[2].UniqueID);

            _rules.Add(rule2);
            _rules.Add(rule3);

            var concerts1 = Helper.LoadSuggestByUser(_users[0].Username);
            Assert.AreEqual(concerts1.Count, 1, "Loaded wrong values");
            Assert.AreEqual(concerts1.Single().UniqueID, _concerts[2].UniqueID, "Loaded wrong values");

            var concerts2 = Helper.LoadSuggestByUser(_users[2].Username);
            Assert.AreEqual(concerts2.Count, 2, "Loaded wrong values");
            Assert.IsTrue(concerts2.Exists(c => c.UniqueID == _concerts[1].UniqueID), "Loaded wrong values");
            Assert.IsTrue(concerts2.Exists(c => c.UniqueID == _concerts[2].UniqueID), "Loaded wrong values");
        }

        [Test]
        public void GetSuggestByUser2()
        {
            var queue = new ConcurrentQueue<UserRecientBlock>();
            _recients = SetRecient2();
            Helper.EnqueueData(queue);
            UserRecientBlock dataBlock;
            queue.TryDequeue(out dataBlock);
            Helper.ExecuteRuleCommand(dataBlock);

            Helper.ExecuteSuggestCommand(_users[0]);
            Helper.ExecuteSuggestCommand(_users[2]);
            /**
             *  Arrival result
             **
               {concert3, concert2}
               {concert1}
               {concert3, concert2, concert1}
               {concert1, concert2}
               {concert2}
              
               {concert3} => {concert2} (support: 40%, confidence: 100%)
             */

            var rule3 = Helper.LoadRule(_concerts[2].UniqueID);
            _rules.Add(rule3);

            var concerts1 = Helper.LoadSuggestByUser(_users[0].Username);
            Assert.AreEqual(concerts1.Count, 1, "Loaded wrong values");
            Assert.AreEqual(concerts1.Single().UniqueID, _concerts[1].UniqueID, "Loaded wrong values");

            var concerts2 = Helper.LoadSuggestByUser(_users[2].Username);
            Assert.AreEqual(concerts2.Count, 1, "Loaded wrong values");
            Assert.AreEqual(concerts2.Single().UniqueID, _concerts[1].UniqueID, "Loaded wrong values");
        }

        [Test]
        public void GetSuggestByUser3()
        {
            var queue = new ConcurrentQueue<UserRecientBlock>();
            _recients = SetRecient3();
            Helper.EnqueueData(queue);
            UserRecientBlock dataBlock;
            queue.TryDequeue(out dataBlock);
            Helper.ExecuteRuleCommand(dataBlock);

            Helper.ExecuteSuggestCommand(_users[0]);
            Helper.ExecuteSuggestCommand(_users[1]);

            /**
             *  Arrival result
             **
                {concert4, concert5, concert2, concert1}
                {concert2, concert1}
                {concert1}
                {concert2, concert1}
                {concert2, concert1}
              
                {concert1} => {concert2} (support: 80%, confidence: 80%)
                {concert2} => {concert1} (support: 80%, confidence: 100%)
             */

            var rule1 = Helper.LoadRule(_concerts[0].UniqueID);
            var rule2 = Helper.LoadRule(_concerts[1].UniqueID);

            _rules.Add(rule1);
            _rules.Add(rule2);

            var concerts1 = Helper.LoadSuggestByConcert(_concerts[0].UniqueID);
            Assert.AreEqual(concerts1.Count, 1, "Loaded wrong values");
            Assert.AreEqual(concerts1.Single().UniqueID, _concerts[1].UniqueID, "Loaded wrong values");

            var concerts2 = Helper.LoadSuggestByUser(_users[1].Username);
            Assert.AreEqual(concerts2.Count, 2, "Loaded wrong values");
            Assert.IsTrue(concerts2.Exists(c => c.UniqueID == _concerts[0].UniqueID), "Loaded wrong values");
            Assert.IsTrue(concerts2.Exists(c => c.UniqueID == _concerts[1].UniqueID), "Loaded wrong values");
        }

        [TearDown]
        public void AfterRunningTest()
        {
            Helper.DeleteUsers(_users);
            Helper.DeleteConcerts(_concerts);
            Helper.DeleteRecients(_recients);
            Helper.DeleteRules(_rules);
            Helper.DeleteSuggests();
        }
    }
}
