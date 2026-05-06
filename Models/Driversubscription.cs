namespace GoChauffeurWebApi.Models
{
    public class Driversubscription
    {
        public int DriversubscriptionId { get; set; }
        public int? SubscriptionId { get; set; }
        public DateTime? Expirydate{ get; set; }
        public DateTime? CreatedDate { get; set; }
 
        public int? DriverId { get; set; }

        public string? transactionId { get; set; }
        public Decimal? Amount { get; set; }

    }
}
