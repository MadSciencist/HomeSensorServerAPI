using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    //TODO: more validation
    [Table("users")]
    public class User : Entity
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        [Required(ErrorMessage ="Email jest konieczny")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Login jest konieczny")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Hasło jest konieczne")]
        [MinLength(5, ErrorMessage = "Hasło musi miec minimum 5 znaków.")]
        [MaxLength(100, ErrorMessage = "Hasło musi mieć maksymalnie 30 znaków")]
        [StringLength(100)]
        public string Password { get; set; }

        public DateTime Birthdate { get; set; }

        public EUserGender Gender { get; set; }

        [Required(ErrorMessage = "Rola jest konieczna")]
        public EUserRole Role { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime LastValidLogin { get; set; }

        public DateTime LastInvalidLogin { get; set; }
        public DateTime JoinDate { get; set; }

        [NotMapped]
        public bool IsSuccessfullyAuthenticated { get; set; }
    }

    public enum EUserRole : int
    {
        Admin = 0,
        Manager,
        Viewer,
        Sensor
    }

    public enum EUserGender : int
    {
        Male = 0,
        Female
    }
}
