namespace GoChauffeurWebApi.Models
{
    public class IgnoreFlexiTrips
    {
        public int IgnoreFlexiTripsId { get; set; }

        public int? FlexiId { get; set; }

        public int? DriverId { get; set; }

        public int? IgnoretripresonsId { get; set; }
    }
}
