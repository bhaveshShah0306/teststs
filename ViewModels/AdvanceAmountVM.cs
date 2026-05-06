namespace GoChauffeurWebApi.ViewModels
{
    public class AdvanceAmountVM
    {
        public int AdvanceAmountId { get; set; }

        public Decimal? Advance { get; set; }

        public int? TripTypeId { get; set; }
        public string? TripTypeName { get; set; }
    }
}
