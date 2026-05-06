namespace GoChauffeurWebApi.Models
{
    public class Cupons
    {
        public int CuponsId { get; set; }
        public string? CuponCode { get; set; }
        public string? CuponName { get; set; }
        public Decimal? Percentage { get; set; }
        public string?  Price { get; set; } 
        public string? Description { get; set; }
        public int? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Flag { get; set; }
    }
}
