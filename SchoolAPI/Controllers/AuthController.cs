using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Dtos;
using SchoolAPI.Models;
using SchoolAPI.Repositories;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        public AuthController(IUserRepository repo)
        {
            _repo = repo;
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
            if(user==null)
            {
                return Unauthorized("invalid email or password");
            }
            return Ok(user);                                                    
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
