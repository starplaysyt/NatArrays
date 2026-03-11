using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class IntRequestConversationElement : IConversationElement
{
    public (int, int) CursorLocation { get; set; }
    public bool DistinctAfterUsage { get; set; }
    public IConversationElement? NextElement { get; set; }

    public IntRequestConversationElement(
        RequestElementArgs<int> requestArgs,
        IConversationElement nextOperation)
    {
        
    }
    
    public void Start()
    {
        throw new NotImplementedException();
    }
}