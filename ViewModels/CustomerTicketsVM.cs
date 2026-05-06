namespace GoChauffeurWebApi.ViewModels
{
    public class CustomerTicketsVM
    {
        public int CustomerTicketsId { get; set; }

        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Number { get; set; }

        public int? TicketId { get; set; }
        public string? TicketName { get; set; }
        public string? Reasons { get; set; }
        public Boolean? IsTicketClosed { get; set; }
    }
}
