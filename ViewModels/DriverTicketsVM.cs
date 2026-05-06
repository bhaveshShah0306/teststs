namespace GoChauffeurWebApi.ViewModels
{
    public class DriverTicketsVM
    {
        public int DriverTicketsId { get; set; }

        public int? DriverId { get; set; }
            
        public string? DriverName { get; set; }

        public string? Number { get; set; }
        public int? TicketId { get; set; }
        public string? TicketName { get; set; }
        public string? Reasons { get; set; }
        public Boolean? IsTicketClosed { get; set; }
    }
}
