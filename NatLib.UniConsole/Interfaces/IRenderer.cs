namespace NatLib.UniConsole.Interfaces;

public interface IRenderer<TColor>
{
    public IColorControlStrategy<TColor> ForegroundControl { get; }
    
    public IColorControlStrategy<TColor> BackgroundControl { get; }
    
    public ICursorControlStrategy CursorControl { get; }
    
    public TColor Foreground { get; set; }
    
    public TColor Background { get; set; }
    
    public (int Left, int Top) CursorPosition { get; set; }

    public void GotoCursor((int Left, int Top) cursor);

    public void WriteFixed(ReadOnlySpan<char> str, int width);

    public void WriteTopBorder(int? dWidth = null);
    
    public void WriteMessageLineSingle(ReadOnlySpan<char> str, int? dWidth = null);

    public void WriteMessageLineWrapped(ReadOnlySpan<char> message, int? dWidth = null);

    public void WriteMessageLineIndexed(ReadOnlySpan<char> message, int index, int? dWidth = null);

    public void WriteMessageLines(ReadOnlySpan<char> message, int? dWidth = null);

    public void WriteMessageLines(string[] lines, int? dWidth = null);

    public void WriteSeparator(int? dWidth = null);

    public void WriteBottomBorder(int? dWidth = null);

    public void WriteMenu(string title, string[] menuItems, int? dWidth = null);

    public void WriteNumeratedMenu(string title, string[] menuItems, int numerationStart = 1, int? dWidth = null);

    public void WriteMessageSingle(string message, int? dWidth = null);

    public void WriteMessage(string[] lines, int? dWidth = null);

    public void WriteMessageWrapped(ReadOnlySpan<char> message, int? dWidth = null);

    public void WriteLine();

    public void WriteLine(ReadOnlySpan<char> str);

    public void WriteLine(char ch);

    public void Write(ReadOnlySpan<char> str);

    public void Write(char ch);

    public void ClearFromCursor();

    public void Clear();
}