namespace SchoolAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public string RoleName { get; set; }
    }
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public string RoleName { get; set; }
    }
    public class UserResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public string RoleName { get; set; }
    }
}
