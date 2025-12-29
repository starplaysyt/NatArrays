namespace NatLib.UniConsole.Conversations;

public class ConversationBlockManager
{
    private static Dictionary<Type, ConversationBlock> _blocks = new();

    public static void Add<T>(ConversationBlock block)
    {
        _blocks[typeof(T)] = block;
    }

    public static ConversationBlock? Get<T>() where T : ConversationBlock, new()
    {
        if (_blocks.TryGetValue(typeof(T), out var block))
        {
            return block;
        }
        else
        {
            block = new T();
            _blocks[typeof(T)] = block;
            return block;
        }
    }
}