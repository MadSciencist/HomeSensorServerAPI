using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [Table("users")]
    public class User : Entity
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime Birthdate { get; set; }
        public EUserGender Gender { get; set; }
        public EUserRole Role { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime LastValidLogin { get; set; }
        public DateTime LastInvalidLogin { get; set; }
    }

    public enum EUserRole
    {
        Admin,
        Manager,
        Viewer,
        Sensor
    }

    public enum EUserGender
    {
        Male,
        Female
    }
}
