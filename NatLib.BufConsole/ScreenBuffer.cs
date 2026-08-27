namespace NatLib.BufConsole;

public class ScreenBuffer
{
    public int Width { get; }
    public int Height { get; }

    private readonly ConsoleCell[,] _cells;

    public ScreenBuffer(int width, int height)
    {
        Width = width;
        Height = height;
        _cells = new ConsoleCell[height, width];
        Clear();
    }

    public ref ConsoleCell this[int x, int y]
    {
        get
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException($"({x},{y}) out of ({Width},{Height})");
            return ref _cells[y, x];
        }
    }

    public bool IsInBounds(int x, int y)
        => x >= 0 && x < Width && y >= 0 && y < Height;

    public void Clear(ConsoleColor bg = ConsoleColor.Black)
    {
        var empty = new ConsoleCell(' ', ConsoleColor.Gray, bg);
        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Width; x++)
            _cells[y, x] = empty;
    }

    public void SetCell(int x, int y, char ch, ConsoleColor fg = ConsoleColor.Gray, ConsoleColor bg = ConsoleColor.Black)
    {
        if (!IsInBounds(x, y)) return;
        _cells[y, x] = new ConsoleCell(ch, fg, bg);
    }

    public ConsoleCell GetCell(int x, int y)
    {
        if (!IsInBounds(x, y)) return ConsoleCell.Empty;
        return _cells[y, x];
    }

    public void CopyFrom(ScreenBuffer other)
    {
        int w = Math.Min(Width, other.Width);
        int h = Math.Min(Height, other.Height);
        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
            _cells[y, x] = other._cells[y, x];
    }
}