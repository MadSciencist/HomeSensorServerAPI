using HomeSensorServerAPI.Models;
using Microsoft.Extensions.Logging;

namespace HomeSensorServerAPI.Repository
{
    public class StreamingDeviceRepository : GenericRepository<StreamingDevice>, IStreamingDeviceRepository
    {
        private readonly ILogger _logger;

        public StreamingDeviceRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("HomeSensorServerAPI.Repository.StreamingDeviceRepository");
        }
    }
}
