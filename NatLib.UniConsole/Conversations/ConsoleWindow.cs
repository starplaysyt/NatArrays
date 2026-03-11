using System.Diagnostics;
using System.Numerics;
using NatLib.UniConsole.Graphics;

namespace NatLib.UniConsole.Conversations;

public class ConsoleWindow
{
    public string Title { get; set; }
    
    public void Start()
    {
        var editingStopwatch = new Stopwatch();

        var lastWidth = Console.WindowWidth;
        var lastHeight = Console.WindowHeight;

        Console.TreatControlCAsInput = true;
        Console.CursorVisible = false;

        Console.Title = "TestTitle";

        Console.Clear();

        ConsoleRenderer.Configuration.PreferableWidth = lastWidth;

        while (true)
        {
            int currentWidth = Console.WindowWidth;
            int currentHeight = Console.WindowHeight;

            bool sizeChanged =
                currentWidth != lastWidth ||
                currentHeight != lastHeight;

            if (sizeChanged)
            {
                editingStopwatch.Restart();

                lastWidth = currentWidth;
                lastHeight = currentHeight;

                Console.Clear();
                Console.WriteLine($"Changing... {currentWidth}x{currentHeight}");
            }
            else if (editingStopwatch.IsRunning)
            {
                if (editingStopwatch.ElapsedMilliseconds > 400)
                {
                    editingStopwatch.Reset();
                    Console.Clear();
                    Console.WriteLine("Resize finished     ");
                    ConsoleRenderer.Configuration.PreferableWidth = lastWidth;
                }
            }
            else
            {
                ConsoleRenderer.Configuration.PreferableWidth = 28;
                ConsoleRenderer.SetCursorPosition(0,0);
                ConsoleRenderer.WriteMessageInBounds("Message");
                ConsoleRenderer.WriteSeparator();
            }
            
            int sleep =
                editingStopwatch.IsRunning
                    ? 50 
                    : 500;

            Thread.Sleep(sleep);
        }
    }
}