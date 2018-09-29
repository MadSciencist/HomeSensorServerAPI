using System.ComponentModel.DataAnnotations;

namespace HomeSensorServerAPI.Models.BindingModels
{
    public class RpiCredentials
    {
        [Required]
        public string RpiUrl { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
