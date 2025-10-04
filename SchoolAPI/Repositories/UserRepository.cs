using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SchoolAPI.Dtos;
using SchoolAPI.Models;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace SchoolAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ERPConn");
        }
        public async Task<bool> UserRegistration(UserDto userdto, int roleId, string ipAddress)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", userdto.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", userdto.PasswordHash);
                    cmd.Parameters.AddWithValue("@FullName", userdto.FullName);
                    cmd.Parameters.AddWithValue("@Phone", userdto.Phone);
                    cmd.Parameters.AddWithValue("@RoleId", roleId);
                    cmd.Parameters.AddWithValue("@IpAddress", ipAddress);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
            }
        }
        public async Task<LoginResponse> Login(string email, string passWordHash)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_LoginUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PassWordHash", passWordHash);

                    await con.OpenAsync();

                    using (SqlDataReader rd = await cmd.ExecuteReaderAsync())
                    {
                        if (await rd.ReadAsync())
                        {
                            return new LoginResponse
                            {
                                UserId = rd.GetInt32(0),
                                FullName = rd.GetString(1),
                                Phone = rd.GetString(2),
                                Status = rd.GetBoolean(3),
                                RoleName = rd.GetString(4)
                            };
                        }
                        return null;
                    }
                }
            }
        }
        public async Task<IEnumerable<UserResponse>> GetUsers()
        {
            var users = new List<UserResponse>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetUsers", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new UserResponse
                        {
                            UserId = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            FullName = reader.GetString(2),
                            Phone = reader.GetString(3),
                            Status = reader.GetBoolean(4),
                            RoleName = reader.GetString(5)
                        });
                    }
                }
            }
            return users;
        }

      
        public async Task<List<RoleDto>> GetRolesAsync()
        {
            var roles = new List<RoleDto>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllRoles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            roles.Add(new RoleDto
                            {
                                RoleId = reader.GetInt32(0),   // Column 0 -> RoleId
                                RoleName = reader.GetString(1) // Column 1 -> RoleName
                            });
                        }
                    }
                }
            }

            return roles;

        }
    }
}