using NatLib.UniConsole.Graphics;
using NatLib.UniConsole.Interfaces;

namespace NatLib.Debug;

public class ConRendererManualTests
{
    public static void Run()
    {
        IRenderer<ConsoleColorExt> renderer = ConRenderer.Instance;
        
        renderer.Write("This is a test write line. ");
        renderer.WriteLine("This is a test write line. ");

        renderer.Foreground = ConsoleColorExt.White;
        renderer.Background = ConsoleColorExt.Red;
        renderer.WriteLine("This is a colored write line. ");

        renderer.Foreground = ConsoleColorExt.Default;
        renderer.Background = ConsoleColorExt.Default;
        renderer.WriteLine("This is a defaulted write line. ");

        renderer.CursorPosition = (5, 5);
        renderer.WriteLine("This is a with set write line. ");
        renderer.CursorPosition = (0, 8);
        
        // WriteFixed tests
        renderer.WriteFixed("This is a exact fixed write line.", 33);
        renderer.Write("|");
        renderer.WriteLine();
        renderer.WriteFixed("This is a small fixed line.", 33);
        renderer.Write("|");
        renderer.WriteLine();
        renderer.WriteFixed("This is a large fixed line, very large line.", 33);
        renderer.Write("|");
        renderer.WriteLine();
        renderer.WriteFixed("And zero-widthed line, it should be invisible", 0);
        renderer.Write("|");
        renderer.WriteLine();
        
        // WriteTopBorder tests
        renderer.WriteTopBorder();
        renderer.WriteTopBorder(20);
        renderer.WriteTopBorder(3);
        renderer.WriteTopBorder(2);
        
        // WriteMessageLineSingle tests 
        renderer.WriteLine("------- WriteMessageLineSingle tests: ");
        renderer.WriteMessageLineSingle("This is a trivial line.");
        renderer.WriteMessageLineSingle("This is an exact line.", 26);
        renderer.WriteMessageLineSingle("This is small line.", 26);
        renderer.WriteMessageLineSingle("This is a very large line.", 26);
        renderer.WriteMessageLineSingle("This is a one line", 9);
        renderer.WriteMessageLineSingle("This is a one line", 8);
        renderer.WriteMessageLineSingle("This is a one line", 7);
        renderer.WriteMessageLineSingle("This is a one line", 6);
        renderer.WriteMessageLineSingle("This is a one line", 5);
        renderer.WriteMessageLineSingle("This is a zero line", 4);
        renderer.WriteMessageLineSingle("", 4);
        renderer.WriteMessageLineSingle("", 5);
        renderer.WriteMessageLineSingle("", 6);
        
        // WriteMessageLineWrapped tests
        renderer.WriteLine("------- WriteMessageLineWrapped tests: ");
        renderer.WriteMessageLineWrapped("This is a trivial line.");
        renderer.WriteMessageLineWrapped("This is an exact line.", 26);
        renderer.WriteMessageLineWrapped("This is small line.", 26);
        renderer.WriteMessageLineWrapped("This is a very large line.", 26);
        renderer.WriteMessageLineWrapped("This is a wrapped line", 9);
        renderer.WriteMessageLineWrapped("This is a wrapped line", 8);
        renderer.WriteMessageLineWrapped("This is a wrapped line", 7);
        renderer.WriteMessageLineWrapped("This is a wrapped line", 6);
        renderer.WriteMessageLineWrapped("This is a wrapped line", 5);
        renderer.WriteMessageLineWrapped("", 5);
        renderer.WriteMessageLineWrapped("", 6);
        renderer.WriteMessageLineWrapped("", 7);
        
        // WriteMessageLineIndexed tests
        renderer.WriteLine("------- WriteMessageLineWrapped tests: ");
        renderer.WriteMessageLineIndexed("This is a trivial line.", 1);
        renderer.WriteMessageLineIndexed("This is a exact line.", 1, 28);
        renderer.WriteMessageLineIndexed("This is small line.", 1, 28);
        renderer.WriteMessageLineIndexed("This is a very large line.", 1, 28);
        renderer.WriteMessageLineIndexed("This is a one line", 1, 10);
        renderer.WriteMessageLineIndexed("This is a one line", 1, 9);
    }
}