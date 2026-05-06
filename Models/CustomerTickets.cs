namespace GoChauffeurWebApi.Models
{
    public class CustomerTickets
    {
        public int CustomerTicketsId { get; set; }

        public int? UserId { get; set; }

        public int? TicketId { get; set; }
        public string? Reasons { get; set; }
        public Boolean? IsTicketClosed { get; set; }
    }
}
