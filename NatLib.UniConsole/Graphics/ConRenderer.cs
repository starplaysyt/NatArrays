using NatLib.Core.Unification;
using NatLib.Core.Utils;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Graphics;

public class ConRenderer : IRenderer<ConsoleColorExt>
{
    public StringStructuralConfiguration Configuration { get; }
    public static ConRenderer Instance { get; } = new ConRenderer();

    private TextWriter Writer => Console.Out;

    // INFO: Implement way of rendering colors separately through strategy, or some other shit, idk

    public IColorControlStrategy<ConsoleColorExt> ForegroundControl { get; }

    public IColorControlStrategy<ConsoleColorExt> BackgroundControl { get; }
    
    public ICursorControlStrategy CursorControl { get; }

    public ConsoleColorExt Foreground
    { get => ForegroundControl.Color;
      set => ForegroundControl.Color = value; }

    public ConsoleColorExt Background
    { get => BackgroundControl.Color;
      set => BackgroundControl.Color = value; }

    public (int Left, int Top) CursorPosition
    { get => CursorControl.Cursor;
      set => CursorControl.Cursor = value; }

    public ConRenderer(StringStructuralConfiguration? configuration = null)
    {
        Configuration = configuration ?? StringStructuralConfiguration.Instance;
        ForegroundControl = new ConForecolorControl(Writer);
        BackgroundControl = new ConBackcolorControl(Writer);
        CursorControl = new ConCursorControl(Writer);
    }

    public void GotoCursor((int Left, int Top) cursor)
    {
        var gotoPosition = Console.GetCursorPosition();
        var fillCharacter = Configuration.EmptyBlock;
        
        CursorPosition = (cursor.Left, cursor.Top);

        var offsetY = Math.Abs(gotoPosition.Top - cursor.Top);

        Span<char> fillString = stackalloc char[Console.WindowWidth];
        fillString.Fill(fillCharacter);

        for (var i = 0; i < offsetY; i++)
        {
            CursorPosition = (0, cursor.Top + i);
            Writer.Write(fillString);
        }

        CursorPosition = (cursor.Left, cursor.Top);
    }

    public void WriteFixed(ReadOnlySpan<char> str, int width)
    {
        if (width < 0) throw new ArgumentOutOfRangeException(
                nameof(width), "Value should be greater than or equal to zero.");
        Span<char> chars = stackalloc char[width];
        var empty = Configuration.EmptyBlock;

        var strLen = Math.Min(str.Length, width);
        str[..strLen].CopyTo(chars);
        if (str.Length <= width)
            chars[str.Length..width].Fill(empty);
        else
        {
            var dotCount = Math.Min(width, 3);
            chars[^dotCount..].Fill('.');
        }

        Writer.Write(chars);
    }

    public void WriteTopBorder(int? dWidth = null)
    {
        var (left, center, right, width) = Configuration.DeconstructTop();
        width = dWidth ?? width;
        if (width < 2) throw new ArgumentOutOfRangeException(
                nameof(width), "WriteTopBorder cannot be executed when width is less than 2.");
        
        Span<char> chars = stackalloc char[width];

        chars[0] = left;
        chars[1..(width - 1)].Fill(center);
        chars[width - 1] = right;

        Writer.WriteLine(chars);
    }

    public void WriteMessageLineSingle(ReadOnlySpan<char> str, int? dWidth = null)
    {
        var (side, center, width) = Configuration.DeconstructMiddle();
        width = dWidth ?? width;
        if (width < 4) throw new ArgumentOutOfRangeException(
            nameof(width), "WriteMessageLineSingle cannot be executed when width is less than 4.");
        
        Span<char> chars = stackalloc char[width];

        var strLen = Math.Min(str.Length, width - 4);

        str[..strLen].CopyTo(chars[2..]);
        chars.Slice(strLen + 2, width - strLen - 2).Fill(center);
        if (str.Length > width - 4)
        {
            var dotCount = Math.Min(strLen, 3);
            chars[^(dotCount + 2)..^2].Fill('.');
        }

        chars[0] = side;
        chars[1] = center;
        chars[^1] = side;

        Writer.WriteLine(chars);
    }
    
    public void WriteMessageLineWrapped(ReadOnlySpan<char> message, int? dWidth = null)
    {
        var (side, center, width) = Configuration.DeconstructMiddle();
        width = dWidth ?? width;
        if (width < 5) throw new ArgumentOutOfRangeException(
            nameof(width), "WriteMessageLineSingle cannot be executed when width is less than 5.");
        
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
            var charsPtr = chars[(i * width)..((i + 1) * width)];
            charsPtr[0] = side;
            charsPtr[1] = center;
            charsCopied = SpanCharUtils.TryCopy(message[(lineWidth * i)..], charsPtr[2..^3]);
            charsPtr[^3] = center;
            charsPtr[^2] = side;
            charsPtr[^1] = '\n';
        }
        // Filling the last line with empty characters
        chars[^(lineWidth - charsCopied + 3)..^3].Fill(center);

        Writer.Write(chars);
    }
    
    public void WriteMessageLineIndexed(ReadOnlySpan<char> message, int index, int? dWidth = null)
    {
        var (side, center, width) = Configuration.DeconstructMiddle();
        width = dWidth ?? width;
        
        var indexString = index.ToString();
        
        Span<char> chars = stackalloc char[width];

        chars[0] = side;
        chars[1] = center;
        var copied = SpanCharUtils.TryCopy(indexString, chars[2..]);
        chars[2 + copied] = '.';
        chars[2 + copied + 1] = center;
        var copiedMessage = SpanCharUtils.TryCopy(message, chars[(4 + copied)..^2]);
        if (message.Length < width - copied - 5)
            chars[(4 + copied + copiedMessage)..^2].Fill(center);
        else
            chars[^5..^2].Fill('.');
        chars[^2] = center;
        chars[^1] = side;
        
        Writer.WriteLine(chars);
    }
    
    public void WriteMessageLines(ReadOnlySpan<char> message, int? dWidth = null)
    {
        var (side, center, width) = Configuration.DeconstructMiddle();
        width = dWidth ?? width;
        // One \n splits line in 2 lines
        var lineCount = message.Count('\n') + 1;
        width++;

        Span<char> chars = stackalloc char[width * lineCount];

        // Last absolute position in message
        var lastMessageIndex = 0;
        // Last absolute position in chars
        var lastCharsIndex = 0;

        for (var i = 0; i < lineCount; i++)
        {
            var messagePtr = message[lastMessageIndex..];
            var currentLineIndex = messagePtr.IndexOf('\n');
            if (currentLineIndex == -1) currentLineIndex = messagePtr.Length;
            var charsPtr = chars.Slice(lastCharsIndex, width);
            charsPtr[0] = side;
            charsPtr[1] = center;
            var charsCopied = SpanCharUtils.TryCopy(messagePtr[..currentLineIndex], charsPtr[2..^3]);
            charsPtr[(2 + charsCopied)..^3].Fill(center);
            charsPtr[^3] = center;
            charsPtr[^2] = side;
            charsPtr[^1] = '\n';

            lastCharsIndex += width;
            lastMessageIndex += currentLineIndex + 1;
        }

        Writer.Write(chars);
    }
    
    public void WriteMessageLines(string[] lines, int? dWidth = null)
    {
        foreach (var line in lines)
        {
            WriteMessageLineSingle(line, dWidth);
        }
    }

    public void WriteSeparator(int? dWidth = null)
    {
        var (left, center, right, width) = Configuration.DeconstructSeparator();
        width = dWidth ?? width;
        Span<char> chars = stackalloc char[width];
        chars[0] = left;
        chars[1..(width - 1)].Fill(center);
        chars[width - 1] = right;

        Writer.WriteLine(chars);
    }

    public void WriteBottomBorder(int? dWidth = null)
    {
        var (left, center, right, width) = Configuration.DeconstructBottom();
        width = dWidth ?? width;
        Span<char> chars = stackalloc char[width];
        chars[0] = left;
        chars[1..(width - 1)].Fill(center);
        chars[width - 1] = right;

        Writer.WriteLine(chars);
    }

    public void WriteMenu(string title, string[] menuItems, int? dWidth = null)
    {
        WriteTopBorder(dWidth);
        WriteMessageLineSingle(title, dWidth);
        WriteSeparator(dWidth);
        WriteMessageLines(menuItems, dWidth);
        WriteBottomBorder(dWidth);
    }
    
    public void WriteNumeratedMenu(string title, string[] menuItems, int numerationStart = 1, int? dWidth = null)
    {
        WriteTopBorder(dWidth);
        WriteMessageLineSingle(title, dWidth);
        WriteSeparator(dWidth);
        for (var i = 0; i < menuItems.Length; i++)
            WriteMessageLineSingle((i + numerationStart) + ". " + menuItems[i], dWidth);
        WriteBottomBorder(dWidth);
    }

    public void WriteMessageSingle(string message, int? dWidth = null)
    {
        WriteTopBorder(dWidth);
        WriteMessageLineSingle(message, dWidth);
        WriteBottomBorder(dWidth);
    }
    
    public void WriteMessage(string[] lines, int? dWidth = null)
    {
        WriteTopBorder(dWidth);
        foreach (var line in lines)
            WriteMessageLineSingle(line, dWidth);
        WriteBottomBorder(dWidth);
    }
    
    public void WriteMessageWrapped(ReadOnlySpan<char> message, int? dWidth = null)
    {
        WriteTopBorder();
        WriteMessageLineWrapped(message);
        WriteBottomBorder();
    }
    
    public void WriteLine() => Writer.WriteLine();
    
    public void WriteLine(ReadOnlySpan<char> str) => Writer.WriteLine(str);
    
    public void WriteLine(char ch) => Writer.WriteLine(ch);

    public void Write(ReadOnlySpan<char> str) => Writer.Write(str);
    
    public void Write(char ch) => Writer.Write(ch);

    public void ClearFromCursor() =>
        Writer.Write("\e[J");
    
    public void Clear() =>
        Console.Clear();
}