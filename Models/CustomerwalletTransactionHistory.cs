namespace GoChauffeurWebApi.Models
{
    public class CustomerwalletTransactionHistory
    {
        public int CustomerwalletTransactionHistoryId { get; set; }
        public int? UserId { get; set; }

        public Decimal? WalletAmount { get; set; }
        public string? Description { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
