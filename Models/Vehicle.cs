namespace GoChauffeurWebApi.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string? VehicleName { get; set; }
        public string? VehicleNo { get; set; }
        public int? VehicleTypeId { get; set; }
        public int? UserId { get; set; }
        public string? Images { get; set; }
    }
}
