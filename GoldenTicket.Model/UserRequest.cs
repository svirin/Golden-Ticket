using System;

namespace GoldenTicket.Model
{
    public class UserRequest
    {
        public string UniqueID { get; set; }
        public DateTime DateCreated { get; set; }
        public string Genre { get; set; }
        public string Artist { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Username { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool IsNotActive { get; set; }
    }
}
