using System.ComponentModel.DataAnnotations;

namespace GPAttendSystemAPI.Data.DTOs
{
    public class dtoLogin
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }
    }
}
