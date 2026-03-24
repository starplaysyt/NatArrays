using NatLib.UniConsole.Graphics;

namespace NatLib.Debug;

public static class ConsoleRendererManualTests
{
    public static void WriteMessageLinesTest()
    {
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageLine("Executing WriteMessageLines()");
        ConsoleRenderer.WriteSeparator();
        
        // Empty string
        ConsoleRenderer.WriteMessageLines("");
        
        ConsoleRenderer.WriteSeparator();
        
        // One short line
        ConsoleRenderer.WriteMessageLines("jansjd");
        
        ConsoleRenderer.WriteSeparator();
        
        // One long line
        ConsoleRenderer.WriteMessageLines("abcdefghijklmnopqrstuvwxyz" +
                                          "abcdefghijklmnopqrstuvwxyz" +
                                          "abcdefghijklmnopqrstuvwxyz" +
                                          "abcdefghijklmnopqrstuvwxyz");
        
        ConsoleRenderer.WriteSeparator();
        
        // Two short lines
        ConsoleRenderer.WriteMessageLines("One short line\nand other one");
        
        ConsoleRenderer.WriteSeparator();
        
        // One \n
        ConsoleRenderer.WriteMessageLines("\n");
        
        ConsoleRenderer.WriteSeparator();

        // One line with two \n
        ConsoleRenderer.WriteMessageLines("\nmessage\n");
        
        ConsoleRenderer.WriteSeparator();

        // Line with exact width
        ConsoleRenderer.WriteMessageLines("1================================================================1");
        
        ConsoleRenderer.WriteSeparator();
        
        // Two lines with exact width
        ConsoleRenderer.WriteMessageLines("1================================================================1\n" +
                                          "1================================================================1");
        
        ConsoleRenderer.WriteSeparator();

        // One long line and one short line
        ConsoleRenderer.WriteMessageLines("1================================================================1\n" +
                                          "aknjsdbasjdbkjk");
        
        ConsoleRenderer.WriteSeparator();
        
        // One short line and one long line
        ConsoleRenderer.WriteMessageLines("aknjsdbasjdbkjk\n" +
                                          "1================================================================1");
        
        ConsoleRenderer.WriteSeparator();
        
        // Several long lines and \n at the end
        ConsoleRenderer.WriteMessageLines("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\n" +
                                          "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\n" +
                                          "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\n" +
                                          "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\n"); // big info
        
        ConsoleRenderer.WriteBottomBorder();
    }
    
}