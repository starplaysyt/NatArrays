namespace NatLib.UniConsole.Interfaces;

public interface IColorControlStrategy<TColor>
{
    public TColor Color { get; set; }
}