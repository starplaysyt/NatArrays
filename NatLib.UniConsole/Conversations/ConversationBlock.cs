using NatLib.UniConsole.Graphics;

namespace NatLib.UniConsole.Conversations;

public abstract class ConversationBlock
{
    /// <summary>
    /// Defines whether window should be cleared from other blocks before this one.
    /// </summary>
    public bool DoClearWindow { get; private set; } = false;
    
    /// <summary>
    /// Defines whether it is possible to leave entering cycle.
    /// </summary>
    public bool CanRetryExit { get; private set; } = false;
    
    /// <summary>
    /// Defines whether need to wait for any key after successful enter (OnComplete) or not.
    /// </summary>
    public bool DoCompleteExecute { get; private set; } = true;
    
    public bool DoCompleteKeyListening { get; private set; } = false;
    
    /// <summary>
    /// Stores current input data. Can be used for operations with user input (truncating, ect)
    /// </summary>
    public string CurrentInput { get; set; } = "";
    
    public string GlobalMessage { get; private set; } = "";
    
    /// <summary>
    /// Object of conversation, like Model of this pipeline. Changes during the conversation.
    /// </summary>
    public object? ConversationSubject { get; set; }
    
    /// <summary>
    /// Writes blocks data on a screen. Considered to use GlobalMessage and ConversationSubject, if needed.
    /// </summary>
    public abstract void OnPresence();

    /// <summary>
    /// Performs actions with input(proceeds it or updates somewhere in model, ect)
    /// and returns whether value can be proceed or not.
    /// </summary>
    public abstract bool OnInput(out object? errorInfo);

    /// <summary>
    /// Happens when CanRetryExit is false and validation fails.
    /// </summary>
    public virtual void OnErrorMessage(object? errorInfo)
    {
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageInBounds("Wrong input. Press any key to retry...");
        ConsoleRenderer.WriteBottomBorder();
    }

    /// <summary>
    /// Happens when CanRetryExit is true and validation fails.
    /// </summary>
    public virtual void OnRetryChoose()
    {
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageInBounds("Wrong input. Press any key to");
        ConsoleRenderer.WriteMessageInBounds("retry, or press Space to exit...");
        ConsoleRenderer.WriteBottomBorder();
    }

    /// <summary>
    /// Happens when this dialog successfully completed
    /// </summary>
    public virtual void OnComplete()
    {
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageInBounds("Conversation completed successfully.");
        ConsoleRenderer.WriteBottomBorder();
    }
}