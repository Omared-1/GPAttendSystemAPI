using ClosedXML.Excel;
using GPAttendSystemAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GPAttendSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class H406AttendController : ControllerBase
    {
        private readonly AppDbContext _db;
        public H406AttendController(AppDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "DataMining, ExpertSystem")]
        [HttpGet("TodayAttends")]
        public IActionResult GetTodayAttends()
        {
            var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            // Querying the database for today's data
            var students = _db.H406AttendRecoreds
                .Where(s => s.AttendDate == today &&
                    ((role == "DataMining" && s.AttendTime >= TimeSpan.FromHours(8) && s.AttendTime <= TimeSpan.FromHours(11.3)) ||
                    (role == "ExpertSystem" && s.AttendTime >= TimeSpan.FromHours(11.5) && s.AttendTime <= TimeSpan.FromHours(14.3))))
                .Select(s => new
                {
                    s.StudentName,
                    AttendDate = s.AttendDate.HasValue ? s.AttendDate.Value.ToString("yyyy-MM-dd") : null,
                    AttendTime = s.AttendTime.HasValue ? s.AttendTime.Value.ToString(@"hh\:mm") : null
                })
                .ToList();

            if (students.Count == 0)
            {
                return NotFound("No data available for today. Have a nice day!");
            }

            // Add role-specific message at the top of the results
            string customHallNumber = "406"; // Replace with your desired custom hall number
            string message = role == "DataMining"
                ? $"Subject: Data Mining, Hall: {customHallNumber}"
                : $"Subject: Expert System, Hall: {customHallNumber}";

            var result = new List<object> { new { Message = message } };
            result.AddRange(students);

            return Ok(result);
        }

        [Authorize(Roles = "DataMining, ExpertSystem")]
        [HttpGet("MonthAttends")]
        public IActionResult GetMonthAttends()
        {
            var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            DateOnly firstDayOfMonth = DateOnly.FromDateTime(DateTime.Today.AddDays(1 - DateTime.Today.Day));
            DateOnly lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var students = _db.H406AttendRecoreds
                .Where(s => s.AttendDate >= firstDayOfMonth && s.AttendDate <= lastDayOfMonth &&
                            ((role == "DataMining" && s.AttendTime >= TimeSpan.FromHours(8) && s.AttendTime <= TimeSpan.FromHours(11.3)) ||
                             (role == "ExpertSystem" && s.AttendTime >= TimeSpan.FromHours(11.4) && s.AttendTime <= TimeSpan.FromHours(14))))
                .Select(s => new
                {

                    s.StudentName,
                    AttendDate = s.AttendDate.HasValue ? s.AttendDate.Value.ToString("yyyy-MM-dd") : null,
                    AttendTime = s.AttendTime.HasValue ? s.AttendTime.Value.ToString(@"hh\:mm") : null
                })
                .ToList();

            if (students.Count == 0)
            {
                return NotFound("No data available for this month. Have a nice day!");
            }

            // Add role-specific message at the top of the results
            string customHallNumber = "406"; // Replace with your desired custom hall number
            string message = role == "DataMining"
                ? $"Subject: Data Mining, Hall: {customHallNumber}"
                : $"Subject: Expert System, Hall: {customHallNumber}";

            var result = new List<object> { new { Message = message } };
            result.AddRange(students);

            return Ok(result);
        }
        [Authorize(Roles = "DataMining, ExpertSystem")]
        [HttpGet("SemsterAttends")]

        public IActionResult GetSemsterAttends()
        {
            // Retrieve the role of the currently authenticated user.
            var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            // Calculate the date range for the last 4 months.
            DateOnly firstDayOfPeriod = DateOnly.FromDateTime(DateTime.Today.AddMonths(-4));
            DateOnly lastDayOfPeriod = DateOnly.FromDateTime(DateTime.Today);

            // Query the database to get the students who attended within the last 4 months and within the specified time range based on the user's role.
            var students = _db.H406AttendRecoreds
                .Where(s => s.AttendDate >= firstDayOfPeriod && s.AttendDate <= lastDayOfPeriod &&
                            ((role == "DataMining" && s.AttendTime >= TimeSpan.FromHours(8) && s.AttendTime <= TimeSpan.FromHours(11.3)) ||
                             (role == "ExpertSystem" && s.AttendTime >= TimeSpan.FromHours(11.4) && s.AttendTime <= TimeSpan.FromHours(14))))
                .Select(s => new
                {
                    s.StudentName,
                    AttendDate = s.AttendDate.HasValue ? s.AttendDate.Value.ToString("yyyy-MM-dd") : null,
                    AttendTime = s.AttendTime.HasValue ? s.AttendTime.Value.ToString(@"hh\:mm") : null
                })
                .ToList();

            // If there are no students found, return a NotFound result with a friendly message.
            if (students.Count == 0)
            {
                return NotFound("No data available for the last 4 months. Have a nice day!");
            }

            // Add role-specific message at the top of the results
            string customHallNumber = "406"; //  custom hall number
            string message = role == "DataMining"
                ? $"Subject: Data Mining, Hall: {customHallNumber}"
                : $"Subject: Expert System, Hall: {customHallNumber}";


            var result = new List<object> { new { Message = message } };
            result.AddRange(students);


            return Ok(result);
        }



    }
}
