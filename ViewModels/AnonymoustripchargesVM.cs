namespace GoChauffeurWebApi.ViewModels
{
    public class AnonymoustripchargesVM
    {
        public int AnonymoustripchargesId { get; set; }

        public int? TriptypeId { get; set; }
        public string? TriptypName{ get; set; }

        public int? Amount { get; set; }
    }
}
