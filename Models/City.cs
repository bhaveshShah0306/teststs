namespace GoChauffeurWebApi.Models
{
    public class City
    {
        public int CityId { get; set; }

        public string? CityName { get; set; }

        public string? StateName { get; set; }
        public string? Latitude { get; set; }

        public string? Longitude { get; set; }
    }
}
