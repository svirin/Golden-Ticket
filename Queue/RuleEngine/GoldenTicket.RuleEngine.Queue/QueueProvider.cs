using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using GoldenTicket.Utilities;
using Parse;

namespace GoldenTicket.RuleEngine.Queue
{
    public class QueueProvider : IQueueProvider<UserRecientBlock>
    {
        public void Enqueue(ConcurrentQueue<UserRecientBlock> queue)
        {
            var recientItems = LoadRecients();

            var userRecientBlock = LoadPivotedRecients(recientItems);

            queue.Enqueue(userRecientBlock);
        }

        private IEnumerable<Recient> LoadRecients() { 
        
            var dataProvider = DI.Factory.GetInstance<IRecientDataProvider<ParseObject>>();

            var recients = dataProvider.GetRecientItems().ToList();

            return recients;
        }

        private UserRecientBlock LoadPivotedRecients(IEnumerable<Recient> recientItems) 
        {
            var userRecientBlock = new UserRecientBlock { VisitedUsers = new List<User>() };

            var recientItemsList = recientItems as IList<Recient> ?? recientItems.ToList();

            var distinctUsers = recientItemsList.DistinctBy(recient => recient.Username).Select(recient => recient.Username).ToList();

            foreach (var username in distinctUsers)
            {
                userRecientBlock.VisitedUsers.Add(
                    new User
                    {
                        Username = username,
                        VisitedConcertsIds = recientItemsList.Where(item => item.Username == username)
                                                     .Select(item => item.ConcertId).ToList()
                    });
            }

            return userRecientBlock;
        }
    }
}
