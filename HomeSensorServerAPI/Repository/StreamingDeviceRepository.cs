using HomeSensorServerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Repository
{
    public class StreamingDeviceRepository : GenericRepository<StreamingDevice>, IStreamingDeviceRepository
    {
        private readonly ILogger _logger;

        public StreamingDeviceRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(StreamingDeviceRepository));
        }

        public override async Task<StreamingDevice> GetByIdAsync(int id)
        {
            return await _context.StreamingDevices.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
