using NatLib.Core.Utils;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class TableConversationElement<T> : IConversationElement where T : class
{
    private CollectionTablePresenter<T> _tableCollectionPresenter;
    public TableConversationElement(CollectionTablePresenter<T> presenter)
    {
        _tableCollectionPresenter = presenter;
    }
    public (int, int) CursorLocation { get; set; }
    public bool DistinctAfterUsage { get; set; }
    public IConversationElement? NextElement { get; set; }

    public void Start(ConversationQuery? parent = null)
    {
        throw new NotImplementedException();
    }
}