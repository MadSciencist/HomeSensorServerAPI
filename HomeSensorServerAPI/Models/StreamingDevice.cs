using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [Table("streaming_devices")]
    public class StreamingDevice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }

        [BindNever]
        public User Creator { get; set; }

        public string Description { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
