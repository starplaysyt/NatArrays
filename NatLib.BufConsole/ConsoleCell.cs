namespace NatLib.BufConsole;

public struct ConsoleCell : IEquatable<ConsoleCell>
{
    public char Character;
    public ConsoleColor Foreground;
    public ConsoleColor Background;

    public ConsoleCell(char character, ConsoleColor foreground = ConsoleColor.Gray, ConsoleColor background = ConsoleColor.Black)
    {
        Character = character;
        Foreground = foreground;
        Background = background;
    }

    public static ConsoleCell Empty => new ConsoleCell(' ', ConsoleColor.Gray, ConsoleColor.Black);

    public bool Equals(ConsoleCell other)
    {
        return Character == other.Character
               && Foreground == other.Foreground
               && Background == other.Background;
    }

    public override bool Equals(object obj) => obj is ConsoleCell cell && Equals(cell);
    public override int GetHashCode() => HashCode.Combine(Character, Foreground, Background);
}