namespace GoChauffeurWebApi.Models
{
    public class DriverTracking
    {
        public int DriverTrackingId { get; set; }

        public int? DriverId { get; set; }

        public int? TripId  { get; set; }

        public Decimal? Latitude { get; set; }
        public Decimal? Longitude { get; set; }
    }
}
