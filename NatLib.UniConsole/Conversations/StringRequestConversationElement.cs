using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class StringRequestConversationElement : IConversationElement
{
    public StringRequestConversationElement(
        RequestElementArgs<string> requestArgs,
        IConversationElement nextOperation)
    {
        
    }

    public void Draw()
    {
        throw new NotImplementedException();
    }
    public (int, int) CursorLocation { get; set; }
    public bool DistinctAfterUsage { get; set; }
    public IConversationElement? NextElement { get; set; }

    public void Start(ConversationQuery? parent = null)
    {
        throw new NotImplementedException();
    }
}