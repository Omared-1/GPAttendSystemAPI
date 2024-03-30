using ClosedXML.Excel;
using GPAttendSystemAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPAttendSystemAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExcelDownloadFileController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ExcelDownloadFileController(AppDbContext db)
        {
            _db = db;
        }


        [Authorize(Roles = "DataMining, ExpertSystem")]
        [HttpGet("DownloadTodayExcel")]
        public IActionResult DownloadTodayExcel()
        {

            var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            // Querying the database for today's data
            var students = _db.H406AttendRecoreds
                .Where(s => s.AttendDate == today &&
                    ((role == "DataMining" && s.AttendTime >= TimeSpan.FromHours(8) && s.AttendTime <= TimeSpan.FromHours(11.5)) ||
                    (role == "ExpertSystem" && s.AttendTime >= TimeSpan.FromHours(11.5) && s.AttendTime <= TimeSpan.FromHours(14.5))))
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


            // Create a new Excel workbook using ClosedXML
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Attendance");

                // Add headers
                worksheet.Cell(1, 1).Value = "Student Name";
                worksheet.Cell(1, 2).Value = "Attendance Date";
                worksheet.Cell(1, 3).Value = "Attendance Time";

                // Populate data
                for (int i = 0; i < students.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = students[i].StudentName;
                    worksheet.Cell(i + 2, 2).Value = students[i].AttendDate;
                    worksheet.Cell(i + 2, 3).Value = students[i].AttendTime;
                }

                // Save the workbook to a memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    // Return the Excel file as a downloadable attachment
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"AttendanceToday_{role}(H406).xlsx");
                }
            }

        }

        [Authorize(Roles = "DataMining, ExpertSystem")]
        [HttpGet("DownloadMonthExcel")]
        public IActionResult DownloadMonthExcel()
        {

            var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            DateOnly firstDayOfMonth = DateOnly.FromDateTime(DateTime.Today.AddDays(1 - DateTime.Today.Day));
            DateOnly lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var students = _db.H406AttendRecoreds
                .Where(s => s.AttendDate >= firstDayOfMonth && s.AttendDate <= lastDayOfMonth &&
                            ((role == "DataMining" && s.AttendTime >= TimeSpan.FromHours(8) && s.AttendTime <= TimeSpan.FromHours(11.5)) ||
                             (role == "ExpertSystem" && s.AttendTime >= TimeSpan.FromHours(11.5) && s.AttendTime <= TimeSpan.FromHours(14.5))))
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

            // Create a new Excel workbook using ClosedXML
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Attendance");

                // Add headers
                worksheet.Cell(1, 1).Value = "Student Name";
                worksheet.Cell(1, 2).Value = "Attendance Date";
                worksheet.Cell(1, 3).Value = "Attendance Time";

                // Populate data
                for (int i = 0; i < students.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = students[i].StudentName;
                    worksheet.Cell(i + 2, 2).Value = students[i].AttendDate;
                    worksheet.Cell(i + 2, 3).Value = students[i].AttendTime;
                }

                // Save the workbook to a memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    // Return the Excel file as a downloadable attachment
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"AttendanceMonthly_{role}(H406).xlsx");
                }
            }
        }


    }
}
