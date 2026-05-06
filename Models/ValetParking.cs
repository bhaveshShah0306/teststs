namespace GoChauffeurWebApi.Models
{
    public class ValetParking
    {
        public int ValetParkingId { get; set; }

        public string? Venue { get; set; }

        public int? UserID { get; set; }

        public string? LocationCoordinates { get; set; }

        public DateTime? StartDatetime { get; set; }

        public DateTime? EndDatetime { get; set; }

        public int? NumberOfDriversRequired { get; set; }

        public int? NumberOfSupervisors { get; set; }

        public int? NumberOfHours { get; set; }

        public Decimal? GSTPercentage { get; set; }

        public Decimal? GSTPrice { get; set; }

        public Decimal? GrandTotal { get; set; }
        public Decimal? AdvanceAMount { get; set; }
        public Boolean? IsAdvancePaid { get; set; }
        public int? DriverId { get; set; }
      
        public Boolean? IsAccepted { get; set; }

        public string? DriverMeansOfTransport { get; set; }



        public string? UniquevaletParkingId { get; set; }
    }
}
