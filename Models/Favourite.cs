namespace GoChauffeurWebApi.Models
{
    public class Favourite
    {
        public int FavouriteId { get; set; }
        public string? FavouriteName { get; set; }
        public string? Address { get; set; }
        public string? place_id { get; set; }
        public string? mapUrl { get; set; }

        public string? Coordinates { get; set; }
        public int? UserId{ get; set; }

        public int? FavouriteTypesId { get; set; }
    }
}
