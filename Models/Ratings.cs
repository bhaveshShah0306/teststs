namespace GoChauffeurWebApi.Models
{
    public class Ratings
    {
        public int RatingsId { get; set; }

        public int? TripId { get; set; }
        public int? DriverId { get; set; }

        public int? Rating { get; set; }

        public string? Images { get; set; }

        public string? Description { get; set; }
    }
}
