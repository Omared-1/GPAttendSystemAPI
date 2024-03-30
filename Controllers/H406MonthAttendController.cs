using GPAttendSystemAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GPAttendSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class H406MonthAttendController : ControllerBase
    {
        private readonly AppDbContext _db;
        public H406MonthAttendController(AppDbContext db)
        {
            _db = db;
        }
        
        [Authorize(Roles = "DataMining, ExpertSystem")]
        [HttpGet("GetStudents")]
        public IActionResult GetStudents()
        {
            var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            DateOnly firstDayOfMonth = DateOnly.FromDateTime(DateTime.Today.AddDays(1 - DateTime.Today.Day));
            DateOnly lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            TimeSpan startTime, endTime;

            if (role == "DataMining")
            {
                startTime = TimeSpan.FromHours(11.5);
                endTime = TimeSpan.FromHours(14.5);
            }
            else if (role == "ExpertSystem")
            {
                startTime = TimeSpan.FromHours(8);
                endTime = TimeSpan.FromHours(11.5);
            }
            else
            {
                return StatusCode(403, new { error = "Invalid role" });
            }

            var students = _db.H406AttendRecoreds
                .Where(s => s.AttendDate >= firstDayOfMonth && s.AttendDate <= lastDayOfMonth)  // Attendance within the current month
                             
                .Select(s => new
                {
                    
                    s.StudentName,
                    AttendDate = s.AttendDate.HasValue ? s.AttendDate.Value.ToString("yyyy-MM-dd") : null, // Handle null value
                    AttendTime = s.AttendTime.HasValue ? s.AttendTime.Value.ToString(@"hh\:mm") : null // Handle null value
                })
                .ToList();

            if (students.Count == 0)
            {
                return NotFound("No data available for this month. Have a nice day!");
            }
            // Add role-specific message at the top of the results
            string customHallNumber = "404"; // Replace with your desired custom hall number
            string message = role == "DataMining"
                ? $"Subject: Data Mining, Hall: {customHallNumber},Monthly Attendance "
                : $"Subject: Expert System, Hall: {customHallNumber}, Monthly Attendance" ;

            var result = new List<object> { new { Message = message } };
            result.AddRange(students);

            return Ok(result);
        }


    }
}
