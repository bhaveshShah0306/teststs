namespace GoChauffeurWebApi.Models
{
    public class Hours
    {
        public int HoursId { get; set; }
        public int? HoursName { get; set; }
        public int TripTypeId { get; set;}
        public int? Charges { get; set; }
        public int? NightCharges { get; set; }
        public Boolean? IsinHours { get; set; }
        public int? TripVarientId { get; set; }
    }
}
