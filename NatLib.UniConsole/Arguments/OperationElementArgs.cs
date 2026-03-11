namespace NatLib.UniConsole.Arguments;

public struct OperationElementArgs
{
    public string MainMessage { get; set; }
    public Func<bool> OperationDelegate { get; set; }
    public string SuccessMessage { get; set; }
    public string FailureMessage { get; set; }
}