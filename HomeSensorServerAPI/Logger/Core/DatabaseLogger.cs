using HomeSensorServerAPI.Repository;

namespace HomeSensorServerAPI.Logger
{
    public class DatabaseLogger : ILogger
    {
        private readonly AppDbContext _context;
        public DatabaseLogger(AppDbContext context)
        {
            _context = context;
        }

        public void LogEvent(LogEvent logEvent)
        {
            _context.Add(logEvent);
            _context.SaveChanges();
        }

        public void ClearAllEvents()
        {

        }
    }
}
