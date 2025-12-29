using NatLib.Core.Utils;
using NatLib.UniConsole.Graphics;

namespace NatLib.UniConsole.Conversations;

public class GeneralConversations
{
    public static readonly GeneralConversations Instance = new();

    public GeneralConversations()
    {
        
    }
    
    public int RequestIntEnter(string message, int preferableWidth)
    {
        GOTO_START:
        ConsoleRenderer.ShowMessageBox(message);
        if (!int.TryParse(Console.ReadLine(), out var choice))
        {
            ConsoleRenderer.ShowMessageBox("Incorrect input. Press any key to try again...");
            Console.ReadKey(true);
            goto GOTO_START;
        }
        
        return choice;
    }

    public object? RequestObjectEnter(string message, Type type)
    {
        GOTO_START:
        Console.Write(message);
        var info = Console.ReadLine() ?? string.Empty;
        
        if (!ReflectionConverter.TryConvert(info, type, out var retValue))
        {
            ConsoleRenderer.ShowMessageBoxMultiline([
                "Exception! Given data was not in a correct format.",
                "REMEMBER! Not all types of data can be changed that way.",
                "Only INumber(?), Enum, IParsable and IConvertible are supported by default.",
                "Press Space to try again, or any button to skip..."
            ]);
            
            var ch = Console.ReadKey(true);
            
            if (ch.Key == ConsoleKey.Spacebar)
                goto GOTO_START; 
        }
        
        return retValue;
    }

    public void ShowEnumReference(Type enumType, int preferableWidth)
    {
        if (!enumType.IsEnum) return;
        
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageInBounds($"Reference for enum {enumType.Name}");
        ConsoleRenderer.WriteSeparator();

        var enumNames = enumType.GetEnumNames();
        var enumValues = enumType.GetEnumValuesAsUnderlyingType();

        for (int i = 0; i < enumNames.Length; i++)
        {
            ConsoleRenderer.WriteMessageInBounds($"{enumNames[i]} = {enumValues.GetValue(i)?.ToString() ?? "(0)"}");
        }
        
        ConsoleRenderer.WriteBottomBorder();
    }
}