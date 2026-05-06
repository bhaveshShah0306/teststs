namespace GoChauffeurWebApi.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }
        public string? UserImage { get; set; }
        public string? Latitude { get; set; }
        public string? Logitude { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? ReferCode { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
    }
}
