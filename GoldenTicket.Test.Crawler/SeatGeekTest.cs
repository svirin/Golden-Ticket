using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GoldenTicket.Crawler.Test.Crawlers;

namespace GoldenTicket.Test.Crawler
{
    [TestFixture]
    class SeatGeekTest
    {
        [Test]
        public void SeatGeekTesting()
        {
            SeatGeekCrawler crawler = new SeatGeekCrawler();
            IEnumerable<Model.Concert> concerts = crawler.GetConcerts("Robbie Williams", new DateTime(2016, 1,1));
            Assert.NotNull(concerts);
        }
    }
}
