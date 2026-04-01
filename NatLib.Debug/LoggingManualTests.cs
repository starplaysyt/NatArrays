using NatLib.Logging;

namespace NatLib.Debug;

public static class LoggingManualTests
{
    public static void RunLoggingManualTests()
    {
        ConsoleLogger.Instance.Config.MinimumLevel = LogLevel.Trace;
        
        ConsoleLogger.Instance.Log(LogLevel.Trace, "Hello World!");
        ConsoleLogger.Instance.Log(LogLevel.Debug, "Hello World!");
        ConsoleLogger.Instance.Log(LogLevel.Info, "Hello World!");
        ConsoleLogger.Instance.Log(LogLevel.Warn, "Hello World!");
        ConsoleLogger.Instance.Log(LogLevel.Error, "Hello World!");
        ConsoleLogger.Instance.Log(LogLevel.Fatal, "Hello World!");
    }
}