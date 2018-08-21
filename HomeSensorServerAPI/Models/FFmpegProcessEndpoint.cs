using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [NotMapped]
    public class FFmpegProcessEndpoint
    {
        [Required]
        public int StreamingDeviceId { get; set; }
        public string Resolution { get; set; }
    }
}
