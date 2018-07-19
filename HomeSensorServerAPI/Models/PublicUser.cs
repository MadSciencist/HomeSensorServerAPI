using HomeSensorServerAPI.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    [NotMapped]
    public class PublicUser : Entity
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public string Role
        {
            get { return UserRoleE.ToString(); }
            set { UserRoleE = value.ToEnum<EUserRole>(); }
        }
        [NotMapped]
        [JsonIgnore]
        public EUserRole UserRoleE { get; set; }
        public EUserGender Gender { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastValidLogin { get; set; }
        public DateTime LastInvalidLogin { get; set; }
    }
}
