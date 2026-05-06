namespace GoChauffeurWebApi.Models
{
    public class TransmissionType
    {
        public int TransmissionTypeId { get; set; }
        public string? TransmissionName { get; set;}

        public int? VehicleTypeId { get; set; }
        public string? icon { get; set; }
    }
}
