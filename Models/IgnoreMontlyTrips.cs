namespace GoChauffeurWebApi.Models
{
    public class IgnoreMontlyTrips
    {

        public int IgnoreMontlyTripsId { get; set; }

        public int? MonthlyId { get; set; }

        public int? DriverId { get; set; }

        public int? IgnoretripresonsId { get; set; }
    }
}
