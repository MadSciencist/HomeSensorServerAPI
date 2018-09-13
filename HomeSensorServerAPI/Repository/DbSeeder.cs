using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Dictionaries;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.PasswordCryptography;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HomeSensorServerAPI.Repository
{
    public class DbSeeder
    {
        public void UpdateDatabase(AppDbContext context)
        {
            context.Database.Migrate();
        }

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
                        Key = (int)role,
                        Value = role.ToString()
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
                        Key = (int)gender,
                        Value = gender.ToString()
                    });
                }

                context.SaveChanges();
            }

            if (!context.NodeTypes.Any())
            {
                foreach (ENodeType nodeType in (ENodeType[])Enum.GetValues(typeof(ENodeType)))
                {
                    context.NodeTypes.Add(new NodeType
                    {
                        Key = (int)nodeType,
                        Value = nodeType.ToString()
                    });
                }

                context.SaveChanges();
            }

            if (!context.SensorTypes.Any())
            {
                foreach (ESensorType sensorType in (ESensorType[])Enum.GetValues(typeof(ESensorType)))
                {
                    context.SensorTypes.Add(new SensorType
                    {
                        Key = (int)sensorType,
                        Value = sensorType.ToString()
                    });
                }

                context.SaveChanges();
            }

            if (!context.ActuatorTypes.Any())
            {
                foreach (EActuatorType actuatorType in (EActuatorType[])Enum.GetValues(typeof(EActuatorType)))
                {
                    context.ActuatorTypes.Add(new ActuatorType
                    {
                        Key = (int)actuatorType,
                        Value = actuatorType.ToString()
                    });
                }

                context.SaveChanges();
            }

            if (!context.SystemData.Any())
            {
                context.SystemData.Add(new SystemData
                {
                    RpiUrl = "url",
                    RpiLogin = "login",
                    RpiPassword = "passw",
                    AppVersion = "1.0.0"
                });

                context.SaveChanges();
            }
        }
    }
}
