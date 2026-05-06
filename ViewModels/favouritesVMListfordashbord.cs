namespace GoChauffeurWebApi.ViewModels
{
    public class favouritesVMListfordashbord
    {
        public int FavouriteId { get; set; }
        public string? FavouriteName { get; set; }
        public string? formatted_address { get; set; }
        public string? place_id { get; set; }

        public Details? details { get; set; }
        public string? mapUrl { get; set; }
        public string? description { get; set; }
    }

    public class Details
    {
        public string? lat { get; set; }
        public string? lng { get; set; }
    }
}
