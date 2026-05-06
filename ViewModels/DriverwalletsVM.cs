namespace GoChauffeurWebApi.ViewModels
{
    public class DriverwalletsVM
    {

        public int DriverwalletId { get; set; }

        public int? DriverId { get; set; }

        public string? WalletBalance { get; set; }

        public int? DriverWalletDeductionResonsId { get; set; }

        public string? DriverWalletDeductionResonsName { get; set; }
    }
}
