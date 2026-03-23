using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class OperationConversationElement : IConversationElement
{
    public OperationConversationElement(
        OperationElementArgs requestArgs,
        IConversationElement? nextOperation)
    {

    }

    public void Draw()
    {
        throw new NotImplementedException();
    }
    public (int, int) EntryCursorLocation { get; set; }
    public bool DistinctAfterUsage { get; set; }
    public IConversationElement? NextElement { get; set; }

    public void Start()
    {
        throw new NotImplementedException();
    }
}