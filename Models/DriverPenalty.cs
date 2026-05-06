namespace GoChauffeurWebApi.Models
{
    public class DriverPenalty
    {
        public int DriverPenaltyId { get; set; }

        public int? DriverId { get; set; }

        public string? Reason { get; set; }
        public string? Balance { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
