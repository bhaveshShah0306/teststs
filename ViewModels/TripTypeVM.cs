namespace GoChauffeurWebApi.ViewModels
{
    public class TripTypeVM
    {
        public int TripTypeId { get; set; }


        public string? TripName { get; set; }

        public string? TripDescription { get; set; }
        public string? Starttime { get; set; }

        public string? Endtime { get; set; }
        public string? Icon { get; set; }

        public List<TripVarientListVM>? Tripvarients{ get; set; }
    }

    
    public class TripVarientListVM
    {
        public int TripVariantId { get; set; }

        public string? TripVariantName { get; set; }

        public int? TriptypeId { get; set; }

        public string? TripTypeName { get; set; }

        public Decimal? BasePrice { get; set; }

        public Decimal? KilometerLimit { get; set; }

        public Decimal? NightCharges { get; set; }
        public Decimal? ChargesperMinute { get; set; }

        public Decimal? PricePerKilometers { get; set; }

        public Boolean? IsTripOneway { get; set; }
    }
}
