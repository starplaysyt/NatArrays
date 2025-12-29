namespace NatLib.UniConsole.Conversations;

public abstract class Conversation
{
    public virtual void DrawGeneralView() { }

    public abstract void RunConversation();
    
    public object ConversationSubject { get; set; }

    protected void RunBlock<T>(Action<T>? initial = null) where T : ConversationBlock, new()
    {
        var block = ConversationBlockManager.Get<T>() ?? throw new InvalidOperationException(); 
        // block = ConversationBlockManager.GetBlock(typeof(T))
        initial?.Invoke(block as T);
        block.ConversationSubject = ConversationSubject;
        
        BLOCK_RUNNING_START:

        if (block.DoClearWindow) Console.Clear();
        
        block.OnPresence();
        
        block.CurrentInput = Console.ReadLine() ?? string.Empty;

        if (!block.OnInput(out var errorMessage))
        {
            if (block.CanRetryExit)
            {
                block.OnRetryChoose();
                if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                    return;
            }
            else
            {
                block.OnErrorMessage(errorMessage);
                goto BLOCK_RUNNING_START;
            }
        }
        
        block.OnComplete();
    }
}