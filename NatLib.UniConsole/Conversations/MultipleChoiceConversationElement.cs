using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class MultipleChoiceConversationElement : IConversationElement
{
    private string[] _titles;
    private IConversationElement[] _conversations;

    public (int, int) EntryCursorLocation { get; set; }
    public bool DistinctAfterUsage { get; set; }
    public IConversationElement? NextElement { get; set; }

    public void Start()
    {
        
    }

    public MultipleChoiceConversationElement(
        List<(string Title, IConversationElement Conversation)> conversations)
    {
        _titles = new string[conversations.Count];
        _conversations = new IConversationElement[conversations.Count];

        for (var i = 0; i < conversations.Count; i++)
        {
            _titles[i] = conversations[i].Title;
            _conversations[i] = conversations[i].Conversation;
        }
    }
}