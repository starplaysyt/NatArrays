namespace NatLib.UniConsole.Interfaces;

public interface ICursorControlStrategy
{
    public (int Left, int Top) Cursor { get; set; }
}