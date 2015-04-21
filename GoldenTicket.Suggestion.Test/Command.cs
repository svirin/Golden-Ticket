using System.Collections.Generic;
using System.Threading;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using Parse;
using GoldenTicket.Suggestion.Apriori;

namespace GoldenTicket.Suggestion.Test
{
    public class Command : ICommand<UserRecientBlock>
    {
        public void ExecuteCommand(UserRecientBlock item)
        {
            var performedCollection = PerformCollection(item);
        }

        private ItemsetCollection PerformCollection(UserRecientBlock item)
        {
            throw new System.NotImplementedException();
        }
    }
}
