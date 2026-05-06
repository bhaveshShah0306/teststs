namespace GoChauffeurWebApi.Models
{
	public class Trip
	{
		public int TripId { get; set; }
		public int? UserId { get; set; }
		public int? DriverId { get; set; }
		public DateTime? StartDateTime { get; set; }
		public DateTime? EndDateTime { get; set; }
		public DateTime? ActualEndTime { get; set; }
		public DateTime? RequstedDateTime { get; set; }
		public string? FromLocationName { get; set; }
		public string? ToLocationName { get; set; }
		public string? FromLocation { get; set; }
		public string? ToLocation { get; set; }
		public Decimal? ExpectedDistance { get; set; }
		public Decimal? DistanceTravelled { get; set; }
		public Decimal? AerialDistance { get; set; }
		public string? PickUPMapURL { get; set; }
		public string? DropUPMapURL { get; set; }
		public Boolean? IsTimeScheduled { get; set; }
		public int? TripTypeId { get; set; }
		public int? TripvarientId { get; set; }
		public int? VehicleTypeId { get; set; }
		public int? TransmissionTypeId { get; set; }
		public int? VehicleId { get; set; }
		public bool? IsSelfBook { get; set; }
		public string? BookedForName { get; set; }
		public string? BookedForNumber { get; set; }
		public bool? IsSecuredTrip { get; set; }
		//public decimal? InsurencePercentage { get; set; }
		public int? NoOfHoursActual { get; set; }
		public int? NoOfHoursSelected { get; set; }
		public int? PaymentStatus { get; set; }
		public decimal EstimatedPrice { get; set; }
		public int? TripStatus { get; set; }
		public string? ImageUrlsList { get; set; }
		public int? PaymentTypeId { get; set; }
		public int? CuponId { get; set; }
		public string? TaxIds { get; set; }
		public decimal? TaxPrice { get; set; }
		public decimal? TotalTripValue { get; set; }
		public decimal? DriversPrice { get; set; }
		public string? TripsUniqueId { get; set; }
		public int? TripRating { get; set; }
		public int? DriverRating { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public Boolean? IsdriverArrived { get; set; }
		public Boolean? IsAtDrop { get; set; }
		public bool? IsTripCompByDriver { get; set; }
		public bool? IsTripStarted { get; set; }
		public Boolean IsProcessing { get; set; }
		public Boolean IsReserved { get; set; }
		public Boolean IsAccepted { get; set; }
		public Boolean? IsCancelled { get; set; }
		public string? DriverMeansOfTransport { get; set; }
		public Boolean? Istakenpics { get; set; }
		public Boolean? IsEndPicsTaken { get; set; }
		public Boolean? IsPaymentdone { get; set; }
		public int? UsersTripsCancelResonsId { get; set; }

		public Boolean? Isonroute { get; set; }
		public int? hoursCharge { get; set; }
		public int? driverReturnCharges { get; set; }
		public int? nightCharge { get; set; }
		//public int? couponAmount { get;set; }
		//public int? secureFee { get; set; }
		public int? taxandfee { get; set; }

		//public decimal? ActualDistance { get; set; }
		//public int? ActualTime { get; set; }
		//public decimal? ExtraTime { get; set; }
		//public decimal? Extradistance { get; set; }
		//public decimal? ExpectedDistance { get; set; }
		//public int? DriverFee { get;set; }

		public decimal? ActualDropLat { get; set; }
		public decimal? ActualDropLon { get; set; }
		public string? ActualDropLocation { get; set; }
		public decimal? ActualDistance { get; set; }
		public decimal? ExtraKilometersUsed { get; set; }
		public decimal? ExtraKilometerCost { get; set; }
		public decimal? ExtraTimeUsed { get; set; }
		public decimal? ExtraTimeCost { get; set; }

		public DateTime? BroadcastedAt { get; set; }

	}
}
