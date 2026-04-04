namespace NatLib.Logging;

public class ConsoleLogger
{
    public static ConsoleLogger Instance { get; } = LoggerFactory.Create();

    public LoggingConfiguration Config { get; set; } = LoggingConfiguration.Instance;

    public string LoggerName;
    
    internal ConsoleLogger(string loggerName)
    {
        LoggerName = loggerName;
    }

    public void Log(LogLevel level, string message, Exception? exception = null)
    {
        var config = Config;
        if (level < config.MinimumLevel)
            return;

        var configLevel = Config.GetLevelConfig(level);
        var originalFg = Console.ForegroundColor;
        var originalBg = Console.BackgroundColor;

        try
        {
            if (config.ShowTimestamp)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ");
            }

            Console.ForegroundColor = configLevel.Foreground;
            if (configLevel.Background.HasValue)
                Console.BackgroundColor = configLevel.Background.Value;

            Console.Write($" {configLevel.Label,-5} ");
            Console.BackgroundColor = originalBg;

            if (config.ShowSenderName)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($" [{LoggerName}] ");
            }

            Console.ForegroundColor = configLevel.MessageColor;
            Console.WriteLine(message);

            if (exception != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"  └─ {exception.GetType().Name}: {exception.Message}");

                if (exception.StackTrace != null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    foreach (var line in exception.StackTrace.Split('\n'))
                    {
                        Console.WriteLine($"     {line.Trim()}");
                    }
                }
            }
        }
        finally
        {
            Console.ForegroundColor = originalFg;
            Console.BackgroundColor = originalBg;
        }
    }

    public void LogTrace(string message, Exception? exception = null)
        => Log(LogLevel.Trace, message, exception);
    public void LogDebug(string message, Exception? exception = null)
    {
        if (Config.IsDebug)
            Log(LogLevel.Debug, message, exception);
    }
    public void LogInfo(string message, Exception? exception = null)
        => Log(LogLevel.Info, message, exception);
    public void LogWarn(string message, Exception? exception = null)
        => Log(LogLevel.Warn, message, exception);
    public void LogError(string message, Exception? exception = null)
        => Log(LogLevel.Error, message, exception);
    public void LogFatal(string message, Exception? exception = null)
        => Log(LogLevel.Fatal, message, exception);

    public void LogErrorAndThrow(string message, Exception exception)
    {
        LogError(message, exception);
        throw exception;
    }

    public void LogFatalAndThrow(string message, Exception exception)
    {
        LogFatal(message, exception);
        throw exception;
    }
}