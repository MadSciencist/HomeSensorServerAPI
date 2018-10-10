using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [Table("system_data")]
    public class SystemData : Entity
    {
        public string RpiUrl { get; set; }
        public string RpiLogin { get; set; }
        public string RpiPassword { get; set; }
        public DateTime? RuningFrom { get; set; }
        public TimeSpan? Uptime { get; set; }

        public string AppVersion { get; set; }
    }
}
