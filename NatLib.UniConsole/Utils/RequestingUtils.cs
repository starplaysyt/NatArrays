using System.Globalization;
using NatLib.Core.Utils;
using NatLib.UniConsole.Graphics;

namespace NatLib.UniConsole.Utils;

public class RequestingUtils
{
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
                ConsoleRenderer.ShowMessageBoxMultiline(
                    ["Exception! Given data was not in a correct format.",
                        "Press any key to try again..."]
                );
                Console.ReadKey(true);
            }
        );
        
        return gotValue;
    }

    public static bool RequestEnterWithExit<T>(string message, out T? value) where T : IParsable<T>
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
                [
                    "Exception! Given data was not in a correct format.",
                    "Press any key to try again..."
                ]);
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
                ConsoleRenderer.ShowMessageBoxMultiline([
                    "Exception! Given data was not in a correct format.",
                    "REMEMBER! Not all types of data can be changed that way.",
                    "Only INumber(?), Enum, IParsable and IConvertible are supported by default.",
                    "Press Space to try again, or any button to skip..."
                ]);
                
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
        
        ConsoleRenderer.ShowMenuItems($"Reference for enum {enumType.Name}:",
            items);
        
        // ConsoleRenderer.WriteTopBorder();
        // ConsoleRenderer.WriteMessageInBounds($"Reference for enum {enumType.Name}");
        // ConsoleRenderer.WriteSeparator();
        //
        //
        //
        // for (int i = 0; i < enumNames.Length; i++)
        // {
        //     ConsoleRenderer.WriteMessageInBounds();
        // }
        //
        // ConsoleRenderer.WriteBottomBorder();
    }
}