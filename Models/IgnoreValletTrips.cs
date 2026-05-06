namespace GoChauffeurWebApi.Models
{
    public class IgnoreValletTrips
    {

        public int IgnoreValletTripsId { get; set; }

        public int ValetParkingId { get; set; }

        public int? DriverId { get; set; }

        public int? IgnoretripresonsId { get; set; }
    }
}
