namespace GoChauffeurWebApi.Models
{
    public class CustomerTicketReasons
    {
        public int CustomerTicketReasonsId { get; set; }

        public string? CustomerTicketReasonsName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Createddate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modifieddate { get; set; }
    }
}
