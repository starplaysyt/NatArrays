namespace NatLib.BufConsole;

public class DrawingSurface
    {
        private readonly ScreenBuffer _buffer;

        public int Width => _buffer.Width;
        public int Height => _buffer.Height;

        public ConsoleColor DefaultForeground { get; set; } = ConsoleColor.Gray;
        public ConsoleColor DefaultBackground { get; set; } = ConsoleColor.Black;

        public DrawingSurface(ScreenBuffer buffer)
        {
            _buffer = buffer;
        }

        public void Clear(ConsoleColor? bg = null)
        {
            _buffer.Clear(bg ?? DefaultBackground);
        }

        /// <summary>Один символ</summary>
        public void PutChar(int x, int y, char ch, ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            _buffer.SetCell(x, y, ch, fg ?? DefaultForeground, bg ?? DefaultBackground);
        }

        /// <summary>Строка текста</summary>
        public void DrawString(int x, int y, string text, ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            if (text == null || y < 0 || y >= Height) return;
            var fgc = fg ?? DefaultForeground;
            var bgc = bg ?? DefaultBackground;
            for (int i = 0; i < text.Length; i++)
            {
                int px = x + i;
                if (px < 0) continue;
                if (px >= Width) break;
                _buffer.SetCell(px, y, text[i], fgc, bgc);
            }
        }

        /// <summary>Горизонтальная линия</summary>
        public void DrawHLine(int x, int y, int length, char ch = '─', ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            for (int i = 0; i < length; i++)
                PutChar(x + i, y, ch, fg, bg);
        }

        /// <summary>Вертикальная линия</summary>
        public void DrawVLine(int x, int y, int length, char ch = '│', ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            for (int i = 0; i < length; i++)
                PutChar(x, y + i, ch, fg, bg);
        }

        /// <summary>Прямоугольник (рамка)</summary>
        public void DrawRect(int x, int y, int width, int height, ConsoleColor? fg = null, ConsoleColor? bg = null,
            char horizontal = '─', char vertical = '│',
            char topLeft = '┌', char topRight = '┐',
            char bottomLeft = '└', char bottomRight = '┘')
        {
            if (width < 2 || height < 2) return;

            PutChar(x, y, topLeft, fg, bg);
            PutChar(x + width - 1, y, topRight, fg, bg);
            PutChar(x, y + height - 1, bottomLeft, fg, bg);
            PutChar(x + width - 1, y + height - 1, bottomRight, fg, bg);

            DrawHLine(x + 1, y, width - 2, horizontal, fg, bg);
            DrawHLine(x + 1, y + height - 1, width - 2, horizontal, fg, bg);
            DrawVLine(x, y + 1, height - 2, vertical, fg, bg);
            DrawVLine(x + width - 1, y + 1, height - 2, vertical, fg, bg);
        }

        /// <summary>Заполненный прямоугольник</summary>
        public void FillRect(int x, int y, int width, int height, char ch = ' ', ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            for (int dy = 0; dy < height; dy++)
                for (int dx = 0; dx < width; dx++)
                    PutChar(x + dx, y + dy, ch, fg, bg);
        }

        /// <summary>Многострочный текст</summary>
        public void DrawTextBlock(int x, int y, string[] lines, ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            for (int i = 0; i < lines.Length; i++)
                DrawString(x, y + i, lines[i], fg, bg);
        }

        /// <summary>Прогресс-бар</summary>
        public void DrawProgressBar(int x, int y, int width, double progress, ConsoleColor filledFg = ConsoleColor.Green, ConsoleColor emptyFg = ConsoleColor.DarkGray, ConsoleColor? bg = null)
        {
            progress = Math.Clamp(progress, 0.0, 1.0);
            int filled = (int)(width * progress);
            for (int i = 0; i < width; i++)
            {
                if (i < filled)
                    PutChar(x + i, y, '█', filledFg, bg);
                else
                    PutChar(x + i, y, '░', emptyFg, bg);
            }
        }

        /// <summary>Линия Брезенхэма</summary>
        public void DrawLine(int x0, int y0, int x1, int y1, char ch = '*', ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while (true)
            {
                PutChar(x0, y0, ch, fg, bg);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        /// <summary>Эллипс Брезенхэма</summary>
        public void DrawEllipse(int cx, int cy, int rx, int ry, char ch = '*', ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            int x = 0, y = ry;
            long rx2 = (long)rx * rx, ry2 = (long)ry * ry;
            long err = ry2 - rx2 * ry + rx2 / 4;

            while (ry2 * x <= rx2 * y)
            {
                PlotEllipsePoints(cx, cy, x, y, ch, fg, bg);
                if (err < 0)
                {
                    x++;
                    err += 2 * ry2 * x + ry2;
                }
                else
                {
                    x++;
                    y--;
                    err += 2 * ry2 * x - 2 * rx2 * y + ry2;
                }
            }

            err = ry2 * (x * 2 + 1) * (x * 2 + 1) / 4 + rx2 * ((long)y - 1) * (y - 1) - rx2 * ry2;
            while (y >= 0)
            {
                PlotEllipsePoints(cx, cy, x, y, ch, fg, bg);
                if (err > 0)
                {
                    y--;
                    err -= 2 * rx2 * y + rx2;
                }
                else
                {
                    y--;
                    x++;
                    err += 2 * ry2 * x - 2 * rx2 * y;
                }
            }
        }

        private void PlotEllipsePoints(int cx, int cy, int x, int y, char ch, ConsoleColor? fg, ConsoleColor? bg)
        {
            PutChar(cx + x, cy + y, ch, fg, bg);
            PutChar(cx - x, cy + y, ch, fg, bg);
            PutChar(cx + x, cy - y, ch, fg, bg);
            PutChar(cx - x, cy - y, ch, fg, bg);
        }
    }