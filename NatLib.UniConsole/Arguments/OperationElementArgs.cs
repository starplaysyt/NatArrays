namespace NatLib.UniConsole.Arguments;

public struct OperationElementArgs
{
    public Func<string>? OperationStartMessage;

    public Func<string>? OperationInProgressMessage;
    
    public Func<string>? OperationCompleteMessage;

    public Func<Task>? OperationAsyncDelegate;
    
    public Action? OperationSyncDelegate;

    public Action? EndOfExecutionDelegate;
    
    /// <summary>
    /// Defines either element should be removed from screen before calling next element, or not.
    /// </summary>
    public bool ClearBeforeNext { get; set; } = false;
    
    /// <summary>
    /// Defines either element should be removed from screen after it's child execution is over, or not.
    /// </summary>
    public bool ClearAtTheEnd { get; set; } = true;

    public OperationElementArgs()
    {
        
    }
}