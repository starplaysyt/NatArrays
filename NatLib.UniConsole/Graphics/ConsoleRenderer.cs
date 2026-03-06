using NatLib.Core.Unification;

namespace NatLib.UniConsole.Graphics;

public static class ConsoleRenderer
{
    private static (int Left, int Top) _checkpointLocation = new();

    public static readonly StringStructuralConfiguration Configuration = new();
    private static TextWriter Writer => Console.Out;
    
    private static ConsoleColorExt _currentForeground = ConsoleColorExt.Default;
    private static ConsoleColorExt _currentBackground = ConsoleColorExt.Default;
    
    #region ConsoleExt
    
    public static void SetForeground(ConsoleColorExt colorCode)
    {
        Writer.Write("\e[{0}m", colorCode + 30);
        _currentForeground = colorCode;
    }

    public static void SetBackground(ConsoleColorExt colorCode)
    {
        Writer.Write("\e[{0}m", colorCode+40);
        _currentBackground = colorCode;
    }
    
    public static void SetCursorPosition(int x, int y)
    {
        Writer.Write("\e[{0};{1}f", y, x);
    }
    
    public static void SetConsoleSize(int cols, int rows) =>
        Writer.Write("\e[8;{0};{1}t", rows, cols);
    
    public static void Clear() => Console.Clear();
    
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

    public static void SetCheckpoint() => _checkpointLocation = Console.GetCursorPosition();

    public static void GotoCheckpoint(bool clear = true)
    {
        if (clear)
        {
            var gotoPosition = Console.GetCursorPosition();
            var fillCharacter = Configuration.EmptyBlock;
            
            SetCursorPosition(_checkpointLocation.Left, _checkpointLocation.Top);
            
            var offsetY = Math.Abs(gotoPosition.Top - _checkpointLocation.Top);

            Span<char> fillChars = stackalloc char[Console.BufferWidth];
            fillChars.Fill(fillCharacter);
            
            for (var i = 0; i < offsetY; i++)
            {
                Writer.Write(fillCharacter);
            }
        }

        Console.SetCursorPosition(_checkpointLocation.Left, _checkpointLocation.Top);
    }

    public static void WriteFixedStringNext(ReadOnlySpan<char> str, int width, char empty)
    {
        // INFO: Refactored to stack allocation method. +-780ms -> 110ms (100_000 calls)
        Span<char> chars = stackalloc char[width];
        
        var strLen = Math.Min(str.Length, width);
        str[..strLen].CopyTo(chars);
        // if (width > str.Length)
            chars[str.Length..width].Fill(empty);
        
        Writer.Write(chars);
    }
    
    public static void WriteTopBorder()
    {
        // INFO: Refactored to stack allocation method. +-5600ms -> 778ms (100_000 calls)
        
        var (left, center, right, width) = Configuration.DeconstructTop();
        Span<char> chars = stackalloc char[width];
        
        chars[0] = left;
        chars[1..(width - 2)].Fill(center);
        chars[width - 1] = right;
        
        Writer.WriteLine(chars);
    }
    
    public static void WriteMessageInBounds(ReadOnlySpan<char> message)
    {
        // INFO: Refactored to stack allocation method. +-3300ms -> 319ms (100_000 calls)
        
        var (left, center, right, width) = Configuration.DeconstructMiddle();
        Span<char> chars = stackalloc char[width + 4];
        
        var strLen = Math.Min(message.Length, width);
        chars[0] = left;
        chars[1] = center;
        message[..strLen].CopyTo(chars[2..]);
        chars.Slice(strLen + 2, width - strLen).Fill(center);
        chars[^1] = right;
        
        Writer.WriteLine(chars);
    }

    public static void WriteSeparator()
    {
        var conf = Configuration;
        var writer = Console.Out;
        
        writer.Write(conf.SectionTRight);
        for (var i = 0; i < conf.PreferableWidth-2; i++)
        {
            writer.Write(conf.HorizontalLine);
        }
        writer.Write(conf.SectionTLeft);
        writer.Write('\n');
    }

    public static void WriteBottomBorder()
    {
        var conf = Configuration;
        var writer = Console.Out;
        
        writer.Write(conf.CornerBottomLeft);
        for (var i = 0; i < conf.PreferableWidth-2; i++)
        {
            writer.Write(conf.HorizontalLine);
        }
        writer.Write(conf.CornerBottomRight);
        writer.Write('\n');
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
}