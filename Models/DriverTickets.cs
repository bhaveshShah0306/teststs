namespace GoChauffeurWebApi.Models
{
    public class DriverTickets
    {
        public int DriverTicketsId { get; set; }

        public int?  DriverId { get; set; }

        public int? TicketId { get; set; }
        public string? Reasons { get; set; }
        public Boolean? IsTicketClosed { get; set; }
    }
}
