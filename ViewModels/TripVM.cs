namespace GoChauffeurWebApi.ViewModels
{
    public class TripVM
    {
        public int TripId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string? Image { get; set; }
        public int? DriverId { get; set; }
        public Boolean? Isonroute { get; set; }
        public Boolean IsReserved { get; set; }
        public string? TripsUniqueId { get; set; }
        public Boolean IsProcessing { get; set; }
        public Boolean? IsdriverArrived { get; set; }
        public Boolean IsAccepted { get; set; }
        public Boolean? IsCancelled { get; set; }
        public DateTime Accepttimefrom { get; set; }
        public DateTime AccepttimeTo { get; set; }
        public string? DriverName { get; set; }
        public string? DriverImage { get; set; }
        public string? LicenceNumber { get; set; }
        public int? Experience { get; set; }
        public DateTime? StartDate { get; set; }
        public string? StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public string? EndTime { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string? ActualEndTime { get; set; }
        public string? DriverPrice { get; set; }
        public Double? ActualTotalMinutes { get; set; }
        public Double? TotalMinutes { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string? RequestedTime { get; set; }
        public Decimal? ExpectedDistance { get; set; }
        public string? FromLocationName { get; set; }

        public string? ToLocationName { get; set; }

        public string? FromLocation { get; set; }
        public string? FromLatitude { get; set; }
        public string? FromLongitude { get; set; }
        public string? ToLocation { get; set; }
        public string? ToLatitude { get; set; }
        public Boolean? Istakenpics { get; set; }
        public Boolean? IsEndPicsTaken { get; set; }

        public string? ToLongitude { get; set; }
        public Decimal? DistanceTravelled { get; set; }
        public Decimal? AerialDistance { get; set; }
        public int? TripTypeId { get; set; }
        public string? TripTypeName { get; set; }
        public string? TripTypeStarttime { get; set; }
        public string? TripTypeEndtime { get; set; }
        public int? TripvarientId { get; set; }
        public string? TripvarientName { get; set; }
        public Decimal? BasePrice { get; set; }
        public string? PickUPMapURL { get; set; }
        public string? DropUPMapURL { get; set; }
        public Boolean? IsTimeScheduled { get; set; }
        public Decimal? KilometerLimit { get; set; }
        public string? TripStatusName { get; set; }
        public Decimal? NightCharges { get; set; }
        public Decimal? ChargesperMinute { get; set; }

        public Decimal? PricePerKilometers { get; set; }
        public Boolean? IsTripOneway { get; set; }
        public int? VehicleTypeId { get; set; }
        public string? VehicleTypeName { get; set; }
        public int? TransmissionTypeId { get; set; }
        public string? TransmissionTypeName { get; set; }
        public int? VehicleId { get; set; }
        public string? VehicleName { get; set; }
        public string? VehicleNo { get; set; }
        public List<VehicleImages>? VehicleImages { get; set; }
        public bool? IsSelfBook { get; set; }
        public string? BookedForName { get; set; }
        public string? BookedForNumber { get; set; }
        public bool? IsSecuredTrip { get; set; }
        public decimal? InsurencePercentage { get; set; }
        public decimal? Insurenceprice { get; set; }
        public int? NoOfHoursActual { get; set; }
        public int? NoOfHoursSelected { get; set; }
        public int? PaymentType { get; set; }
        public int? PaymentStatus { get; set; }
        public string? EstimatedPrice { get; set; }
        public int? TripStatus { get; set; }
        public bool? IsTripStarted { get; set; }
        public bool? IsATDrop { get; set; }
        public string? ImageUrlsList { get; set; }
        public List<Imageurls> imageurls { get; set; }
        public bool? IsTripCompByDriver { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? CuponId { get; set; }
        public string? CuponCode { get; set; }
        public string? CuponName { get; set; }
        public Decimal? CuponPercentage { get; set; }
        public string? TaxIds { get; set; }

        public string? TaxName { get; set; }
        public Decimal? Percentage { get; set; }
        public decimal? ExpectedTotalTripvalue { get; set; }
        public string? TotalTripValue { get; set; }
        public string? Anonymuscharge { get; set; }
        public decimal? Aerialpriceperkilometer { get; set; }
        public decimal? triptaxvalue { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public Boolean? IsPaymentdone { get; set; }
        public Decimal? GSTAmount { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public int? hoursCharge { get; set; }
        public int? driverReturnCharges { get; set; }
        public int? nightCharge { get; set; }
        //public int? couponAmount { get; set; }
        //public int? secureFee { get; set; }
        public int? taxandfee { get; set; }
        //public int? DriverFee { get; set; }


    }

    public class Imageurls
    {
        public string Images { get; set; }
    }
    public class VehicleImages
    {
        public string? VechileImage { get; set; }
    }

    public class TotalTripCountsVM
    {
        public int? TotalTrips { get; set; }
        public int? TotalDrivers { get; set; }
        public int? TotalCustomers { get; set; }
        public Decimal? TotalGSTPercentage { get; set; }
        public Decimal? TotalGSTAmount { get; set; }
        public Decimal? TotalCustomerAmount { get; set; }


    }
}
