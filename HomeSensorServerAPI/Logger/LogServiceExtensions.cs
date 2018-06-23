using HomeSensorServerAPI.Repository;
using System;

namespace HomeSensorServerAPI.Logger
{
    public static class LogServiceExtensions
    {
        public static void LogToConsole(this LogService logService, Exception e)
        {
            string message = "Stack: " + e.StackTrace.ToString() + ", Message: " + e.Message.ToString();
            LogEvent logEvent = new LogEvent
            {
                DateOccured = DateTime.Now,
                LogLevel = ELogLevel.EXCEPTION,
                LogMessage = message
            };

            logService.Notify(new ConsoleLogger(), logEvent);
        }

        public static void LogToConsole(this LogService logService, ELogLevel logLevel, string message)
        {
            LogEvent logEvent = new LogEvent
            {
                DateOccured = DateTime.Now,
                LogLevel = logLevel,
                LogMessage = message
            };

            logService.Notify(new ConsoleLogger(), logEvent);
        }

        public static void LogToDatabase(this LogService logService, AppDbContext context, Exception e)
        {
            string message = "Stack: " + e.StackTrace.ToString() + ", Message: " + e.Message.ToString();
            LogEvent logEvent = new LogEvent
            {
                DateOccured = DateTime.Now,
                LogLevel = ELogLevel.EXCEPTION,
                LogMessage = message
            };

            logService.Notify(new DatabaseLogger(context), logEvent);
        }

        public static void LogToDatabase(this LogService logService, AppDbContext context, ELogLevel logLevel, string message)
        {
            LogEvent logEvent = new LogEvent
            {
                DateOccured = DateTime.Now,
                LogLevel = logLevel,
                LogMessage = message
            };

            logService.Notify(new DatabaseLogger(context), logEvent);
        }
    }
}
