namespace SchoolAPI.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public string RoleName { get; set; }
    }
}
