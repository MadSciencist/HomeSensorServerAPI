using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace HomeSensorServerAPI.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StreamingDevice> StreamingDevices { get; set; }

        /* dictionaries */
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserGender> UserGenders { get; set; }
        public DbSet<NodeType> NodeTypes { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }
        public DbSet<ActuatorType> ActuatorTypes { get; set; }
    }
}
