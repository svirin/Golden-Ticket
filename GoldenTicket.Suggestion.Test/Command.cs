using System.Collections.Generic;
using System.Threading;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.Suggestion.Test
{
    public class Command : ICommand<User>
    {
        private ISuggestDataProvider<ParseObject> _suggestDataProvider;

        public void ExecuteCommand(User item)
        {
            _suggestDataProvider = DI.Factory.GetInstance<ISuggestDataProvider<ParseObject>>();

            var suggestResults = ExecuteSuggestionOperation(item);

            var suggestUniqueResults = RemoveDuplicates(suggestResults);

            _suggestDataProvider.SaveMany(suggestUniqueResults);
        }

        private IEnumerable<Suggest> RemoveDuplicates(IEnumerable<Suggest> suggestResults)
        {
            var suggestions = new List<Suggest>();

            foreach (var suggestItem in suggestResults)
            { 
                var isExisted = _suggestDataProvider.IsExisted(suggestItem);

                if (!isExisted)
                {
                    suggestions.Add(suggestItem);
                }
            }

            return suggestions;
        }

        private IEnumerable<Suggest> ExecuteSuggestionOperation(User item)
        {
            // TODO : Here must be implemented suggesting algorythm
            var suggestions = new List<Suggest>
            {
                new Suggest 
                {
                    Username = "Unknown",
                    ConcertId = "ssss"
                }
            };

            Thread.Sleep(1000);

            return suggestions;
        }
    }
}
