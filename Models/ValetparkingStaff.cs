namespace GoChauffeurWebApi.Models
{
    public class ValetparkingStaff
    {
        public int ValetparkingStaffId { get; set; }

        public string? DriverId{ get; set; }

        public string? SuperVisiorId { get; set; }

        public int? ValetParkingId { get; set; }

        public Boolean? Isregistrationclosed { get; set; }
    }
}
