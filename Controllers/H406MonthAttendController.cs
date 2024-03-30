using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using GPAttendSystemAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing.Text;

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

            var students = _db.H406AttendRecoreds
                .Where(s => s.AttendDate >= firstDayOfMonth && s.AttendDate <= lastDayOfMonth &&
                            ((role == "DataMining" && s.AttendTime >= TimeSpan.FromHours(8) && s.AttendTime <= TimeSpan.FromHours(11)) ||
                             (role == "ExpertSystem" && s.AttendTime >= TimeSpan.FromHours(11.5) && s.AttendTime <= TimeSpan.FromHours(14))))
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
            string customHallNumber = "404"; // Replace with your desired custom hall number
            string message = role == "DataMining"
                ? $"Subject: Data Mining, Hall: {customHallNumber}"
                : $"Subject: Expert System, Hall: {customHallNumber}";

            var result = new List<object> { new { Message = message } };
            result.AddRange(students);

            return Ok(result);
        }

    }
}

