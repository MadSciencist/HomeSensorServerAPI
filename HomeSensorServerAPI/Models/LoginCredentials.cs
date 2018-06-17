using System.ComponentModel.DataAnnotations;

namespace HomeSensorServerAPI.Models
{
    public class LoginCredentials
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
