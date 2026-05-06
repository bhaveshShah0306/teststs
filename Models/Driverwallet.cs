namespace GoChauffeurWebApi.Models
{
    public class Driverwallet
    {
        public int DriverwalletId { get; set; }

        public int? DriverId { get; set; }

        public string? WalletBalance { get; set; }
            
        public int? DriverWalletDeductionResonsId { get; set; }
    }
}
