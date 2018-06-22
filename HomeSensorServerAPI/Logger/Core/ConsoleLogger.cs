using System;

namespace HomeSensorServerAPI.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void LogEvent(LogEvent logEvent)
        {
            if(logEvent.LogLevel == ELogLevel.EXCEPTION)
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(logEvent.DateOccured.ToString() + " " + logEvent.LogMessage);

            Console.ResetColor();
        }
        public void ClearAllEvents()
        {

        }
    }
}
