namespace GoChauffeurWebApi.Models
{
    public class AerialDistancePrice
    {
        public int AerialDistancePriceId { get; set; }

        public string? AerialDistancePriceperKilometer { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
