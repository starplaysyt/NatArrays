namespace NatLib.UniConsole.Graphics;

public static class ConsoleRenderer
{
    private static (int Left, int Top) _checkpointLocation = new();
    
    public static readonly RenderingStyleConfiguration Configuration 
        = new RenderingStyleConfiguration();

    public static void SetCheckpoint() => _checkpointLocation = Console.GetCursorPosition();

    public static void GotoCheckpoint(bool clear = true)
    {
        if (clear)
        {
            var gotoPosition = Console.GetCursorPosition();
            
            Console.SetCursorPosition(_checkpointLocation.Left, _checkpointLocation.Top);
            
            var offsetY = Math.Abs(gotoPosition.Top - _checkpointLocation.Top);
            
            for (var i = 0; i < offsetY; i++)
            {
                for (var j = 0; j < Console.BufferWidth; j++)
                {
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        Console.SetCursorPosition(_checkpointLocation.Left, _checkpointLocation.Top);
    }


    public static void Clear() => Console.Clear();
    
    public static void SetConsoleSizeAscii(int cols, int rows) =>
        Console.Write($"\e[8;{rows};{cols}t");

    public static void WriteFixedStringNext(string str, int width, char empty)
    {
        var writer = Console.Out;
        
        for (var i = 0; i < Math.Min(str.Length, width); i++)
            writer.Write(str[i]);

        for (var i = str.Length; i < width; i++)
            writer.Write(empty);
    }
    
    public static void WriteTopBorder()
    {
        var conf = Configuration;
        var writer = Console.Out;
        
        writer.Write(conf.CornerTopLeft);
        for (var i = 0; i < conf.PreferableWidth-2; i++)
        {
            Console.Write(conf.HorizontalLine);
        }
        writer.Write(conf.CornerTopRight);
        writer.Write('\n');
    }
    
    public static void WriteMessageInBounds(string message)
    {
        var conf = Configuration;
        var writer = Console.Out;
        
        writer.Write(conf.VerticalLine);
        writer.Write(' ');
        WriteFixedStringNext(message, conf.PreferableWidth - 4, ' ');
        writer.Write(' ');
        writer.Write(conf.VerticalLine);
        writer.Write('\n');
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