namespace GoChauffeurWebApi.ViewModels
{
    public class HoursVM
    {
        public int HoursId { get; set; }
        public string? HoursName { get; set; }
        public int? HoursinNumber{ get; set; }
        public int? TotalHours { get; set; }
        public int TripTypeId { get; set; }
        public int? TripVarientId { get; set; }
        public int? Charges { get; set; }
        public int? NightCharges { get; set; }
        public string? Starttime { get; set; }
        public string? Endtime { get; set; }
        public Boolean? IsinHours { get; set; }
    }

 
}
