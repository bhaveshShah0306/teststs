namespace GoChauffeurWebApi.ViewModels
{
    public class DriverTransactionsVM
    {
        public int DriverTransactionId { get; set; }
        public int? DriverId { get; set; }
        public string? transactionId { get; set; }
        public Decimal? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Boolean? IsJoiningFee { get; set; }


        public string? description { get; set; }
    }

    
}
