namespace GoChauffeurWebApi.Models
{
    public class Chargestypes
    {
        public int ChargestypesId { get; set; }

        public string? ChargesName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public Decimal? Rate { get; set; }
    }
}
