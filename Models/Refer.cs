namespace GoChauffeurWebApi.Models
{
    public class Refer
    {
        public int ReferId { get; set; }

        public string? ReferalCode { get; set; }

        public int? ReferedBy { get; set; }
        public int? ReferredTo { get; set; }
        public Boolean? Isused { get; set; }
    }
}
