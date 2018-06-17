using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [Table("users")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime Birthdate { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Admin,
        Manager,
        Viewer,
        Sensor
    }
}
