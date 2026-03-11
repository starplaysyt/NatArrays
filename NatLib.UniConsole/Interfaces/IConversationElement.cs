namespace NatLib.UniConsole.Interfaces;

public interface IConversationElement
{
    public (int, int) CursorLocation { get; set; }

    public bool DistinctAfterUsage { get; set; }
    
    public IConversationElement? NextElement { get; set; }
    
    public void Start();
}