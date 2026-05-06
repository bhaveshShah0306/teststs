namespace GoChauffeurWebApi.Models
{
    public class TripTax
    {
        public int TripTaxId { get; set; }

        public Decimal? Percentage { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get;set; }
    }
}
