namespace NatLib.UniConsole.Conversations;

public class ConversationQuery
{
    private List<IQueryElement> _queries = [];
    
    // queries left on screen for info, might be added here after invoking Update
    private List<IConversationQueryElement> _conversationQueries = [];
    
    private IQueryElement? _currentElement = null;


    public void RenderState()
    {
        foreach (var conversationQuery in _conversationQueries)
        {
            conversationQuery.RenderState();
        }
        
        _currentElement?.RenderState();
    }

    public bool Request(char request) => true;
}