
using Nest;

namespace GoChauffeurWebApi.ViewModels
{
    public class FlexisVm
    {

        public int FlexiId { get; set; }

        public string? PickupLocation { get; set; }

        public string? Coordinates { get; set; }
        public DateTime Accepttimefrom { get; set; }
        public DateTime AccepttimeTo { get; set; }
        public int? EstimatedHours { get; set; }

        public bool IsTimeScheduled { get; set; }
        public int? VehicleTypeId { get; set; }

        public int? TransmissionId { get; set; }
        //public int? TransmissionTypeId { get; set; }
        public DateTime? PickUpTime { get; set; }
        public string? Anonymuscharge { get; set; }
        public string? Language { get; set; }

        public Boolean? Isonroute { get; set; }
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
        public string driverPhoneNumber { get; set; }
        public string? Image { get; set; }
        public Boolean? IsDriverAssigned { get; set; }

        public Decimal? EstimatedPrice { get; set; }
        public Decimal? DriverPrice { get; set; }

        public Decimal? EstimatedTaxValue { get; set; }
        public Decimal? FinalPrice { get; set; }
        public Decimal? EstimatedCuponPrice { get; set; }

        public Boolean? IsAdvancePaid { get; set; }

        public Decimal? AdvancePaid { get; set; }

        public int? NoofDays { get; set; }
        public int? TripStatus { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PickUPMapURL { get; set; }
        public Boolean? IsAccepted { get; set; }
        public DateTime? FlexiSelectedDateTime { get; set; }

        public string? VehicleTypeName { get; set; }

        public string? TransmissionName { get; set; }
        public Boolean? CloseTrip { get; set; }


        public int? UsersTripsCancelResonsId { get; set; }

        public string? UserTripsCancelResonsName { get; set; }
        public string? DriverMeansOfTransport { get; set; }

        public string? UniqueflexiId { get; set; }

        public Boolean IsdriverArrived { get; set; }
        public bool istripcompleted { get; set; }
        public bool IsTripStarted { get; set; }
        public Boolean IsProcessing { get; set; }
        public Boolean IsReserved { get; set; }

        public Boolean IsCancelled { get; set; }
        public string? Taxvalue { get; set; }
        public int? selecteddateListvalue { get; set; }
        public Boolean? isalltripscompleted { get; set; }

        public List<FlexiDateListVM> DateList { get; internal set; }
    }
}
