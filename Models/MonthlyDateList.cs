namespace GoChauffeurWebApi.Models
{
    public class MonthlyDateList   
    {
        public int MonthlyDateListId { get; set; }
       
        public DateTime? Date { get; set; }

        public int? MonthlyId { get; set; }
            
        public Boolean? IsTripCompByDriver { get; set; }

        public int? ActualHours { get; set; }
        public string? ImageUrlsList { get; set; }

        public Decimal? ActualPrice { get; set; }
        public string? DriversPrice { get; set; }
        public Decimal? ActualTaxValue { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? endtime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public int? CuponId { get; set; }

        public Decimal? ActualCuponPrice { get; set; }

        public Boolean? IsTripStarted { get; set; }

        public Boolean? IsDriverArrival { get; set; }

        public Boolean? Istakenpics { get; set; }
        public Boolean? IsEndPicsTaken { get; set; }
        public Boolean? Isonroute { get; set; }
        public Boolean? IsAtDrop { get; set; }
        public Boolean? IsPaymentdone { get; set; }
        public int? selecteddateListvalue { get; set; }
        public string? DriverMeansOfTransport { get; set; }
    }
}
