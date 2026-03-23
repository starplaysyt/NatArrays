using System.Diagnostics;
using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Graphics;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class MessageConversationElement : IConversationElement
{
    public (int, int) EntryCursorLocation { get; set; }
    public bool DistinctAfterUsage { get; set; }
    public IConversationElement? NextElement { get; set; }
    public MessageElementArgs RequestArgs { get; set; }

    public MessageConversationElement(MessageElementArgs requestArgs,
        IConversationElement? nextElement)
    {
        RequestArgs = requestArgs;
        DistinctAfterUsage = requestArgs.DistinctAfterUsage;
        NextElement = nextElement;
    }

    public void Start()
    {
        var reqArgs = RequestArgs;
        EntryCursorLocation = ConsoleRenderer.GetCheckpoint();

        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageInBounds(reqArgs.Message);
        if (reqArgs.WaitForUserKey)
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
            ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);

        NextElement?.Start();

        if (reqArgs.ClearAtTheEnd)
            ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);
    }
}