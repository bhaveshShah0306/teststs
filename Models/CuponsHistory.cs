namespace GoChauffeurWebApi.Models
{
    public class CuponsHistory
    {
        public int CuponsHistoryId { get; set; }

        public int? CouponId { get; set; }

        public int? UserId { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}






