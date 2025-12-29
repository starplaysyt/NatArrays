using NatLib.UniConsole.Conversations;
using NatLib.UniConsole.Graphics;

namespace NatLib.Debug;

public class GetValueConversationBlock : ConversationBlock
{
    public GetValueConversationBlock() : base()
    {
        ConversationBlockManager.Add<GetValueConversationBlock>(this);
    }
    
    public override void OnPresence()
    {
        ConsoleRenderer.ShowMessageBox("Enter a value: ");
    }

    public override bool OnInput(out object? errorInfo)
    {
        if (int.TryParse(CurrentInput, out var result))
        {
            errorInfo = result;
            return true;
        }
        else
        {
            errorInfo = "not value";
            return false;
        }
    }
}

public class GetIntValueConversation : Conversation
{
    public override void RunConversation()
    {
        RunBlock<GetValueConversationBlock>();
    }
}