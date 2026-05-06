namespace GoChauffeurWebApi.Models
{
    public class WithdrawRequest
    {
        public int WithdrawRequestId { get; set; }

        public int? DriverId { get; set; }

        public string? withdrawamount { get; set; }

        public Boolean? IsApproved { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
