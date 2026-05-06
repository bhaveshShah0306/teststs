using Nest;

namespace GoChauffeurWebApi.Models
{
    public class Flexi
    {

        public int FlexiId { get; set; }

        public string? PickupLocation { get; set; }

        public string? Coordinates { get; set; }

        public int? EstimatedHours { get; set; }

        public DateTime? PickUpTime { get; set; }

        public string? Language { get; set; }
        
        public int? VehicleTypeId { get; set; }
        
        public int? TransmissionId { get; set; }

        //public int? TransmissionTypeId { get; set; }
        public Boolean? IsDriverAssigned { get; set; }

        public Decimal? EstimatedPrice { get; set; }
       
        public Decimal? EstimatedTaxValue { get; set; }

        public Decimal? EstimatedCuponPrice { get; set; }
        public int? UserId { get; set; }

        public int? DriverId { get; set; }


        public Boolean? IsAdvancePaid { get; set; }

        public Decimal? AdvancePaid { get; set; }

        public int? NoofDays { get; set; }


        public Boolean? IsAccepted { get; set; }        

        public DateTime? FlexiSelectedDateTime { get; set; }

        public string? DriverMeansOfTransport { get; set; }


        public String? PickUPMapURL { get; set; }

        public Boolean? CloseTrip { get; set; }

        public Decimal? FinalPrice { get; set; }
        public string? UniqueflexiId { get; set; }
        public int? UsersTripsCancelResonsId { get; set; }
        public Boolean IsdriverArrived { get; set; }
        public bool IsTripCompByDriver { get; set; }
        public bool IsTripStarted { get; set; }
        public Boolean IsProcessing { get; set; }

        public bool IsTimeScheduled { get; set; }
        public Boolean IsReserved { get; set; }

        public Boolean IsCancelled { get; set; }

        public Boolean? Isonroute { get; set; }
        public Boolean? IsPaymentdone { get; set; }
        public int? selecteddateListvalue { get; set; }
    }
}
