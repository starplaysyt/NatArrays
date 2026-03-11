using System.Diagnostics;
using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Graphics;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class MessageConversationElement : IConversationElement
{
    public (int, int) CursorLocation { get; set; }
    public bool DistinctAfterUsage { get; set; }
    public IConversationElement? NextElement { get; set; }
    private string _message;
    private bool _pressAnyKey; 
    public MessageConversationElement(MessageElementArgs requestArgs, IConversationElement? nextElement)
    {
        _pressAnyKey = requestArgs.WaitForUserKey;
        _message = requestArgs.Message;
        DistinctAfterUsage = requestArgs.DistinctAfterUsage;
        NextElement = nextElement;
    }

    public void Start()
    {
        CursorLocation = ConsoleRenderer.GetCheckpoint();
        
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageInBounds(_message);
        if (_pressAnyKey)
        {
            ConsoleRenderer.WriteSeparator();
            ConsoleRenderer.WriteMessageInBounds("Press any key to continue...");
            ConsoleRenderer.WriteBottomBorder();
            Console.ReadKey(true);
        }
        else
        {
            ConsoleRenderer.WriteBottomBorder();
        }

        if (DistinctAfterUsage) 
            ConsoleRenderer.GotoCheckpoint(CursorLocation);
        
        NextElement?.Start();
        
        ConsoleRenderer.GotoCheckpoint(CursorLocation);
    }
}