using GoldenTicket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenTicket.Crawler.Test.Crawlers
{
    interface ICrawler
    {
        IEnumerable<Concert> GetConcerts(string artist, DateTime endDate);
    }
}
