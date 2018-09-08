using HomeSensorServerAPI.Models;
using Microsoft.Extensions.Logging;

namespace HomeSensorServerAPI.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ILogger _logger;
        public UserRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(UserRepository));
        }
    }
}
