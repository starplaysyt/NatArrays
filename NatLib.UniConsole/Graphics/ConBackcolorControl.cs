using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Graphics;

public class ConBackcolorControl(TextWriter writer) : IColorControlStrategy<ConsoleColorExt>
{
    public ConsoleColorExt Color
    {
        get;
        set
        {
            field = value;
            writer.Write("\e[{0}m", value + 40);
        }
    }
}