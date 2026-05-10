using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Graphics;

public class ConCursorControl(TextWriter writer) : ICursorControlStrategy
{
    public (int Left, int Top) Cursor
    { 
        get => (Console.CursorLeft, Console.CursorTop);
        set => writer.Write("\e[{0};{1}f", value.Top + 1, value.Left + 1);
    }
}