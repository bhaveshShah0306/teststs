namespace GoChauffeurWebApi.Models
{
    public class DriverTicketsReason
    {
        public int DriverTicketsReasonId { get; set; }

        public string? DriverTicketsReasonName { get; set; }
        public int? Type { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Createddate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modifieddate { get; set; }
    }
}
