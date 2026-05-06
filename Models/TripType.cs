namespace GoChauffeurWebApi.Models
{
    public class TripType
    {
        public int TripTypeId { get; set; }


        public string? TripName { get; set;}

        public string? TripDescription { get; set;}

        public string? Starttime { get; set; }

        public string? Endtime { get; set; }

        public string? Icon { get; set; }
    }
}
