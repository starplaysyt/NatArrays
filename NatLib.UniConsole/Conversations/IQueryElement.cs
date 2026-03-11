namespace NatLib.UniConsole.Conversations;

public interface IQueryElement
{
    public void RenderState();
    
    public TResponse Request<TRequest, TResponse>(TRequest request);
}