using System.Globalization;
using NatLib.Core.Utils;
using NatLib.UniConsole.Graphics;

namespace NatLib.UniConsole.Utils;

public static class RequestingUtils
{
    public static string[] REQ_EXCEPT_MESSAGE =
    [
        "Exception! Given data was not in a correct format.",
        "Press any key to try again..."
    ];

    public static string[] REQ_RANGE_EXCEPT_MESSAGE =
    [
        "Exception! Given data was not in a correct format",
        "or out of range.",
        "Press any key to try again..."
    ];

    public static string[] REQ_QUIT_EXCEPT_MESSAGE =
    [
        "Exception! Given data was not in a correct format.",
        "Press Space to try again, or any key to exit..."
    ];

    public static string[] REQ_RANGE_QUIT_EXCEPT_MESSAGE =
    [
        "Exception! Given data was not in a correct format",
        "or out of range.",
        "Press Space to try again, or any key to exit..."
    ];

    public static string[] REQ_REFLECTION_QUIT_EXCEPT_MESSAGE =
    [
        "Exception! Given data was not in a correct format.",
        "REMEMBER! Not all types of data can be changed that way.",
        "Only INumber(?), Enum, IParsable and IConvertible are supported by default.",
        "Press Space to try again, or any button to skip..."
    ];

    public static string REQ_ENUM_REFERENCE_HEADER = "Reference for enum";


    /// <summary>
    /// Performs continuous requestion T value from reading user input.
    /// </summary>
    public static T RequestEnter<T>(string message) where T : IParsable<T>
    {
        var gotValue = default(T);

        DelegateUtils.ValidateCycle(
            () =>
            {
                ConsoleRenderer.ShowMessageBox(message);
                ShowEnumReference(typeof(T));
            },
            () =>
            {
                Console.Write(">>> ");
                var res = T.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var retValue);
                gotValue = retValue;
                return res;
            },
            () =>
            {
                ConsoleRenderer.ShowMessageBoxMultiline(REQ_EXCEPT_MESSAGE);
                Console.ReadKey(true);
            }
        );

        return gotValue;
    }

    public static T RequestEnterRange<T>(string message, T min, T max) where T : IParsable<T>, IComparable<T>
    {
        var gotValue = default(T);

        DelegateUtils.ValidateCycle(
            () =>
            {
                ConsoleRenderer.ShowMessageBox(message);
                ShowEnumReference(typeof(T));
            },
            () =>
            {
                Console.Write(">>> ");
                var res = T.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var retValue);

                gotValue = retValue;

                return res && min.CompareTo(retValue) <= 0 && max.CompareTo(retValue) >= 0;
            },
            () =>
            {
                ConsoleRenderer.ShowMessageBoxMultiline(
                    REQ_RANGE_EXCEPT_MESSAGE
                );
                Console.ReadKey(true);
            }
        );

        return gotValue;
    }

    public static bool RequestEnterWithExit<T>(string message, out T? value, T min, T max)
        where T : IParsable<T>, IComparable<T>
    {
        var gotValue = default(T);

        var result = DelegateUtils.ValidateCycleWithExit(
            () =>
            {
                ConsoleRenderer.ShowMessageBox(message);
                ShowEnumReference(typeof(T));
            },
            () =>
            {
                Console.Write(">>> ");
                var res = T.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var retValue);

                gotValue = retValue;

                return res && min.CompareTo(retValue) <= 0 && max.CompareTo(retValue) >= 0;
            },
            () =>
            {
                ConsoleRenderer.ShowMessageBoxMultiline(
                    REQ_QUIT_EXCEPT_MESSAGE);
                var ch = Console.ReadKey(true);
                return ch.Key == ConsoleKey.Spacebar;
            }
        );

        value = result ? gotValue : default;

        return result;
    }

    public static bool RequestEnterRangeWithExit<T>(string message, out T? value) where T : IParsable<T>
    {
        var gotValue = default(T);

        var result = DelegateUtils.ValidateCycleWithExit(
            () =>
            {
                ConsoleRenderer.ShowMessageBox(message);
                ShowEnumReference(typeof(T));
            },
            () =>
            {
                Console.Write(">>> ");
                var res = T.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var retValue);
                gotValue = retValue;
                return res;
            },
            () =>
            {
                ConsoleRenderer.ShowMessageBoxMultiline(
                    REQ_RANGE_QUIT_EXCEPT_MESSAGE);
                var ch = Console.ReadKey(true);
                return ch.Key == ConsoleKey.Spacebar;
            }
        );

        value = result ? gotValue : default;

        return result;
    }

    public static bool RequestEnterWithExit(string message, Type type, out object? value)
    {
        object? gotValue = null;

        var result = DelegateUtils.ValidateCycleWithExit(
            () =>
            {
                ConsoleRenderer.ShowMessageBox(message);
                ShowEnumReference(type);
            },
            () =>
            {
                Console.Write(">>> ");
                var res = ConvertingUtils.TryReflectionConvert(Console.ReadLine(), type, out var retValue);
                gotValue = retValue;
                return res;
            },
            () =>
            {
                ConsoleRenderer.ShowMessageBoxMultiline(REQ_REFLECTION_QUIT_EXCEPT_MESSAGE);

                var ch = Console.ReadKey(true);

                return ch.Key == ConsoleKey.Spacebar;
            }
        );

        value = result ? gotValue : null;

        return result;
    }

    public static void ShowEnumReference(Type enumType)
    {
        if (!enumType.IsEnum) return;

        var items = enumType.GetEnumNames()
            .Zip(enumType.GetEnumValues().Cast<object>(),
                (n, v) => $"{n} = {Convert.ToInt64(v)}")
            .ToArray();

        ConsoleRenderer.ShowMenuItems($"{REQ_ENUM_REFERENCE_HEADER} {enumType.Name}:",
            items);
    }
}