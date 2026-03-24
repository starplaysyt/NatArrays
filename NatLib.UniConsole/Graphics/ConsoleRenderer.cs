using NatLib.Core.Unification;
using NatLib.Core.Utils;

namespace NatLib.UniConsole.Graphics;

public static class ConsoleRenderer
{
    /// <summary>
    /// StringStructuralConfiguration object used to configure defaults used in generation.
    /// </summary>
    public static readonly StringStructuralConfiguration Configuration = new();

    private static TextWriter Writer => Console.Out;

    /// <summary>
    /// Gets or sets console foreground color.
    /// </summary>
    public static ConsoleColorExt CurrentForeground
    { get;
      set
      { field = value;
        SetForegroundColor(value); } }

    /// <summary>
    /// Gets or sets console background color.
    /// </summary>
    public static ConsoleColorExt CurrentBackground
    { get;
      set
      { field = value;
        SetBackgroundColor(value); } }

    /// <summary>
    /// First console window initialization, performs window clearing.
    /// </summary>
    static ConsoleRenderer()
    {
        Console.Clear();
    }

    /// <summary>
    /// Gets current console cursor alignment from left to right, and from top to bottom.
    /// </summary>
    public static (int Left, int Top) GetCheckpoint() => (Console.CursorLeft, Console.CursorTop);

    /// <summary>
    /// Moves cursor to given alignment.
    /// </summary>
    /// <param name="checkpoint">New cursor alignment.</param>
    /// <param name="clear">When true - function will clear everything from previous cursor position to new one.</param>
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

    /// <summary>
    /// Pads given span to given width, and writes it out on the screen.
    /// </summary>
    /// <param name="str">Given span of chars to be padded.</param>
    /// <param name="width">Width of result output.</param>
    /// <param name="empty">Character, that will be used to fill empty characters.</param>
    public static void WriteFixed(ReadOnlySpan<char> str, int width, char empty = ' ')
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

    /// <summary>
    /// Writes top table border with using of Configuration characters with global width, set by Configuration.
    /// </summary>
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

    /// <summary>
    /// Writes table message line, wrapped by separators with global width, set by Configuration.
    /// </summary>
    public static void WriteMessageLine(ReadOnlySpan<char> message)
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

    /// <summary>
    /// Writes separator line with global width, set by Configuration.
    /// </summary>
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

    /// <summary>
    /// Writes bottom table border with using of Configuration characters with global width, set by Configuration.
    /// </summary>
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

    /// <summary>
    /// Writes menu table items with using of Configuration characters.
    /// </summary>
    /// <param name="title">Menu title.</param>
    /// <param name="menuItems">Array of title elements.</param>
    public static void WriteMenu(string title, string[] menuItems)
    {
        WriteTopBorder();
        WriteMessageLine(title);
        WriteSeparator();
        foreach (var item in menuItems)
        {
            WriteMessageLine(item);
        }
        WriteBottomBorder();
    }

    /// <summary>
    /// Writes menu table items with using of Configuration characters and provided numeration.
    /// </summary>
    /// <param name="title">Menu title.</param>
    /// <param name="menuItems">Array of title elements.</param>
    /// <param name="numerationStart">Number of first element.</param>
    public static void WriteNumeratedMenu(string title, string[] menuItems, int numerationStart = 1)
    {
        WriteTopBorder();
        WriteMessageLine(title);
        WriteSeparator();
        for (var i = 0; i < menuItems.Length; i++)
            WriteMessageLine((i + numerationStart) + ". " + menuItems[i]);
        WriteBottomBorder();
    }

    /// <summary>
    /// Writes bordered message with using of Configuration characters.
    /// </summary>
    /// <param name="message">Given message.</param>
    public static void WriteMessage(string message)
    {
        WriteTopBorder();
        WriteMessageLine(message);
        WriteBottomBorder();
    }

    /// <summary>
    /// Writes bordered message with several lines inside with using of Configuration characters.
    /// </summary>
    /// <param name="lines">Given message lines.</param>
    public static void WriteMessageMultiline(string[] lines)
    {
        WriteTopBorder();
        foreach (var line in lines)
            WriteMessageLine(line);
        WriteBottomBorder();
    }

    /// <summary>
    /// Writes bordered message with several lines inside with using of Configuration characters.
    /// and performs word-wrap when needed.
    /// </summary>
    /// <param name="message">Given message.</param>
    public static void WriteMessageWrap(ReadOnlySpan<char> message)
    {
        var (side, center, width) = Configuration.DeconstructMiddle();
        var lineWidth = width - 4; // Length of content for one single line
        var linesCount = message.Length / lineWidth;

        // That fixes two serious problems - division inconsistency and zero-length strings.
        if (message.Length % lineWidth != 0 || message.Length == 0) linesCount++;

        // Incrementing width for \n character addition.
        width++;

        Span<char> chars = stackalloc char[width * linesCount];

        var charsCopied = 0;

        // Structure is like that - '| <linewidth> |\n' = 5 extra characters for manual fill.
        for (var i = 0; i < linesCount; i++)
        {
            var localSpan = chars[(i * width)..((i + 1) * width)];
            localSpan[0] = side;
            localSpan[1] = center;
            charsCopied = SpanCharUtils.TryCopy(message[(lineWidth * i)..], localSpan[2..^3]);
            localSpan[^3] = center;
            localSpan[^2] = side;
            localSpan[^1] = '\n';
        }
        // Filling the last line with empty characters
        chars[^(lineWidth - charsCopied + 3)..^3].Fill(center);

        Writer.Write(chars);
    }

    /// <summary>
    /// Writes empty line.
    /// </summary>
    public static void WriteLine() => Writer.WriteLine();

    /// <summary>
    /// Writes span of chars to the console.
    /// </summary>
    public static void Write(ReadOnlySpan<char> str) => Writer.Write(str);

    /// <summary>
    /// Writes span of chars to the console and moves cursor to the next line.
    /// </summary>
    public static void WriteLine(ReadOnlySpan<char> str) => Writer.WriteLine(str);

    /// <summary>
    /// Writes character to the console.
    /// </summary>
    public static void Write(char ch) => Writer.Write(ch);

    /// <summary>
    /// Writes character to the console and moves cursor to the next line.
    /// </summary>
    public static void WriteLine(char ch) => Writer.WriteLine(ch);

    #region Reading Functions
    /// <summary>
    /// Reads key from the console if its available, and returns ConsoleKey and result of operation.
    /// </summary>
    public static bool TryGetKey(out ConsoleKey key)
    {
        key = Console.KeyAvailable ? Console.ReadKey(true).Key : ConsoleKey.None;
        return Console.KeyAvailable;
    }

    /// <summary>
    /// Reads key from the console if its available, and returns char key representation and result of operation.
    /// </summary>
    public static bool TryGetKey(out char key)
    {
        key = Console.KeyAvailable ? Console.ReadKey(true).KeyChar : char.MinValue;
        return Console.KeyAvailable;
    }

    /// <summary>
    /// Reads key from the console, waits until it's available, and returns ConsoleKey of given key.
    /// </summary>
    /// <param name="intercept">Defines either key should be shown in console window or not.</param>
    public static ConsoleKey ReadConsoleKey(bool intercept = false) =>
        Console.ReadKey(intercept).Key;

    /// <summary>
    /// Reads key from the console, waits until it's available, and returns char representation of given key.
    /// </summary>
    /// <param name="intercept">Defines either key should be shown in console window or not.</param>
    public static char ReadCharKey(bool intercept = false) =>
        Console.ReadKey(intercept).KeyChar;

    /// <summary>
    /// Reads key from the console, waits until it's available, and returns ConsoleKeyInfo of given key.
    /// </summary>
    /// <param name="intercept">Defines either key should be shown in console window or not.</param>
    public static ConsoleKeyInfo ReadKey(bool intercept = false) =>
        Console.ReadKey(intercept);

    /// <summary>
    /// Reads line from console. Returns empty string when returned line is null.
    /// </summary>
    public static string ReadLine() =>
        Console.ReadLine() ?? string.Empty;
    #endregion

    #region Escape-sequence Functions
    /// <summary>
    /// Clears everything in console from the current cursor position to the end of the buffer.
    /// </summary>
    public static void ClearFromCursor() =>
        Writer.Write("\e[J");

    /// <summary>
    /// Sets cursor position in the console.
    /// </summary>
    public static void SetCursorPosition(int x, int y) =>
        Writer.Write("\e[{0};{1}f", y + 1, x + 1);

    /// <summary>
    /// Sets the size of the console.
    /// </summary>
    public static void SetConsoleSize(int cols, int rows) =>
        Writer.Write("\e[8;{0};{1}t", rows, cols);

    /// <summary>
    /// Clears everything in the console.
    /// </summary>
    public static void Clear() =>
        Console.Clear();

    /// <summary>
    /// Resets foreground color of the console text.
    /// </summary>
    public static void ResetForeground() =>
        CurrentForeground = ConsoleColorExt.Default;

    /// <summary>
    /// Resets background color of the console text.
    /// </summary>
    public static void ResetBackground() =>
        CurrentBackground = ConsoleColorExt.Default;

    private static void SetForegroundColor(ConsoleColorExt color) =>
        Writer.Write("\e[{0}m", color + 30);

    private static void SetBackgroundColor(ConsoleColorExt color) =>
        Writer.Write("\e[{0}m", color + 40);
    #endregion
}