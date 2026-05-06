namespace GoChauffeurWebApi.Models
{
    public class TripCancellationReasons
    {
        public int TripCancellationReasonsId { get; set; }

        public string? TripCancellationReason { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
