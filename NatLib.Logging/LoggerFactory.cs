using System.Reflection;

namespace NatLib.Logging;

public static class LoggerFactory
{
    public const string DefaultLoggerName = "default";
    private static Dictionary<string, ConsoleLogger> Loggers { get; } = new();

    public static int LoggersCount => Loggers.Count;

    static LoggerFactory() =>
        Loggers.Add(DefaultLoggerName, new ConsoleLogger(Assembly.GetEntryAssembly()?.GetName().Name ?? DefaultLoggerName));

    public static ConsoleLogger Create() => Loggers[DefaultLoggerName];

    public static ConsoleLogger Create(string name)
    {
        if (Loggers.TryGetValue(name, out var logger))
            return logger;

        var createdLogger = new ConsoleLogger(name);
        Loggers.Add(name, createdLogger);
        return createdLogger;
    }

    public static ConsoleLogger Create(object? obj)
        => obj is null ? Loggers[DefaultLoggerName] : Create(obj.GetType().Name);

    public static ConsoleLogger Create<T>()
        => Create(typeof(T).Name);

    public static ConsoleLogger Create(Type type)
        => Create(type.Name);
}