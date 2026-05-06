namespace GoChauffeurWebApi.ViewModels
{
    public class FlexiPostVM
    {
        public int FlexiId { get; set; }

        public string? PickupLocation { get; set; }
        public string? PickupMapURL { get; set; }
        public string? Coordinates { get; set; }

        public int? EstimatedHours { get; set; }

        public int? VehicleTypeId { get; set; }

        public int? TransmissionId { get; set; }

        public DateTime? PickUpTime { get; set; }

        public string? Language { get; set; }

        public int? UserId { get; set; }

        public int? DriverId { get; set; }

        public Boolean? IsDriverAssigned { get; set; }

        public Decimal? EstimatedPrice { get; set; }

        public Decimal? EstimatedTaxValue { get; set; }

        public Decimal? EstimatedCuponPrice { get; set; }

        public Boolean? IsAdvancePaid { get; set; }

        public Decimal? AdvancePaid { get; set; }

        public int? NoofDays { get; set; }

        public DateTime? FlexiSelectedDate { get; set; }

        public List<FlexiDateListVM> Datelists { get; set; }
    }

    public class FlexiDateListVM
    {
        public int FlexiDatesListId { get; set; }
        public Boolean? IsDriverArrival { get; set; }
        public DateTime? Date { get; set; }
        public string? Time { get; set; }//0:00 AM/PM
        public string? ImageUrlsList { get; set; }

        public int? FlexiId { get; set; }
        public Boolean? Istakenpics { get; set; }
        public Boolean? IsEndPicsTaken { get; set; }
        public Boolean? IsTripCompByDriver { get; set; }

        public int? ActualHours { get; set; }

        public Decimal? ActualPrice { get; set; }

        public Decimal? ActualTaxValue { get; set; }

        public int? CuponId { get; set; }

        public Decimal? ActualCuponPrice { get; set; }
        public int? selecteddateListvalue { get; set; }
        public Boolean? IsTripStarted { get; set; }
        public Boolean? IsAtDrop { get; set; }
        public Boolean? Isonroute { get; set; }
    }
}
