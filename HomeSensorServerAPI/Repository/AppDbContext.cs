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
        public DbSet<SystemData> SystemData { get; set; }

        /* dictionaries */
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserGender> UserGenders { get; set; }
        public DbSet<NodeType> NodeTypes { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }
        public DbSet<ActuatorType> ActuatorTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(n => n.NodesOwner)
                .WithOne(u => u.Owner);

            builder.Entity<User>()
                .HasMany(s => s.StreamingDevicesOwner)
                .WithOne(u => u.Owner);
        }
    }
}
