namespace GoChauffeurWebApi.Models
{
    public class TripVariant
    {
        public int TripVariantId { get; set; }

        public string? TripVariantName { get; set; }

        public int? TriptypeId { get; set; }
        public Decimal? BasePrice { get; set; }

        public Decimal? KilometerLimit { get; set; }

        public Decimal? NightCharges { get; set; }
        public Decimal? ChargesperMinute { get; set; }

        public Decimal? PricePerKilometers { get; set; }

        public Boolean? IsTripOneway { get; set; }


    } 
}
