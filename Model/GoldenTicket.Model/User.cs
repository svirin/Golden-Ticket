using System.Collections.Generic;
namespace GoldenTicket.Model
{
    public class User
    {
        public string UniqueID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public List<string> VisitedConcertsIds { get; set; }
    }
}
