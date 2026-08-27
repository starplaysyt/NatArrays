namespace NatLib.BufConsole;

public class KeyPressedEventArgs : EventArgs
{
    public ConsoleKeyInfo KeyInfo { get; }
    public KeyPressedEventArgs(ConsoleKeyInfo ki) => KeyInfo = ki;
}

public class LineSubmittedEventArgs : EventArgs
{
    public string Line { get; }
    public LineSubmittedEventArgs(string line) => Line = line;
}

public class InputChangedEventArgs : EventArgs
{
    public string CurrentText { get; }
    public int CursorPosition { get; }
    public InputChangedEventArgs(string text, int pos)
    {
        CurrentText = text;
        CursorPosition = pos;
    }
}

public class InputManager : IDisposable
{
    public event EventHandler<KeyPressedEventArgs> KeyPressed;
    public event EventHandler<LineSubmittedEventArgs> LineSubmitted;
    public event EventHandler<InputChangedEventArgs> InputChanged;

    private Thread _thread;
    private volatile bool _running;
    private readonly object _lock = new();

    private readonly List<char> _buf = new();
    private int _cursor;

    private volatile bool _lineMode;
    private string _submittedLine;
    private readonly ManualResetEventSlim _lineReady = new(false);

    private readonly List<string> _history = new();
    private int _historyIdx;
    private string _savedCurrentInput; // сохранение текущего ввода при навигации по истории

    public bool IsLineMode => _lineMode;

    public string CurrentInput
    {
        get { lock (_lock) return new string(_buf.ToArray()); }
    }

    public int CursorPosition
    {
        get { lock (_lock) return _cursor; }
    }

    public void Start()
    {
        if (_running) return;
        _running = true;
        _thread = new Thread(Loop) { IsBackground = true, Name = "InputLoop" };
        _thread.Start();
    }

    public void Stop()
    {
        _running = false;
        _lineReady.Set();
    }

    public string ReadLine()
    {
        lock (_lock)
        {
            _buf.Clear();
            _cursor = 0;
            _lineMode = true;
            _lineReady.Reset();
            _historyIdx = _history.Count;
            _savedCurrentInput = "";
        }

        FireInputChanged();
        _lineReady.Wait();
        _lineMode = false;

        var line = _submittedLine ?? "";
        if (!string.IsNullOrWhiteSpace(line))
        {
            lock (_lock)
            {
                // Не добавляем дубликат последней команды
                if (_history.Count == 0 || _history[^1] != line)
                    _history.Add(line);
            }
        }

        return line;
    }

    private void Loop()
    {
        while (_running)
        {
            if (!Console.KeyAvailable)
            {
                Thread.Sleep(5);
                continue;
            }

            var ki = Console.ReadKey(intercept: true);
            KeyPressed?.Invoke(this, new KeyPressedEventArgs(ki));

            if (!_lineMode) continue;

            bool changed = false;

            lock (_lock)
            {
                // Ctrl+комбинации
                bool ctrl = (ki.Modifiers & ConsoleModifiers.Control) != 0;

                switch (ki.Key)
                {
                    case ConsoleKey.Enter:
                        _submittedLine = new string(_buf.ToArray());
                        _buf.Clear();
                        _cursor = 0;
                        _lineReady.Set();
                        LineSubmitted?.Invoke(this, new LineSubmittedEventArgs(_submittedLine));
                        continue;

                    case ConsoleKey.Backspace:
                        if (ctrl)
                        {
                            // Ctrl+Backspace — удалить слово назад
                            int target = FindWordBoundaryLeft();
                            if (target < _cursor)
                            {
                                _buf.RemoveRange(target, _cursor - target);
                                _cursor = target;
                                changed = true;
                            }
                        }
                        else if (_cursor > 0)
                        {
                            _cursor--;
                            _buf.RemoveAt(_cursor);
                            changed = true;
                        }
                        break;

                    case ConsoleKey.Delete:
                        if (_cursor < _buf.Count)
                        {
                            _buf.RemoveAt(_cursor);
                            changed = true;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (ctrl)
                        {
                            int target = FindWordBoundaryLeft();
                            if (target != _cursor) { _cursor = target; changed = true; }
                        }
                        else if (_cursor > 0)
                        {
                            _cursor--;
                            changed = true;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (ctrl)
                        {
                            int target = FindWordBoundaryRight();
                            if (target != _cursor) { _cursor = target; changed = true; }
                        }
                        else if (_cursor < _buf.Count)
                        {
                            _cursor++;
                            changed = true;
                        }
                        break;

                    case ConsoleKey.Home:
                        if (_cursor != 0) { _cursor = 0; changed = true; }
                        break;

                    case ConsoleKey.End:
                        if (_cursor != _buf.Count) { _cursor = _buf.Count; changed = true; }
                        break;

                    case ConsoleKey.UpArrow:
                        if (_history.Count > 0)
                        {
                            // Сохраняем текущий ввод при первом нажатии вверх
                            if (_historyIdx == _history.Count)
                                _savedCurrentInput = new string(_buf.ToArray());

                            if (_historyIdx > 0)
                            {
                                _historyIdx--;
                                Replace(_history[_historyIdx]);
                                changed = true;
                            }
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (_historyIdx < _history.Count - 1)
                        {
                            _historyIdx++;
                            Replace(_history[_historyIdx]);
                            changed = true;
                        }
                        else if (_historyIdx == _history.Count - 1)
                        {
                            _historyIdx = _history.Count;
                            Replace(_savedCurrentInput ?? "");
                            changed = true;
                        }
                        break;

                    // Ctrl+U — очистить строку
                    case ConsoleKey.U:
                        if (ctrl)
                        {
                            _buf.Clear();
                            _cursor = 0;
                            changed = true;
                        }
                        else goto default;
                        break;

                    // Ctrl+A — в начало
                    case ConsoleKey.A:
                        if (ctrl && _cursor != 0)
                        {
                            _cursor = 0;
                            changed = true;
                        }
                        else goto default;
                        break;

                    // Ctrl+E — в конец
                    case ConsoleKey.E:
                        if (ctrl && _cursor != _buf.Count)
                        {
                            _cursor = _buf.Count;
                            changed = true;
                        }
                        else goto default;
                        break;

                    // Ctrl+K — удалить от курсора до конца
                    case ConsoleKey.K:
                        if (ctrl && _cursor < _buf.Count)
                        {
                            _buf.RemoveRange(_cursor, _buf.Count - _cursor);
                            changed = true;
                        }
                        else goto default;
                        break;

                    default:
                        if (!ctrl && ki.KeyChar >= 32)
                        {
                            _buf.Insert(_cursor, ki.KeyChar);
                            _cursor++;
                            changed = true;
                        }
                        break;
                }
            }

            if (changed) FireInputChanged();
        }
    }

    /// <summary>Найти границу слова слева от курсора</summary>
    private int FindWordBoundaryLeft()
    {
        int pos = _cursor - 1;
        // Пропускаем пробелы
        while (pos > 0 && _buf[pos] == ' ') pos--;
        // Пропускаем символы слова
        while (pos > 0 && _buf[pos - 1] != ' ') pos--;
        return Math.Max(0, pos);
    }

    /// <summary>Найти границу слова справа от курсора</summary>
    private int FindWordBoundaryRight()
    {
        int pos = _cursor;
        int len = _buf.Count;
        // Пропускаем символы слова
        while (pos < len && _buf[pos] != ' ') pos++;
        // Пропускаем пробелы
        while (pos < len && _buf[pos] == ' ') pos++;
        return pos;
    }

    private void Replace(string s)
    {
        _buf.Clear();
        _buf.AddRange(s);
        _cursor = _buf.Count;
    }

    private void FireInputChanged()
    {
        string text;
        int pos;
        lock (_lock)
        {
            text = new string(_buf.ToArray());
            pos = _cursor;
        }
        InputChanged?.Invoke(this, new InputChangedEventArgs(text, pos));
    }

    public void Dispose()
    {
        Stop();
        _lineReady.Dispose();
    }
}