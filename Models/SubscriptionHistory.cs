namespace GoChauffeurWebApi.Models
{
    public class SubscriptionHistory
    {
        public int Id { get; set; }
        public int? SubId { get; set; }
        public int? UserId { get; set; }
        public DateTime? SubStartDate { get; set; }
        public DateTime? SubEndDate { get; set; }
    }
}
