using HomeSensorServerAPI.Models;
using System;
using System.Linq;
using HomeSensorServerAPI.BusinessLogic;

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
                    Birthdate = new DateTime(1994, 6, 20)
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
                    Birthdate = new DateTime(1994, 6, 20)
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
                    Birthdate = new DateTime(1994, 6, 20)
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
                    Birthdate = new DateTime(1994, 6, 20)
                });

                context.SaveChanges();
            }
        }
    }
}
