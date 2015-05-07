using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Json;
using GoldenTicket.Model;

namespace GoldenTicket.Crawler.Test.Crawlers
{
    class SeatGeekCrawler : ICrawler
    {
        private const string URL = "https://api.seatgeek.com/2/events";

        public IEnumerable<Model.Concert> GetConcerts(string artist, DateTime endDate)
        {
            var responses = new List<Concert>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            string urlParameters = "?taxonomies.name=concert&q=" + artist + "&datetime_utc.lte=" + endDate.Year + '-' + endDate.Month + '-' + endDate.Day;

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                string s = response.Content.ReadAsStringAsync().Result;
                JsonValue jsonValue = JsonObject.Parse(s);
                JsonObject fullJson = jsonValue as JsonObject;
                JsonArray events = (JsonArray)fullJson["events"];

                foreach (JsonObject eventJson in events)
                {
                    Concert concert = new Concert();
                    JsonObject venue = (JsonObject)eventJson["venue"];

                    concert.ConcertName = (string)eventJson["title"];
                    // concert.Abstract = (string)eventJson["title"];
                    // concert.Genre = (string)eventJson["title"];
                    concert.Artist = artist;
                    // concert.Region = (string)venue["title"];
                    concert.Country = (string)venue["country"];
                    concert.Arena = (string)venue["name"];
                    concert.Description = (string)eventJson["title"];
                    DateTime date = Convert.ToDateTime((string)eventJson["datetime_local"]);
                    concert.DateStart = date;
                    concert.DateEnd = date;
                    concert.CrawlerName = "SeatGeek";

                    responses.Add(concert);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return responses;

        }
    }
}
