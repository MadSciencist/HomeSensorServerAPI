using HomeSensorServerAPI.Models;
using System;
using System.Linq;
using HomeSensorServerAPI.BusinessLogic;
using System.Collections.Generic;

namespace HomeSensorServerAPI.Repository
{
    public class DbSeeder
    {
        public void EnsurePopulated(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.Add(new User()
                {
                    Login = "admin",
                    Name = "Mateusz",
                    Lastname = "Kryszczak",
                    Email = "mkrysz1337@gmail.com",
                    Password = new PasswordCryptoSerivce().CreateHashString("admin"),
                    Role = EUserRole.Admin,
                    Gender = EUserGender.Male,
                    PhotoUrl = "url",
                    LastInvalidLogin = DateTime.Now,
                    LastValidLogin = DateTime.Now,
                    Birthdate = new DateTime(1994, 6, 20),
                    JoinDate = DateTime.Now
                });

                context.Users.Add(new User()
                {
                    Login = "Manager",
                    Name = "Mateusz",
                    Lastname = "Kryszczak",
                    Email = "mkrysz1337@gmail.com",
                    Password = new PasswordCryptoSerivce().CreateHashString("admin"),
                    Role = EUserRole.Manager,
                    Gender = EUserGender.Male,
                    PhotoUrl = "url",
                    LastInvalidLogin = DateTime.Now,
                    LastValidLogin = DateTime.Now,
                    Birthdate = new DateTime(1994, 6, 20),
                    JoinDate = DateTime.Now
                });

                context.Users.Add(new User()
                {
                    Login = "Viewer",
                    Name = "Mateusz",
                    Lastname = "Kryszczak",
                    Email = "mkrysz1337@gmail.com",
                    Password = new PasswordCryptoSerivce().CreateHashString("admin"),
                    Role = EUserRole.Viewer,
                    Gender = EUserGender.Male,
                    PhotoUrl = "url",
                    LastInvalidLogin = DateTime.Now,
                    LastValidLogin = DateTime.Now,
                    Birthdate = new DateTime(1994, 6, 20),
                    JoinDate = DateTime.Now
                });

                context.Users.Add(new User()
                {
                    Login = "homeAutomationSensor",
                    Name = "Mateusz",
                    Lastname = "Kryszczak",
                    Email = "mkrysz1337@gmail.com",
                    Password = new PasswordCryptoSerivce().CreateHashString("homeAutomationSensorPassword"),
                    Role = EUserRole.Sensor,
                    Gender = EUserGender.Male,
                    PhotoUrl = "url",
                    LastInvalidLogin = DateTime.Now,
                    LastValidLogin = DateTime.Now,
                    Birthdate = new DateTime(1994, 6, 20),
                    JoinDate = DateTime.Now
                });

                context.SaveChanges();
            }


            if (!context.UserRoles.Any())
            {
                foreach (EUserRole role in (EUserRole[])Enum.GetValues(typeof(EUserRole)))
                {
                    context.UserRoles.Add(new UserRole
                    {
                        Value = (int)role,
                        Dictionary = role.ToString()
                    });
                }

                context.SaveChanges();
            }

            if (!context.UserGenders.Any())
            {
                foreach (EUserGender gender in (EUserGender[])Enum.GetValues(typeof(EUserGender)))
                {
                    context.UserGenders.Add(new UserGender
                    {
                        Value = (int)gender,
                        Dictionary = gender.ToString()
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
