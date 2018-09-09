using HomeSensorServerAPI.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSensorServerAPI.Models
{
    //TODO: more validation
    [Table("users")]
    public class User : Entity, IUser
    {
        [BindNever]
        public override int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        //reduce this due to some MySQL connector issues
        [StringLength(100)]
        public string Password { get; set; }
        public DateTime? Birthdate { get; set; }
        public EUserGender? Gender { get; set; }
        public EUserRole? Role { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime? LastValidLogin { get; set; }
        public DateTime? LastInvalidLogin { get; set; }
        public DateTime? JoinDate { get; set; }

        [NotMapped]
        public bool IsSuccessfullyAuthenticated { get; set; }
    }
}
