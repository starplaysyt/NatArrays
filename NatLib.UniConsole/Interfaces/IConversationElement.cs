using NatLib.UniConsole.Conversations;

namespace NatLib.UniConsole.Interfaces;

public interface IConversationElement
{
    public (int, int) EntryCursorLocation { get; set; }

    public IConversationElement? NextElement { get; set; }

    public void Start();
}