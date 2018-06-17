using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [Table("sensors")]
    public class Sensor : Entity
    {
        public override long Id { get; set; }
        public DateTime TimeStamp { get; set; }

        public string Identifier { get; set; }
        public string Data { get; set; }
    }
}
