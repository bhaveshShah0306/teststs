namespace GoChauffeurWebApi.ViewModels
{
    public class SubscriptionVM
    {
        public int SubscriptionId { get; set; }
        public string? SubName { get; set; }
        public int? Duration { get; set; }
        public decimal? SubPrice { get; set; }
        public string? SubBenefits { get; set; }
        public string? SubDescription { get; set; }
        public string? Status { get; set; }
        public int? Percentage { get; set; }
        public bool IsDriverSub { get; set; }

        public int? SubscripationGstId { get; set; }

        public string? SubscripationName { get; set; }

        public string? GstPercentage { get; set; }

        public decimal? AfterGsttotalsubamount
        { get; set; }
    }
}
