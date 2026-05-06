namespace GoChauffeurWebApi.Models
{
    public class InsurenceTaxandPrice
    {
        public int InsurenceTaxandPriceId { get; set; }

        public Decimal TaxPercentage { get; set; }

        public Decimal Price { get; set; }
    }
}
