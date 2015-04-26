namespace GoldenTicket.Model
{
    public class Rule
    {
        public string UniqueID { get; set; }

        public string SourceConcertId { get; set; }

        public string TargetConcertIds { get; set; }

        public double Support { get; set; }

        public double Confidence { get; set; }
    }
}
