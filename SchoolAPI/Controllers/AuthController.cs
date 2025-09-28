using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using SchoolAPI.Dtos;
using SchoolAPI.Models;
using SchoolAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;


namespace SchoolAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> UserRegistration(UserDto userdto, int roleId)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            bool result = await _repo.UserRegistration(userdto, roleId, ip);
            if (result)
            {
                return Ok(new { Message = "User registered successfully" });
            }
            else
            {
                return BadRequest("Registration Failled");
            }

        }
        [HttpGet("login")]
        public async Task<IActionResult> Login(string email, string passWordHash)
        {
            var user = await _repo.Login(email, passWordHash);
            if (user == null)
                return Unauthorized("Invalid email or password");

            var keyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); 

            var securityKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
             
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

           
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = jwtToken,
                user = new { user }
            });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _repo.GetUsers();
            if (result==null)
            {
                return NotFound("User not found!");
            }
            return Ok (result);
        }
    }
}
