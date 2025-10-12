using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Dtos;
using SchoolAPI.Repositories;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _configuration;

        public DashboardController(IUserRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }
        [HttpGet("totalCounts")]
        public async Task<IActionResult> GetDetailsCount()
        {
            var res = await _repo.GetDashboardDetails();
            return Ok(res);


        }
        [HttpGet("attendanceCalendar")]
        public async Task<IActionResult> GetAttendanceCalendar(DateTime startDate, DateTime endDate)
        {
            var data = await _repo.GetAttendance(startDate, endDate); // returns list of records
            return Ok(data);
        }

    }
}
