using System;
using System.Collections.Generic;
using System.Linq;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using Parse;
using GoldenTicket.Crawler.Test.Crawlers;

namespace GoldenTicket.Crawler.Test
{
    public class Command : ICommand<Artist>
    {
        private IConcertDataProvider<ParseObject> _resultDataProvider;
        private readonly List<ICrawler> _crawlers = new List<ICrawler>() { new SeatGeekCrawler() };

        public void ExecuteCommand(Artist item)
        {
            _resultDataProvider = DI.Factory.GetInstance<IConcertDataProvider<ParseObject>>();

            // Execute crawling
            var crawleResults = CrawleByRequest(item);

            var uniqueResults = GetUniqueResults(crawleResults);

            // Save responses
            _resultDataProvider.SaveMany(uniqueResults);
        }

        private IEnumerable<Concert> GetUniqueResults(IEnumerable<Concert> crawleResults)
        {
            var uniqueResults = crawleResults.Where(crawleResult => !_resultDataProvider.IsExisted(crawleResult));

            return uniqueResults;
        }

        public IEnumerable<Concert> CrawleByRequest(Artist item)
        {
            var responses = new List<Concert>(); 
            var endDate = new DateTime();
            endDate.AddMonths(3);

            foreach(ICrawler crawler in _crawlers)
            {
                responses.AddRange(crawler.GetConcerts(item.Name, endDate));
            }

            return responses;
        }
    }
}
