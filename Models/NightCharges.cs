namespace GoChauffeurWebApi.Models
{
    public class NightCharges
    {
        public int NightChargesId { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Decimal? Charges { get; set; }
    }
}
