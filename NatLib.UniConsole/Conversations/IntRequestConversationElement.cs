using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Graphics;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class IntRequestConversationElement : IConversationElement
{
    public (int, int) CursorLocation { get; set; }
    public IConversationElement? NextElement { get; set; }
    public RequestElementArgs<int> RequestArgs { get; set; }

    public IntRequestConversationElement(
        RequestElementArgs<int> requestArgs,
        IConversationElement? nextOperation)
    {
        RequestArgs = requestArgs;
        NextElement = nextOperation;
    }
    
    public void Start(ConversationQuery? parent = null)
    {
        var reqArgs = RequestArgs;
        CursorLocation = ConsoleRenderer.GetCheckpoint();
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageInBounds(reqArgs.Message());

        if (reqArgs.ReferencePresenter != null) // Reference generation
        {
            ConsoleRenderer.WriteSeparator();
            reqArgs.ReferencePresenter.PresentString();
            ConsoleRenderer.WriteBottomBorder();
        }
        
        ConsoleRenderer.WriteBottomBorder();
        
        var insertCursorPosition = ConsoleRenderer.GetCheckpoint();
        string inputString;
        int intInput;

        GOTO_INPUT_STARTING:
        
        ConsoleRenderer.Write(">>> ");
        inputString = Console.ReadLine() ?? "";
        reqArgs.RawValueBinding?.Invoke(inputString);

        if (int.TryParse(inputString, out intInput) 
            && reqArgs.ValidationDelegate?.Invoke(intInput) != false)
        {
            reqArgs.InvocationStatusProvider?.Invoke(true);
            reqArgs.ConvertedValueBinding?.Invoke(intInput);
        }
        else
        {
            ConsoleRenderer.WriteTopBorder();
            ConsoleRenderer.WriteMessageInBounds(reqArgs.RetryMessage());
            ConsoleRenderer.WriteSeparator();
            
            ConsoleRenderer.WriteMessageInBounds(reqArgs.CanBeQuit ? 
                "Press any key to retry, or Space key to leave..." :
                "Press any key to retry...");
            ConsoleRenderer.WriteBottomBorder();
            var key = Console.ReadKey(true).Key;
            
            if (key != ConsoleKey.Spacebar || !reqArgs.CanBeQuit)
            {
                ConsoleRenderer.GotoCheckpoint(insertCursorPosition);
                goto GOTO_INPUT_STARTING;
            }
            
            reqArgs.InvocationStatusProvider?.Invoke(false);
            
            if (reqArgs.DistinctAfterQuit)
                ConsoleRenderer.GotoCheckpoint(CursorLocation);
            
            reqArgs.QuitElement?.Start();
            
            // Okay, that means that QuitElement logically
            // becomes a new execution branch, not a way to fix
            // insertion problem, or let user try again. 
            if (reqArgs.QuitElementOverridesNextElement)
            {
                if (reqArgs.ClearAtTheEnd)
                    ConsoleRenderer.GotoCheckpoint(CursorLocation);
                return;
            }
        }
        
        if (reqArgs.DistinctAfterUsage)
            ConsoleRenderer.GotoCheckpoint(CursorLocation);
        
        NextElement?.Start();

        if (reqArgs.ClearAtTheEnd) 
            ConsoleRenderer.GotoCheckpoint(CursorLocation);
    }
}