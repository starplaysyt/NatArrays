using System.Runtime.InteropServices;
using System.Text;

namespace NatLib.BufConsole;

public static class WindowsAnsi
{
    private const int STD_INPUT_HANDLE = -10;
    private const int STD_OUTPUT_HANDLE = -11;

    private const uint ENABLE_PROCESSED_OUTPUT = 0x0001;
    private const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

    private const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;

    public static bool TryEnable()
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (!OperatingSystem.IsWindows())
            return true;

        if (Console.IsOutputRedirected)
            return false;

        IntPtr hOut = GetStdHandle(STD_OUTPUT_HANDLE);
        if (hOut == IntPtr.Zero || hOut == new IntPtr(-1))
            return false;

        if (!GetConsoleMode(hOut, out uint outMode))
            return false;

        outMode |= ENABLE_PROCESSED_OUTPUT;
        outMode |= ENABLE_WRAP_AT_EOL_OUTPUT;
        outMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        outMode |= DISABLE_NEWLINE_AUTO_RETURN;

        if (!SetConsoleMode(hOut, outMode))
            return false;

        // --- INPUT ---
        // Убеждаемся что VT input ВЫКЛЮЧЕН
        if (!Console.IsInputRedirected)
        {
            IntPtr hIn = GetStdHandle(STD_INPUT_HANDLE);
            if (hIn != IntPtr.Zero && hIn != new IntPtr(-1))
            {
                if (GetConsoleMode(hIn, out uint inMode))
                {
                    // Убрать VT input если он случайно включён
                    inMode &= ~0x0200u; // ~ENABLE_VIRTUAL_TERMINAL_INPUT
                    SetConsoleMode(hIn, inMode);
                }
            }
        }

        return true;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
}