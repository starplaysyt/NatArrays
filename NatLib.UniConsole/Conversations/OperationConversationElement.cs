using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Graphics;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class OperationConversationElement : IConversationElement
{
    public (int, int) EntryCursorLocation { get; set; }
    
    public IConversationElement? NextElement { get; set; }
    
    public OperationElementArgs OperationArgs { get; set; }
    
    public OperationConversationElement(
        OperationElementArgs operationArgs,
        IConversationElement? nextOperation)
    {
        OperationArgs = operationArgs;
        NextElement = nextOperation;
    }

    public void Start()
    {
        var operationArgs = OperationArgs;
        EntryCursorLocation = ConsoleRenderer.GetCheckpoint();
        
        if (operationArgs.OperationStartMessage != null)
            ConsoleRenderer.WriteMessageSingle(operationArgs.OperationStartMessage.Invoke());
        
        if (operationArgs.OperationInProgressMessage != null)
            ConsoleRenderer.WriteMessageSingle(operationArgs.OperationInProgressMessage.Invoke());

        operationArgs.OperationAsyncDelegate?.Invoke().GetAwaiter().GetResult();
        
        operationArgs.OperationSyncDelegate?.Invoke();
        
        if (operationArgs.OperationCompleteMessage != null)
            ConsoleRenderer.WriteMessageSingle(operationArgs.OperationCompleteMessage.Invoke());
        
        if (operationArgs.ClearBeforeNext)
            ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);

        NextElement?.Start();

        if (operationArgs.ClearAtTheEnd)
            ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);
        
        operationArgs.EndOfExecutionDelegate?.Invoke();
    }
}