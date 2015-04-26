using System;

namespace GoldenTicket.Model
{

    public enum RequestStatus
    {
        NotActivated = 0,
        Activated = 1
    }
    public class Request
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
        public RequestStatus Status { get; set; }
    }
}
