namespace NatLib.Logging;

public static class LoggingUtils
{
    public static bool CheckNullOrWarn(object? value, string sender, string message)
    {
        if (value is not null) return false;
        else
        {
            ConsoleLogger.LogWarning(sender, message);
            return true;
        }
    }
        
    public static bool CheckNullOrError(object? value, string sender, string message)
    {
        if (value is not null) return false;
        else
        {
            ConsoleLogger.LogError(sender, message);
            return true;
        }
    }
        
    public static bool CheckNullOrFatal(object? value, string sender, string message)
    {
        if (value is not null) return false;
        else
        {
            ConsoleLogger.LogFatal(sender, message);
            return true;
        }
    }
}