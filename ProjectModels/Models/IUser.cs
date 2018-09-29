using System;
using HomeSensorServerAPI.Models.Enums;

namespace HomeSensorServerAPI.Models
{
    public interface IUser
    {
        int Id { get; set; }
        DateTime? Birthdate { get; set; }
        string Email { get; set; }
        EUserGender? Gender { get; set; }
        bool IsSuccessfullyAuthenticated { get; set; }
        DateTime? JoinDate { get; set; }
        DateTime? LastInvalidLogin { get; set; }
        string Lastname { get; set; }
        DateTime? LastValidLogin { get; set; }
        string Login { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        string PhotoUrl { get; set; }
        EUserRole? Role { get; set; }
    }
}