using System.Reflection;
using System.Globalization;

namespace NatLib.Core.Utils;

/// <summary>
/// Implements converter that converts string to given type when type is referenced from IParsable.
/// </summary>
public static class ReflectionConverter
{
    public static bool TryConvert(string? input, Type targetType, out object? value)
    {
        value = null;
        
        // string
        if (targetType == typeof(string))
        {
            value = input;
            return true;
        }

        // Nullable<T>?
        var underlying = Nullable.GetUnderlyingType(targetType);
        if (underlying != null)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                value = null;
                return true;
            }

            targetType = underlying;
        }

        // enum
        if (targetType.IsEnum)
        {
            // if (long.TryParse(input, out var n))
            // {
            //     value = Enum.ToObject(targetType, n);
            //     return true;
            // }

            if (Enum.TryParse(targetType, input, ignoreCase: true, out var ev))
            {
                value = ev;
                return true;
            }

            return false;
        }

        try
        {
            value = Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            return TryIParsable(input, targetType, out value);
        }
    }
    
    private static bool TryIParsable(string? input, Type targetType, out object? value)
    {
        value = null;

        var iParsableInterface = targetType
            .GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IParsable<>));

        if (iParsableInterface == null)
            return false;

        var tryParseMethod = targetType.GetMethod(
            "TryParse",
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            new[]
            {
                typeof(string),
                typeof(IFormatProvider),
                targetType.MakeByRefType()
            },
            modifiers: null);

        if (tryParseMethod == null)
            return false;

        object?[] args =
        {
            input,
            CultureInfo.InvariantCulture,
            null
        };

        var success = (bool)tryParseMethod.Invoke(null, args)!;
        value = success ? args[2] : null;
        return success;
    }
}