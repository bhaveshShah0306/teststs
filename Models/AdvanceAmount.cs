namespace GoChauffeurWebApi.Models
{
    public class AdvanceAmount
    {
        public int AdvanceAmountId { get; set; }

        public Decimal? Advance{ get; set; }

        public int? TripTypeId { get; set; }

    }
}
