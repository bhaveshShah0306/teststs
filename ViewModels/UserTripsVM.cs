namespace GoChauffeurWebApi.ViewModels
{
	public class UserTripsVM
	{
		public int TripId { get; set; }

		public int? DriverId { get; set; }
		public DateTime? FromTime { get; set; }
		public DateTime? ToTime { get; set; }

		public string? PickupLocation { get; set; }
		public string? TaxIds { get; set; }
		public string? TaxPrice { get; set; }
		public string? DropLocation { get; set; }
		public bool? IsTripStarted { get; set; }
		public string? TripTypeName { get; set; }
		public string? DriverName { get; set; }
		public string? Image { get; set; }
		public int? CuponId { get; set; }
		public string? CuponCode { get; set; }
		public string? CuponName { get; set; }
		public Decimal? Percentage { get; set; }
		public decimal? Price { get; set; }
		public string? Description { get; set; }
		public string? PhoneNumber { get; set; }
		public int? Hours { get; set; }

		public string? VehicleTypeName { get; set; }
		public string? TransmissionTypeName { get; set; }

		public decimal? TotalTripValue { get; set; }


		public string? PaymentType { get; set; }

		public decimal? Distance { get; set; }

		public string? Drivermeansoftransport { get; set; }
		public Boolean? IsOnroute { get; set; }
		public Boolean? IsdriverArrived { get; set; }
		public Boolean? IsProcessing { get; set; }
		public Boolean? IsReserved { get; set; }
		public Boolean? IsAccepted { get; set; }
		public Boolean? IsCancelled { get; set; }
		public Boolean? Istripcompleted { get; set; }
	}

	public class TripByIdVM
	{
		public int TripId { get; set; }
		public int? UserId { get; set; }
		public bool? IsSelfBook { get; set; }
		public Boolean? IsTripStarted { get; set; }
		public Boolean? IsAtDrop { get; set; }
		public string? Image { get; set; }
		public string?  BookedForName { get; set; }
		public decimal? EstimatedPrice { get; set; }

		public int? UsersTripsCancelResonsId { get; set; }
		public string? UserTripsCancelResonsName { get; set; }
		public string? BookedForNumber { get; set; }
		public string? TaxIds { get; set; }
		public DateTime? FromTime { get; set; }
		public DateTime? EndTime { get; set; }
		public DateTime? ToTime { get; set; }

		public string? PickupLocation { get; set; }

		public string? DropLocation { get; set; }

		public string? Hours { get; set; }
		public string? ExpecterHours { get; set; }

		public int? VehicleTypeId { get; set; }

		public string? VehicleTypeName { get; set; }

		public int? TransamissionTypeId { get; set; }

		public string? TransmissionTypeName { get; set; }
		public string? TripTypeName { get; set; }
		public DateTime? TripTime { get; set; }

		public decimal? RideFare { get; set; }

		public int? CouponId { get; set; }

		public Decimal? CouponPercentage { get; set; }

		public decimal? CoupunPrice { get; set; }

		public string? TaxPrice { get; set; }

		public string? InsurencePrice { get; set; }

		public int? DriverId { get; set; }

		public string? DriverName { get; set; }

		public string? SubscriptionName { get; set; }

		public string? DriverContactNumber { get; set; }

		public int? PaymentTypeId { get; set; }

		public string? PaymentTypeName { get; set; }
		public string? FromCoordinates { get; set; }
		public string? ToCoordinates { get; set; }

		public decimal? ExtraTimeUsed { get; set; }
		public decimal? ExtraTimeCharge { get; set; }

		public decimal? ExtraKilometersUsed { get; set; }
		public decimal? ExtraKilometerCost { get; set; }
		public decimal? NightCharges { get; set; }

		public string? TotalTripValue { get; set; }

		public Boolean? Istripcompleted { get; set; }

		public string? PaymentType { get; set; }
		public string? Imageurls { get; set; }
		public decimal? Distance { get; set; }

		public Boolean? IsdriverArrived { get; set; }
		public Boolean? IsProcessing { get; set; }
		public Boolean? IsReserved { get; set; }
		public Boolean? IsAccepted { get; set; }
		public Boolean? IsCancelled { get; set; }


	}
}
