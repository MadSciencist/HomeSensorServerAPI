using HomeSensorServerAPI.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Logger
{
    [Table("logevents")]
    public class LogEvent : Entity
    {
        public DateTime DateOccured { get; set; }
        public ELogLevel LogLevel { get; set; }
        public string LogMessage { get; set; }
    }
}
