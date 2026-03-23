using System.Diagnostics;
using NatLib.Core.Unification;

namespace NatLib.UniConsole.Graphics;

public static class ConsoleRenderer
{
    public static readonly StringStructuralConfiguration Configuration = new();

    private static TextWriter Writer => Console.Out;

    public static ConsoleColorExt CurrentForeground
    { get => field;
      set
      { field = value;
        Writer.Write("\e[{0}m", value + 30); } }

    public static ConsoleColorExt CurrentBackground
    { get => field;
      set
      { field = value;
        Writer.Write("\e[{0}m", field + 40); } }

    static ConsoleRenderer()
    {
        Console.Clear();
    }

    #region ConsoleExt
    public static void ClearFromCursor() =>
        Writer.Write("\e[J");

    public static void ResetForeground() =>
        CurrentForeground = ConsoleColorExt.Default;
    public static void ResetBackground() =>
        CurrentBackground = ConsoleColorExt.Default;

    public static void SetCursorPosition(int x, int y) =>
        Writer.Write("\e[{0};{1}f", y + 1, x + 1);

    public static void SetConsoleSize(int cols, int rows) =>
        Writer.Write("\e[8;{0};{1}t", rows, cols);

    public static void Clear() =>
        Console.Clear();

    public static bool TryGetKey(out ConsoleKey key)
    {
        key = Console.KeyAvailable ? Console.ReadKey(true).Key : ConsoleKey.None;
        return Console.KeyAvailable;
    }

    public static bool TryGetKey(out char key)
    {
        key = Console.KeyAvailable ? Console.ReadKey(true).KeyChar : char.MinValue;
        return Console.KeyAvailable;
    }
    #endregion

    public static (int Left, int Top) GetCheckpoint() => (Console.CursorLeft, Console.CursorTop);

    public static void GotoCheckpoint((int Left, int Top) checkpoint, bool clear = true)
    {
        if (clear)
        {
            var gotoPosition = Console.GetCursorPosition();
            var fillCharacter = Configuration.EmptyBlock;

            SetCursorPosition(checkpoint.Left, checkpoint.Top);

            var offsetY = Math.Abs(gotoPosition.Top - checkpoint.Top);

            Span<char> fillString = stackalloc char[Console.WindowWidth];
            fillString.Fill(fillCharacter);

            for (var i = 0; i < offsetY; i++)
            {
                SetCursorPosition(0, checkpoint.Top + i);
                Writer.Write(fillString);
            }
        }

        SetCursorPosition(checkpoint.Left, checkpoint.Top);
    }

    public static void WriteFixedStringNext(ReadOnlySpan<char> str, int width, char empty)
    {
        // INFO: Refactored to stack allocation method. +-780ms -> 110ms (100_000 calls)
        Span<char> chars = stackalloc char[width];

        var strLen = Math.Min(str.Length, width);
        str[..strLen].CopyTo(chars);
        if (str.Length < width)
            chars[str.Length..width].Fill(empty);
        else
            chars[^3..].Fill('.');

        Writer.Write(chars);
    }

    public static void WriteTopBorder()
    {
        // INFO: Refactored to stack allocation method. +-5600ms -> 778ms (100_000 calls)

        var (left, center, right, width) = Configuration.DeconstructTop();
        Span<char> chars = stackalloc char[width];

        chars[0] = left;
        chars[1..(width - 1)].Fill(center);
        chars[width - 1] = right;

        Writer.WriteLine(chars);
    }

    public static void WriteMessageInBounds(ReadOnlySpan<char> message)
    {
        // INFO: Refactored to stack allocation method. +-3300ms -> 319ms (100_000 calls)

        var (side, center, width) = Configuration.DeconstructMiddle();
        Span<char> chars = stackalloc char[width];

        var strLen = Math.Min(message.Length, width - 4);

        message[..strLen].CopyTo(chars[2..]);
        chars.Slice(strLen + 2, width - strLen - 2).Fill(center);
        if (message.Length > width - 4) chars[(width - 5)..^2].Fill('.');

        chars[0] = side;
        chars[1] = center;
        chars[^1] = side;

        Writer.WriteLine(chars);
    }

    public static void WriteSeparator()
    {
        // INFO: Refactored to stack allocation method. +-4150ms -> 573ms

        var (left, center, right, width) = Configuration.DeconstructSeparator();
        Span<char> chars = stackalloc char[width];
        chars[0] = left;
        chars[1..(width - 1)].Fill(center);
        chars[width - 1] = right;

        Writer.WriteLine(chars);
    }

    public static void WriteBottomBorder()
    {
        // INFO: Refactored to stack allocation method. +-4150ms -> 573ms

        var (left, center, right, width) = Configuration.DeconstructBottom();
        Span<char> chars = stackalloc char[width];
        chars[0] = left;
        chars[1..(width - 1)].Fill(center);
        chars[width - 1] = right;

        Writer.WriteLine(chars);
    }

    public static void ShowMenuItems(string title, IEnumerable<string> menuItems)
    {
        WriteTopBorder();
        WriteMessageInBounds(title);
        WriteSeparator();
        foreach (var item in menuItems)
        {
            WriteMessageInBounds(item);
        }
        WriteBottomBorder();
    }

    public static void ShowMenuItemsWithNumeration(string title, IEnumerable<string> menuItems)
    {
        WriteTopBorder();
        WriteMessageInBounds(title);
        WriteSeparator();
        var counter = 1;
        foreach (var item in menuItems)
        {
            WriteMessageInBounds(counter + ". " + item);
            counter++;
        }
        WriteBottomBorder();
    }

    public static void ShowMessageBox(string title)
    {
        WriteTopBorder();
        WriteMessageInBounds(title);
        WriteBottomBorder();
    }

    public static void ShowMessageBoxMultiline(IEnumerable<string> lines)
    {
        WriteTopBorder();
        foreach (var line in lines)
            WriteMessageInBounds(line);
        WriteBottomBorder();
    }

    public static void WriteLine() => Writer.WriteLine();

    public static void Write(ReadOnlySpan<char> str) => Writer.Write(str);

    public static void WriteLine(ReadOnlySpan<char> str) => Writer.WriteLine(str);

    public static void Write(char ch) => Writer.Write(ch);

    public static void WriteLine(char ch) => Writer.WriteLine(ch);
}