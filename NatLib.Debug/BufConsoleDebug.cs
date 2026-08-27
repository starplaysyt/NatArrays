using NatLib.BufConsole;

namespace NatLib.Debug;

public class BufConsoleDebug
{
    public static void Run()
    {
        // Включаем VT mode на Windows
        WindowsAnsi.TryEnable();

        using var bc = new BufferedConsole();
        bc.StartInput();

        bc.Input.KeyPressed += (s, e) =>
        {
            if (e.KeyInfo.Key == ConsoleKey.Escape)
                Environment.Exit(0);
        };

        // Начальная отрисовка
        bc.Draw(s =>
        {
            s.Clear(ConsoleColor.DarkBlue);
            s.DrawRect(0, 0, bc.Width, bc.InputLineY, ConsoleColor.Yellow, ConsoleColor.DarkBlue);

            string title = " Buffered Console ";
            s.DrawString(bc.Width / 2 - title.Length / 2, 0, title,
                ConsoleColor.White, ConsoleColor.DarkMagenta);

            s.DrawString(2, 2, "Клавиши:", ConsoleColor.White, ConsoleColor.DarkBlue);
            s.DrawString(4, 3, "← → Home End    - перемещение по строке", ConsoleColor.Gray, ConsoleColor.DarkBlue);
            s.DrawString(4, 4, "↑ ↓              - история команд", ConsoleColor.Gray, ConsoleColor.DarkBlue);
            s.DrawString(4, 5, "Ctrl+← Ctrl+→   - прыжок по словам", ConsoleColor.Gray, ConsoleColor.DarkBlue);
            s.DrawString(4, 6, "Ctrl+U           - очистить строку", ConsoleColor.Gray, ConsoleColor.DarkBlue);
            s.DrawString(4, 7, "Ctrl+K           - удалить до конца", ConsoleColor.Gray, ConsoleColor.DarkBlue);
            s.DrawString(4, 8, "Delete           - удалить символ", ConsoleColor.Gray, ConsoleColor.DarkBlue);
            s.DrawString(4, 9, "ESC              - выход", ConsoleColor.Gray, ConsoleColor.DarkBlue);

            s.DrawString(2, 11, "Прогресс:", ConsoleColor.White, ConsoleColor.DarkBlue);
            s.DrawProgressBar(12, 11, 30, 0.0);
        });
        bc.Flush();

        // Фоновая анимация — курсор НЕ мигает
        var animThread = new Thread(() =>
        {
            for (double p = 0; p <= 1.01; p += 0.01)
            {
                bc.Draw(s => s.DrawProgressBar(12, 11, 30, p));
                bc.Flush();
                Thread.Sleep(50);
            }
        }) { IsBackground = true };
        animThread.Start();

        // Цикл ввода
        int msgLine = 13;
        while (true)
        {
            string input = bc.ReadLine("> ");

            if (string.IsNullOrEmpty(input)) continue;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            if (input == "clear")
            {
                bc.Draw(s => s.FillRect(1, 13, bc.Width - 2, bc.InputLineY - 14,
                    ' ', bg: ConsoleColor.DarkBlue));
                msgLine = 13;
                bc.Flush();
                continue;
            }

            bc.Draw(s =>
            {
                if (msgLine >= bc.InputLineY - 1)
                {
                    s.FillRect(1, 13, bc.Width - 2, bc.InputLineY - 14,
                        ' ', bg: ConsoleColor.DarkBlue);
                    msgLine = 13;
                }
                s.DrawString(2, msgLine, $">>> {input}",
                    ConsoleColor.Yellow, ConsoleColor.DarkBlue);
                msgLine++;
            });
            bc.Flush();
        }
    }
}