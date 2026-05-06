namespace GoChauffeurWebApi.Models
{
    public class CustomerWallet
    {
        public int CustomerWalletId { get; set; }
        public int? UserId{ get; set; }
        public int? WalletBalance { get; set; }
        public int? CreatedBy{ get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
