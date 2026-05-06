namespace GoChauffeurWebApi.Models
{
    public class ReferEarning
    {
        public int ReferEarningId { get; set; }
        public int? ReferredEarning { get; set; }
        public int? ReferredByEarning { get; set; }
    }
}
