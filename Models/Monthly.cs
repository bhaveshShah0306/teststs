namespace GoChauffeurWebApi.Models
{
    public class Monthly
    {
        public int MonthlyId { get; set; }

        public int? TripVarientId { get; set; }
        public string? PickUpLocation { get; set; }
        public string? PickupMapURL { get; set; }
        public string? PickUpLocationCoordinates { get; set; }
        public int? NoofDays { get; set; }
        public DateTime? SelectedDate { get; set; }
        public DateTime? Pickuptime { get; set; }

        public int? VehicleTypeId { get; set; }


        public bool IsTimeScheduled { get; set; }
        public Boolean? IsDriverAssigned { get; set; }
        public int? TransmissionId { get; set; }

        public int EstimatedHours { get; set; }

        public Decimal? Estimatedprice { get; set; }

        public Boolean? IsAdvancedPayment { get; set; }

        public Decimal? AdvancePaid { get; set; }
        public Decimal? FinalPrice { get; set; }
        public Boolean? IsPermanent { get; set; }
        public int? DriverId { get; set; }
        public int? UserId { get; set; }
        public Boolean? IsAccepted { get; set; }

        public string? DriverMeansOfTransport { get; set; }
        public Boolean? CloseTrip { get; set; }

        public string? UniqueMonthlyId { get; set; }

        public int? UsersTripsCancelResonsId { get; set; }
        public Boolean IsdriverArrived { get; set; }
        public bool IsTripCompByDriver { get; set; }
        public bool IsTripStarted { get; set; }
        public Boolean IsProcessing { get; set; }
        public Boolean IsReserved { get; set; }

        public Boolean IsCancelled { get; set; }
        public Boolean? IsPaymentdone { get; set; }
        public Boolean? Isonroute { get; set; }
        public int? selecteddateListvalue { get; set; }
    }
}
