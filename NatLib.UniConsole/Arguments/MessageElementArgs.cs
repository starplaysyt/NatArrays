namespace NatLib.UniConsole.Arguments;

public struct MessageElementArgs
{
    public string Message { get; set; } = "Message";
    public bool WaitForUserKey { get; set; } = true;
    public bool DistinctAfterUsage { get; set; } = true;
    public bool ClearAtTheEnd { get; set; } = true;

    public MessageElementArgs()
    {

    }
}