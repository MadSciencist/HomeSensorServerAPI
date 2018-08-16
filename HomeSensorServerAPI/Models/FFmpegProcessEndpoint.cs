using System.ComponentModel.DataAnnotations;

namespace HomeSensorServerAPI.Models
{
    public class FFmpegProcessEndpoint
    {
        [Required]
        public string Url { get; set; }
        public string Resolution { get; set; }
    }
}
