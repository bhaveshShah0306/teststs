namespace GoChauffeurWebApi.ViewModels
{
    public class FavouriteVM
    {
        public int FavouriteId { get; set; }
        public string? FavouriteName { get; set; }
        public string? Address { get; set; }
        public string? place_id { get; set; }
        public string? mapUrl { get; set; }

        public string? Coordinates { get; set; }
        public int? UserId { get; set; }

        public string? UserName { get; set; }

        public string? ContactNumber { get; set; }
        public int? FavouriteTypesId { get; set; }

        public string? FavouriteTypeName { get; set; }
    }
}
