namespace NatLib.UniConsole.Conversations;

public interface IConsolePipelineElement
{
    public void StartElement();

    public void RunElement();

    public void FinalElement();
}