using System.Text;

namespace NatLib.BufConsole
{
    public class Renderer
    {
        private readonly TextWriter _writer;
        private ConsoleCell[,] _front;
        private readonly int _width;
        private readonly int _height;
        private bool _forceFullRedraw = true;

        public Renderer(TextWriter writer, int width, int height)
        {
            _writer = writer ?? Console.Out;
            _width = width;
            _height = height;
            _front = new ConsoleCell[height, width];
            InvalidateFront();
        }

        /// <summary>
        /// Рендер буфера. Если showCursorAt != null — показать курсор в этой позиции.
        /// Если null — курсор скрыт. Всё в одном Write для отсутствия мерцания.
        /// </summary>
        public void Render(ScreenBuffer backBuffer, (int x, int y)? showCursorAt = null)
        {
            var sb = new StringBuilder(_width * _height);

            // Скрыть курсор в начале
            sb.Append("\x1b[?25l");

            ConsoleColor curFg = (ConsoleColor)(-1);
            ConsoleColor curBg = (ConsoleColor)(-1);

            int maxY = Math.Min(_height, backBuffer.Height);
            int maxX = Math.Min(_width, backBuffer.Width);

            for (int y = 0; y < maxY; y++)
            {
                bool needMove = true;

                for (int x = 0; x < maxX; x++)
                {
                    var newCell = backBuffer.GetCell(x, y);
                    var oldCell = _front[y, x];

                    if (!_forceFullRedraw && newCell.Equals(oldCell))
                    {
                        needMove = true;
                        continue;
                    }

                    // Не пишем в правый нижний угол — скролл
                    if (y == _height - 1 && x == _width - 1)
                    {
                        _front[y, x] = newCell;
                        continue;
                    }

                    if (needMove)
                    {
                        sb.Append("\x1b[");
                        sb.Append(y + 1);
                        sb.Append(';');
                        sb.Append(x + 1);
                        sb.Append('H');
                        needMove = false;
                    }

                    if (curFg != newCell.Foreground || curBg != newCell.Background)
                    {
                        curFg = newCell.Foreground;
                        curBg = newCell.Background;
                        sb.Append("\x1b[");
                        sb.Append(AnsiFg(curFg));
                        sb.Append(';');
                        sb.Append(AnsiBg(curBg));
                        sb.Append('m');
                    }

                    sb.Append(newCell.Character);
                    _front[y, x] = newCell;
                }
            }

            // Сброс цветов
            sb.Append("\x1b[0m");

            // Курсор — в том же Write, без промежуточного Flush
            if (showCursorAt.HasValue)
            {
                var (cx, cy) = showCursorAt.Value;
                sb.Append("\x1b[");
                sb.Append(cy + 1);
                sb.Append(';');
                sb.Append(cx + 1);
                sb.Append('H');
                sb.Append("\x1b[?25h"); // показать курсор
            }
            // Если курсор не нужен — он уже скрыт (мы скрыли в начале)

            // Одна атомарная запись — нет мерцания
            _writer.Write(sb.ToString());
            _writer.Flush();

            _forceFullRedraw = false;
        }

        public void ForceFullRedraw() => _forceFullRedraw = true;

        public void InvalidateFront()
        {
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    _front[y, x] = new ConsoleCell('\0');
            _forceFullRedraw = true;
        }

        private static int AnsiFg(ConsoleColor c) => c switch
        {
            ConsoleColor.Black => 30,
            ConsoleColor.DarkRed => 31,
            ConsoleColor.DarkGreen => 32,
            ConsoleColor.DarkYellow => 33,
            ConsoleColor.DarkBlue => 34,
            ConsoleColor.DarkMagenta => 35,
            ConsoleColor.DarkCyan => 36,
            ConsoleColor.Gray => 37,
            ConsoleColor.DarkGray => 90,
            ConsoleColor.Red => 91,
            ConsoleColor.Green => 92,
            ConsoleColor.Yellow => 93,
            ConsoleColor.Blue => 94,
            ConsoleColor.Magenta => 95,
            ConsoleColor.Cyan => 96,
            ConsoleColor.White => 97,
            _ => 37
        };

        private static int AnsiBg(ConsoleColor c) => AnsiFg(c) + 10;
    }
}