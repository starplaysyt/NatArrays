namespace NatLib.Logging;

public static class ConsoleLogger
{
    /// <summary>
    /// LogInfo logs message and do nothing.
    /// </summary>
    /// <param name="sender">Sender is a part of the message, but highlighted separately</param>
    /// <param name="message">Message is a simple message string</param>
    public static void LogInfo(string sender, string message) =>
        Log("INFO",
            LoggingConfiguration.InfoLabelColor,
            LoggingConfiguration.InfoSenderColor,
            LoggingConfiguration.InfoMessageColor,
            sender,
            message,
            true,
            false);

    /// <summary>
    /// LogDebug logs message and do nothing.
    /// </summary>
    /// <param name="sender">Sender is a part of the message, but highlighted separately</param>
    /// <param name="message">Message is a simple message string</param>
    public static void LogDone(string sender, string message) =>
        Log("DONE",
            LoggingConfiguration.DoneLabelColor,
            LoggingConfiguration.DoneSenderColor,
            LoggingConfiguration.DoneMessageColor,
            sender,
            message,
            true,
            false);

    /// <summary>
    /// LogDebug logs message when static variable IsDebug is true.
    /// </summary>
    /// <param name="sender">Sender is a part of the message, but highlighted separately</param>
    /// <param name="message">Message is a simple message string</param>
    public static void LogDebug(string sender, string message) =>
        Log("DEBUG",
            LoggingConfiguration.DebugLabelColor,
            LoggingConfiguration.DebugSenderColor,
            LoggingConfiguration.DebugMessageColor,
            sender,
            message,
            true,
            true);

    /// <summary>
    /// LogWarning logs message and do nothing.
    /// </summary>
    /// <param name="sender">Sender is a part of the message, but highlighted separately</param>
    /// <param name="message">Message is a simple message string</param>
    public static void LogWarning(string sender, string message) =>
        Log("WARN",
            LoggingConfiguration.WarningLabelColor,
            LoggingConfiguration.WarningSenderColor,
            LoggingConfiguration.WarningMessageColor,
            sender,
            message,
            true,
            false);

    /// <summary>
    /// LogError logs message and do nothing.
    /// </summary>
    /// <param name="sender">Sender is a part of the message, but highlighted separately</param>
    /// <param name="message">Message is a simple message string</param>
    public static void LogError(string sender, string message) =>
        Log("ERROR",
            LoggingConfiguration.ErrorLabelColor,
            LoggingConfiguration.ErrorSenderColor,
            LoggingConfiguration.ErrorMessageColor,
            sender,
            message,
            true,
            false);

    /// <summary>
    /// LogFatal logs message and throws 1 from Environment.Exit. Execution stops.
    /// </summary>
    /// <param name="sender">Sender is a part of the message, but highlighted separately</param>
    /// <param name="message">Message is a simple message string</param>
    public static void LogFatal(string sender, string message) =>
        Log("FATAL",
            LoggingConfiguration.FatalLabelColor,
            LoggingConfiguration.FatalSenderColor,
            LoggingConfiguration.FatalMessageColor,
            sender,
            message,
            true,
            false,
            true);

    public static void Log(string label, ConsoleColor lColor, ConsoleColor sColor, ConsoleColor mColor, string sender, string message, bool showTime = true, bool isDebug = true, bool doExit = false)
    {
        if (!isDebug && LoggingConfiguration.IsDebug) return;
        var conColor = Console.ForegroundColor;
        if (showTime) Console.Write($@"[{DateTime.Now.TimeOfDay:hh\:mm\:ss}] ");
        Console.ForegroundColor = lColor;
        Console.Write($"[{label}] ".PadRight(LoggingConfiguration.LabelWidthAllign + 3));
        Console.ForegroundColor = sColor;
        
        if (sender.Length > LoggingConfiguration.MaxSenderLengthValue) LoggingConfiguration.MaxSenderLengthValue = sender.Length;
        
        Console.Write($"[{sender}] ".PadRight(LoggingConfiguration.MaxSenderLengthValue + 3));
        Console.ForegroundColor = mColor;
        Console.Write($"{message}");
        Console.WriteLine();
        Console.ForegroundColor = conColor;
        
        if (doExit) Environment.Exit(1);
    }
}