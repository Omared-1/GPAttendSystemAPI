using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPAttendSystemAPI.Data.Models
{
    public class H406AttendRecored
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? StudentName { get; set; }
        public TimeSpan? AttendTime { get; set; } 
        public DateOnly? AttendDate { get; set; }
    }
}
