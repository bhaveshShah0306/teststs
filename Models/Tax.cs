namespace GoChauffeurWebApi.Models
{
    public class Tax
    {
        public int TaxId { get; set; }
        public string? TaxName { get; set; }
        public Decimal? Percentage { get; set; }
    }
}
