using System.Text;

namespace NatLib.BufConsole;

public class BufferedConsole : IDisposable
{
    private readonly ScreenBuffer _backBuffer;
    private readonly Renderer _renderer;
    private readonly DrawingSurface _surface;
    private readonly InputManager _input;

    private readonly object _bufferLock = new();
    private Timer _autoFlushTimer;

    // Состояние ввода (обновляется из событий InputManager)
    private string _currentInputText = "";
    private int _currentInputCursor;
    private volatile bool _inputActive;

    public int Width => _backBuffer.Width;
    public int Height => _backBuffer.Height;
    public DrawingSurface Surface => _surface;
    public InputManager Input => _input;

    // Настройки строки ввода
    public int InputLineY { get; set; }
    public string InputPrompt { get; set; } = "> ";
    public ConsoleColor InputFg { get; set; } = ConsoleColor.White;
    public ConsoleColor InputBg { get; set; } = ConsoleColor.Black;
    public ConsoleColor PromptFg { get; set; } = ConsoleColor.Cyan;

    public BufferedConsole(int? width = null, int? height = null, TextWriter writer = null)
    {
        Console.OutputEncoding = Encoding.UTF8;

        int w, h;
        try
        {
            w = width ?? Console.WindowWidth;
            h = height ?? Console.WindowHeight;
        }
        catch
        {
            w = width ?? 80;
            h = height ?? 25;
        }

        var tw = writer ?? Console.Out;

        _backBuffer = new ScreenBuffer(w, h);
        _renderer = new Renderer(tw, w, h);
        _surface = new DrawingSurface(_backBuffer);
        _input = new InputManager();

        InputLineY = h - 2;

        _input.InputChanged += OnInputChanged;

        // Очистка экрана
        tw.Write("\x1b[2J\x1b[H\x1b[?25l");
        tw.Flush();
    }

    public void StartInput() => _input.Start();

    public void EnableAutoFlush(int intervalMs = 33)
    {
        _autoFlushTimer?.Dispose();
        _autoFlushTimer = new Timer(_ => Flush(), null, 0, intervalMs);
    }

    public void DisableAutoFlush()
    {
        _autoFlushTimer?.Dispose();
        _autoFlushTimer = null;
    }

    /// <summary>
    /// Отрисовать буфер. Курсор показывается только если идёт ввод.
    /// </summary>
    public void Flush()
    {
        (int x, int y)? cursorPos = null;

        lock (_bufferLock)
        {
            if (_inputActive)
            {
                ComposeInputLine();
                cursorPos = CalculateCursorScreenPosition();
            }

            _renderer.Render(_backBuffer, cursorPos);
        }
    }

    public void ForceRedraw()
    {
        _renderer.ForceFullRedraw();
        Flush();
    }

    /// <summary>Потокобезопасное рисование</summary>
    public void Draw(Action<DrawingSurface> drawAction)
    {
        lock (_bufferLock)
        {
            drawAction(_surface);
        }
    }

    /// <summary>Блокирующее чтение строки</summary>
    public string ReadLine(string prompt = null)
    {
        if (prompt != null) InputPrompt = prompt;

        _inputActive = true;
        _currentInputText = "";
        _currentInputCursor = 0;

        Flush();

        var line = _input.ReadLine();

        _inputActive = false;

        // Очистить строку ввода
        lock (_bufferLock)
        {
            ClearInputLineInBuffer();
        }
        Flush();

        return line;
    }

    private void OnInputChanged(object sender, InputChangedEventArgs e)
    {
        _currentInputText = e.CurrentText;
        _currentInputCursor = e.CursorPosition;
        Flush();
    }

    /// <summary>Вычисляет позицию видимого курсора на экране</summary>
    private (int x, int y) CalculateCursorScreenPosition()
    {
        int startX = InputPrompt.Length;
        int maxVisible = Width - startX;

        int scrollOffset = 0;
        if (_currentInputCursor > maxVisible - 1)
            scrollOffset = _currentInputCursor - maxVisible + 1;

        int screenCursorX = startX + (_currentInputCursor - scrollOffset);
        if (screenCursorX >= Width) screenCursorX = Width - 1;

        return (screenCursorX, InputLineY);
    }

    /// <summary>Впечатывает строку ввода в back-буфер</summary>
    private void ComposeInputLine()
    {
        int y = InputLineY;

        // Очистка строки
        for (int x = 0; x < Width; x++)
            _backBuffer.SetCell(x, y, ' ', InputFg, InputBg);

        // Промпт
        for (int i = 0; i < InputPrompt.Length && i < Width; i++)
            _backBuffer.SetCell(i, y, InputPrompt[i], PromptFg, InputBg);

        // Текст с учётом скроллинга
        int startX = InputPrompt.Length;
        int maxVisible = Width - startX;
        if (maxVisible <= 0) return;

        int scrollOffset = 0;
        if (_currentInputCursor > maxVisible - 1)
            scrollOffset = _currentInputCursor - maxVisible + 1;

        string text = _currentInputText ?? "";

        for (int i = 0; i + scrollOffset < text.Length && i < maxVisible; i++)
        {
            int sx = startX + i;
            if (sx < Width)
                _backBuffer.SetCell(sx, y, text[i + scrollOffset], InputFg, InputBg);
        }
    }

    private void ClearInputLineInBuffer()
    {
        for (int x = 0; x < Width; x++)
            _backBuffer.SetCell(x, InputLineY, ' ', InputFg, InputBg);
    }

    public void Dispose()
    {
        DisableAutoFlush();
        _input.InputChanged -= OnInputChanged;
        _input.Dispose();

        var writer = Console.Out;
        writer.Write("\x1b[?25h\x1b[0m");
        writer.Flush();
    }
}