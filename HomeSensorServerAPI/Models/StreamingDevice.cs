using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [Table("streaming_devices")]
    public class StreamingDevice : Entity
    {
        public string  Name { get; set; }
        public string ConnectionString { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        //TODO authorization for users
        public List<User> AuthorizedUsers { get; set; }
    }
}
