namespace GoChauffeurWebApi.ViewModels
{
    public class VehicleVM
    {
        public int VehicleId { get; set; }
        public string? VehicleNo { get; set; }
        public int? VehicleTypeId { get; set; }
        public int? UserId { get; set; }
        public string? VehicleTypeName { get; internal set; }
        public string? UsersName { get; internal set; }
    }
}
