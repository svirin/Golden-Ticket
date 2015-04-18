using System;

namespace GoldenTicket.Model
{
    public  class Concert
    {
        public string UniqueID { get; set; }

        public string ConcertName { get; set; }

        public string Abstract { get; set; }

        public string Genre { get; set; }

        public string Artist { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Arena { get; set; }

        public string Description { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string CrawlerName { get; set; }

        public string CrawlerURL { get; set; }

        public string ImageURL { get; set; }

        public string NormalPrice { get; set; }

        public string DiscountPrice { get; set; }

    }
}
