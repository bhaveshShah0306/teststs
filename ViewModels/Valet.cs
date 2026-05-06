namespace GoChauffeurWebApi.ViewModels
{
    public class ValetVM
    {
        public int ValetParkingId { get; set; }

        public string? Venue { get; set; }

        public int? UserID { get; set; }
        
        public string? UserName { get; set; }
        
        public string? ContactNumber{ get; set; }

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

        public string? DriverMeansOfTransport { get; set; }

        public string? UniquevaletParkingId { get; set; }
        public List<ValetparkingStaffVM>? Staff { get; set; }
    }

    public class ValetparkingStaffVM
    {
        public int ValetparkingStaffId { get; set; }

        public string? DriverId { get; set; }

        public string? SuperVisiorId { get; set; }

        public int? ValetParkingId { get; set; }

        public Boolean? Isregistrationclosed { get; set; }


    }
}
