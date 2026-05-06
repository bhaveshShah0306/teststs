namespace GoChauffeurWebApi.ViewModels
{
    public class DriversubscriptionTransctionVM
    {
        public int DriversubscriptionId { get; set; }
        public int? DriverId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DriverName { get; set; }
        public DateTime? Expirydate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? AfterGsttotalsubamount
        { get; set; }
        public int? SubscriptionId { get; set; }
    
        public string? SubName { get; set; }
        public int? Duration { get; set; }
        public decimal? SubPrice { get; set; }
        public string? SubBenefits { get; set; }
        public string? SubDescription { get; set; }
        public string? Status { get; set; }
        public Decimal? GSTAmount { get; set; }

        public string? transactionId { get; set; }
        public Decimal? Amount { get; set; }
        public bool IsDriverSub { get; set; }
    }

}

