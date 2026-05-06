namespace GoChauffeurWebApi.ViewModels
{
    public class MonthlyVM
    {

        public int MonthlyId { get; set; }
        public int? TripVarientId { get; set; }
        public string? TripVarientName { get; set; }
        public string? PickUpLocation { get; set; }
        public string? PickupMapURL { get; set; }
        public int? selecteddateListvalue { get; set; }
        public DateTime Accepttimefrom { get; set; }
        public DateTime AccepttimeTo { get; set; }
        public Boolean? IsDriverAssigned { get; set; }
        public string? PickUpLocationCoordinates { get; set; }
        public string? Anonymuscharge { get; set; }
        public int? NoofDays { get; set; }
        public bool IsTimeScheduled { get; set; }
        public Boolean? CloseTrip { get; set; }
        public int? TripStatus { get; set; }
        public DateTime? Pickuptime { get; set; }
        public int? VehicleTypeId { get; set; }
        public string? VehicleTypeName{ get; set; }
        public string? VehicleTypeImage { get; set; }
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
        public int? DriverPrice { get; set; }
        public string driverPhoneNumber { get; set; }
        public DateTime? SelectedDate { get; set; }
        public string? Image { get; set; }
        public int? TransmissionId { get; set; }
        public string? TransmissinName { get; set; }
        public int EstimatedHours { get; set; }
        public Boolean? IsAccepted { get; set; }
        public Decimal? Estimatedprice { get; set; }
        public Decimal? FinalPrice { get; set; }
        public Boolean? IsAdvancedPayment { get; set; }
        public Boolean? IsPermanent { get; set; }
        public string? DriverMeansOfTransport { get; set; }

        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        //public string? PickUPMapURL { get; set; }




        public Boolean IsdriverArrived { get; set; }
        public bool istripcompleted { get; set; }
        public bool IsTripStarted { get; set; }
        public Boolean IsProcessing { get; set; }
        public Boolean IsReserved { get; set; }

        public Boolean IsCancelled { get; set; }

        public string? Taxvalue { get; set; }
        public Boolean? IsDriverArrival { get; set; }
        public Boolean? Isonroute { get; set; }
        public int? UsersTripsCancelResonsId { get; set; }
        public string? UserTripsCancelResonsName { get; set; }
        public string? UniqueMonthlyId { get; set; }
        public Boolean? isalltripscompleted { get; set; }
        public List<MonthlyDateListVM>? DateList { get; set; }
    }

    public class MonthlyDateListVM
    {
        public int MonthlyDateListId { get; set; }

        public DateTime? Date { get; set; }

        public string? Time { get; set; }

        public int? MonthlyId { get; set; }
        public string? ImageUrlsList { get; set; }
        public Boolean? IsTripCompByDriver { get; set; }

        public int? ActualHours { get; set; }
        public Boolean? IsDriverArrival { get; set; }
        public Decimal? ActualPrice { get; set; }
        public Boolean? IsAtDrop { get; set; }
        public Decimal? ActualTaxValue { get; set; }

        public int? CuponId { get; set; }

        public int? selecteddateListvalue { get; set; }
        public Decimal? ActualCuponPrice { get; set; }
        public Boolean? Isonroute { get; set; }
        public Boolean? IsTripStarted { get; set; }
        public Boolean? Istakenpics { get; set; }
        public Boolean? IsEndPicsTaken { get; set; }

    }
}
