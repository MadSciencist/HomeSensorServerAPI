using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    public class UsersStreamingDevices
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int StreamingDeviceId { get; set; }
        public StreamingDevice StreamingDevice { get; set; } 
    }
}
