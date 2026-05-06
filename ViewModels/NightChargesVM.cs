namespace GoChauffeurWebApi.ViewModels
{
    public class NightChargesVM
    {
        public int NightChargesId { get; set; }

        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public Decimal? Charges { get; set; }
    }
}
