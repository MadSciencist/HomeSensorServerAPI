using HomeSensorServerAPI.Repository;
using System;

namespace HomeSensorServerAPI.Logger
{
    public class LogService
    {
        ILogger _logger = null;

        public LogService() {}

        public LogService(ILogger logger)
        {
            _logger = logger;
        }

        public void Notify(LogEvent logEvent)
        {
            if (_logger == null)
                return;

            _logger.LogEvent(logEvent);
        }

        public void Notify(ILogger logger, LogEvent logEvent)
        {
            _logger = logger;
            _logger.LogEvent(logEvent);
        }
    }
}
