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
        private ISuggestionResultDataProvider<ParseObject> _suggestDataProvider;

        public void ExecuteCommand(User item)
        {
            _suggestDataProvider = DI.Factory.GetInstance<ISuggestionResultDataProvider<ParseObject>>();

            var suggestResults = ExecuteSuggestionOperation(item);

            var suggestUniqueResults = RemoveDuplicates(suggestResults);

            _suggestDataProvider.SaveMany(suggestUniqueResults);
        }

        private IEnumerable<SuggestionResult> RemoveDuplicates(IEnumerable<SuggestionResult> suggestResults)
        {
            var suggestions = new List<SuggestionResult>();

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

        private IEnumerable<SuggestionResult> ExecuteSuggestionOperation(User item)
        {
            // TODO : Here must be implemented suggesting algorythm
            var suggestions = new List<SuggestionResult>
            {
                new SuggestionResult 
                {
                    Abstract = "Abstract #1",
                    Arena = "Arena1",
                    Artist = "Artist",
                    ConcertName = "ConcertName",
                    Country = "Country",
                    Description = "Description",
                    Genre = "Genre",
                    UserName = "UserName"
                }
            };

            Thread.Sleep(1000);

            return suggestions;
        }
    }
}
