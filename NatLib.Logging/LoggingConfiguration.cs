namespace NatLib.Logging;

public static class LoggingConfiguration
{
    public static int MaxSenderLengthValue { get; set; } = 10;
    
    public static bool IsDebug { get; set; } = false;

    public static ConsoleColor InfoLabelColor = ConsoleColor.White;
    public static ConsoleColor InfoSenderColor = ConsoleColor.Gray;
    public static ConsoleColor InfoMessageColor = ConsoleColor.Gray;
        
    public static ConsoleColor DoneLabelColor = ConsoleColor.Green;
    public static ConsoleColor DoneSenderColor = ConsoleColor.DarkGreen;
    public static ConsoleColor DoneMessageColor = ConsoleColor.DarkGreen;
        
    public static ConsoleColor WarningLabelColor = ConsoleColor.Yellow;
    public static ConsoleColor WarningSenderColor = ConsoleColor.White;
    public static ConsoleColor WarningMessageColor = ConsoleColor.DarkYellow;
        
    public static ConsoleColor ErrorLabelColor = ConsoleColor.Red;
    public static ConsoleColor ErrorSenderColor = ConsoleColor.White;
    public static ConsoleColor ErrorMessageColor = ConsoleColor.DarkRed;
        
    public static ConsoleColor DebugLabelColor = ConsoleColor.Magenta;
    public static ConsoleColor DebugSenderColor = ConsoleColor.DarkMagenta;
    public static ConsoleColor DebugMessageColor = ConsoleColor.DarkMagenta;
        
    public static ConsoleColor FatalLabelColor = ConsoleColor.Red;
    public static ConsoleColor FatalSenderColor = ConsoleColor.DarkRed;
    public static ConsoleColor FatalMessageColor = ConsoleColor.DarkRed;
    
    public static int LabelWidthAllign { get; set; } = 5;
}