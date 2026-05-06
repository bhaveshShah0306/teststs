namespace GoChauffeurWebApi.Models
{
    public class IgnoredTrips
    {
        public int IgnoredTripsID { get; set; }

        public int? DriverId { get; set; }

        public int? FlexiId{ get; set; }
        public int? ReasonId{ get; set; }
    }
}
