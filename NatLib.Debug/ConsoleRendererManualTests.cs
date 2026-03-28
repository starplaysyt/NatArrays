using NatLib.UniConsole.Graphics;

namespace NatLib.Debug;

public static class ConsoleRendererManualTests
{
    public static void WriteMessageLinesTest()
    {
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageLineSingle("Executing WriteMessageLines()");
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

    public static void WriteMessageLineIndexedTest()
    {
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageLineSingle("Executing WriteMessageLineIndexed()");
        ConsoleRenderer.WriteSeparator();
        
        // Empty string
        ConsoleRenderer.WriteMessageLineIndexed("", 1);
        
        ConsoleRenderer.WriteSeparator();
        
        // Short string
        ConsoleRenderer.WriteMessageLineIndexed("test message", 1);
        
        ConsoleRenderer.WriteSeparator();
        
        // Long string
        ConsoleRenderer.WriteMessageLineIndexed("test message test message test message test message test message test message test message test", 1);
        
        ConsoleRenderer.WriteSeparator();
        
        // Perfect string
        ConsoleRenderer.WriteMessageLineIndexed("test message out of bounds test input value number and more val", 1);
        
        ConsoleRenderer.WriteSeparator();
        
        // Long index with short string
        ConsoleRenderer.WriteMessageLineIndexed("asdjknasjkdk", 100000);
        
        ConsoleRenderer.WriteSeparator();
        
        // Long index with long string
        ConsoleRenderer.WriteMessageLineIndexed("akjsndjanksdnnaksnkjdnkjajksndkjansjkdnkjanskndkkasjkdbjasdjabskjbdjk", 10000);
        
        ConsoleRenderer.WriteBottomBorder();
    }

    public static void WriteMessageLineWrappedTest()
    {
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageLineSingle("Executing WriteMessageLineWrapped()");
        ConsoleRenderer.WriteSeparator();
        
        // Empty string
        ConsoleRenderer.WriteMessageLineWrapped("");
        
        ConsoleRenderer.WriteSeparator();
        
        // Short string
        ConsoleRenderer.WriteMessageLineWrapped("anjsndjkn");
        ConsoleRenderer.WriteSeparator();
        
        // Long string
        ConsoleRenderer.WriteMessageLineWrapped("kansdnjansjkndjknajknsndkjnaskjnkjdnkjanjksndjknjkasnkdnkjanjksnkjdnkanskjdnjkanskjdnkj");
        ConsoleRenderer.WriteSeparator();
        
        // Exact string 
        ConsoleRenderer.WriteMessageLineWrapped("kansdnjansjkndjknajknsndkjnaskjnkjdnkjanjksndjknjkasnkdnkjanjksnkn");
        ConsoleRenderer.WriteSeparator();
        
        // Long long string
        ConsoleRenderer.WriteMessageLineWrapped("ajnsjdnnakjnskdnjknaknskjdnjkaskjdn" +
                                                "ajknsjdnkannskdjknakjsndknkjanksndk" +
                                                "kajnskdjnakjsndknkanksjndjknajknsjk" +
                                                "nanskdnjkanjknsdkjnkjanksjndjknakjn" +
                                                "ajnskjdnkanskndkjaknsjkdnjknajknskd" +
                                                "kjansjdnajknsjkdnkjankjsndjkakjsnda" +
                                                "ajknsjdjnaksdnk");
        ConsoleRenderer.WriteBottomBorder();
    }
}