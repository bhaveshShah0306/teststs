namespace GoChauffeurWebApi.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string? AdminName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public int? RolesId { get; set; }
        public string? Password { get; set; }
    }
}
