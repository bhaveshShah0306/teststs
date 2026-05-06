namespace GoChauffeurWebApi.ViewModels
{
    public class DriverWalletTransactionHistoriesVM
    {

        public int DriverWalletTransactionHistoryId { get; set; }

        public int? DriverId { get; set; }

        public Decimal? WalletAmount { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }


        public string? description { get; set; }
        public string? transactionId { get; set; }


    }
}
