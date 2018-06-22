namespace HomeSensorServerAPI.Logger
{
    public interface ILogger
    {
        void LogEvent(LogEvent logEvent);
        void ClearAllEvents();
    }
}
