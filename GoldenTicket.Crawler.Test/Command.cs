using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.Crawler.Test
{
    public class Command : ICommand<Artist>
    {
        private ISearchResultDataProvider<ParseObject> _resultDataProvider;

        public void ExecuteCommand(Artist item)
        {
            _resultDataProvider = DI.Factory.GetInstance<ISearchResultDataProvider<ParseObject>>();

            // Execute crawling
            var crawleResults = CrawleByRequest(item);

            var uniqueResults = GetUniqueResults(crawleResults);

            // Save responses
            _resultDataProvider.SaveMany(uniqueResults);
        }

        private IEnumerable<SearchResult> GetUniqueResults(IEnumerable<SearchResult> crawleResults)
        {
            var uniqueResults = crawleResults.Where(crawleResult => !_resultDataProvider.IsExisted(crawleResult));

            return uniqueResults;
        }

        public IEnumerable<SearchResult> CrawleByRequest(Artist item)
        {
            var responses = new List<SearchResult>
            {
                new SearchResult 
                {
                    ConcertName = "Gala #" + item.UniqueID,
                    Abstract = "Gala #" + item.UniqueID,
                    Genre = "Gala #" + item.UniqueID,
                    Artist = "Gala #" + item.UniqueID,
                    Region = "Gala #" + item.UniqueID,
                    Country = "Gala #" + item.UniqueID,
                    Arena = "Gala #" + item.UniqueID,
                    Description = "Gala #" + item.UniqueID,
                    DateStart = DateTime.MinValue,
                    DateEnd = DateTime.MinValue,
                    CrawlerName = "Test"
                }
            };

            Thread.Sleep(1000);
            //LogFactory.Log.InfoFormat("Request item {0}", requestItem.UniqueID);

            return responses;
        }
    }
}
