using HomeSensorServerAPI.Models;
using Microsoft.Extensions.Logging;

namespace HomeSensorServerAPI.Repository
{
    public class SystemSettingsRepository : GenericRepository<SystemData>, ISystemSettingsRepository
    {
        private readonly ILogger _logger;
        public SystemSettingsRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(UserRepository));
        }
    }
}
