using HomeSensorServerAPI.Models;
using System;
using System.Linq;

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
                    Password = "admin",
                    Role = UserRole.Admin,
                    Birthdate = new DateTime(1994, 6, 20)
                });

                context.Users.Add(new User()
                {
                    Login = "Manager",
                    Name = "Mateusz",
                    Lastname = "Kryszczak",
                    Email = "mkrysz1337@gmail.com",
                    Password = "admin",
                    Role = UserRole.Manager,
                    Birthdate = new DateTime(1994, 6, 20)
                });

                context.Users.Add(new User()
                {
                    Login = "Viewer",
                    Name = "Mateusz",
                    Lastname = "Kryszczak",
                    Email = "mkrysz1337@gmail.com",
                    Password = "admin",
                    Role = UserRole.Viewer,
                    Birthdate = new DateTime(1994, 6, 20)
                });

                context.Users.Add(new User()
                {
                    Login = "homeAutomationSensor",
                    Name = "Mateusz",
                    Lastname = "Kryszczak",
                    Email = "mkrysz1337@gmail.com",
                    Password = "homeAutomationSensorPassword",
                    Role = UserRole.Sensor,
                    Birthdate = new DateTime(1994, 6, 20)
                });

                context.SaveChanges();
            }
        }
    }
}
