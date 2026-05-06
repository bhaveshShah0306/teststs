namespace GoChauffeurWebApi.Models
{
    public class ValetParkingPricing
    {
        public int ValetParkingPricingId { get; set; }

        public Decimal? SupervisorCharges  { get; set; }

        public Decimal? DriverCharges  { get; set; }

        public Decimal? BaseHours { get; set; }

        public Decimal? BasePrice { get; set; }

        public Decimal? HourlyPrice { get; set; }

    }
}
