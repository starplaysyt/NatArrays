namespace NatLib.UniConsole.Conversations;

// Interface that helps query elements interacting with ConversationQuery to change its state.
public interface IConversationQueryElement : IQueryElement
{
    public ConversationQuery ConversationQuery { get; }
}