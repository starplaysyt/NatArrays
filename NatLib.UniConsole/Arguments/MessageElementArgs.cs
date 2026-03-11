namespace NatLib.UniConsole.Arguments;

public struct MessageElementArgs
{
    public string Message { get; set; }
    
    public bool WaitForUserKey { get; set; }
    
    public bool DistinctAfterUsage { get; set; }
}