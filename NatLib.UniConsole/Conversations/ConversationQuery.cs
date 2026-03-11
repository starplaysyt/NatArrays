using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class ConversationQuery 
{
    public Dictionary<string, IConversationElement> StaticElements { get; set; }
    public IConversationElement RootElement { get; set; }
    public IConversationElement CurrentElement { get; set; }

    public ConversationQuery(
        Dictionary<string, IConversationElement> staticElements,
        IConversationElement rootElement)
    {
        StaticElements = staticElements;
        RootElement = rootElement;
    }

    public void Run()
    {
        foreach (var staticElement in StaticElements)
        {
            staticElement.Value.Start();
        }
        
        RootElement.Start();
    }
}