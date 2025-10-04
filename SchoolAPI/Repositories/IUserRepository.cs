using SchoolAPI.Dtos;
using SchoolAPI.Models;

namespace SchoolAPI.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserRegistration(UserDto userdto, int roleId, string ipAddress);
        Task<LoginResponse> Login(string email, string passwordHash);
        Task<IEnumerable<UserResponse>> GetUsers();
       
        Task<List<RoleDto>> GetRolesAsync();
        
    }
}
