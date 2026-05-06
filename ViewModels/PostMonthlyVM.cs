namespace GoChauffeurWebApi.ViewModels
{
    public class PostMonthlyVM
    {
        public int MonthlyId { get; set; }

        public int? TripVarientId { get; set; }

        public DateTime? NoofDays { get; set; }

        public int? VehicleTypeId { get; set; }

        public int? TransmissionId { get; set; }

        public int EstimatedHours { get; set; }

        public Decimal? Estimatedprice { get; set; }

        public Boolean? IsAdvancedPayment { get; set; }
    }
}
