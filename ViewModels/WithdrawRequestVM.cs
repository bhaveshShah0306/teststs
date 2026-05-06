namespace GoChauffeurWebApi.ViewModels
{
    public class WithdrawRequestVM
    {
        public int WithdrawRequestId { get; set; }

        public int? DriverId { get; set; }

        public string? PhoneNumber { get; set; }
        public string? DriverName { get; set; }
        public string? withdrawamount { get; set; }

        public string? WalletBalance { get; set; }
        public Boolean? IsApproved { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
