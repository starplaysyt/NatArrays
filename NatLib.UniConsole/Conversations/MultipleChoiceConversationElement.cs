using NatLib.Core.Utils;
using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Graphics;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class MultipleChoiceConversationElement : IConversationElement
{
    public (int, int) EntryCursorLocation { get; set; }
    public IConversationElement? NextElement { get; set; }
    
    public IConversationElement?[] ChoicesElements;
    public string[] ChoicesTitles;
    
    public ChoiceElementArgs ChoiceArgs { get; set; }

    public void Start()
    {
        var choiceArgs = ChoiceArgs;
        EntryCursorLocation = ConsoleRenderer.GetCheckpoint();
        
        GOTO_WRITING_STARTING:
        
        ConsoleRenderer.WriteNumeratedMenu(
            choiceArgs.ChoicesTitle(), ChoicesTitles);
        
        var insertCursorPosition = ConsoleRenderer.GetCheckpoint();
        int inputChoice;
        string inputString;
        
        GOTO_INPUT_STARTING:
        
        ConsoleRenderer.Write(">>> ");
        if (choiceArgs.UseFastInputSystem)
        {
            inputString = ConsoleRenderer.ReadCharKey().ToString();
            ConsoleRenderer.WriteLine();
        }
        else
            inputString =
                ConsoleRenderer.ReadLine();

        choiceArgs.RawUserInputProvider?.Invoke(inputString);

        if (int.TryParse(inputString, out inputChoice) && inputChoice > 0 && inputChoice <= ChoicesTitles.Length)
        {
            choiceArgs.InvocationStatusProvider?.Invoke(true);
            choiceArgs.ChoiceIndexProvider?.Invoke(inputChoice);

            if (choiceArgs.ClearBeforeChoice)
                ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);
            
            if (choiceArgs.ClearSelectionBeforeChoice)
                ConsoleRenderer.GotoCheckpoint(insertCursorPosition);
            
            ChoicesElements[inputChoice-1]?.Start();

            if (choiceArgs.CycleRequesting)
            {
                ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);
                goto GOTO_WRITING_STARTING;
            }
        }
        else
        {
            ConsoleRenderer.WriteTopBorder();
            ConsoleRenderer.WriteMessageLineSingle(choiceArgs.RetryMessage());
            ConsoleRenderer.WriteSeparator();
            ConsoleRenderer.WriteMessageLineSingle(choiceArgs.CanBeQuit ? "Press any key to retry, or Space key to leave..." : "Press any key to retry...");
            ConsoleRenderer.WriteBottomBorder();

            var key = ConsoleRenderer.ReadConsoleKey(true);
            
            if (key != ConsoleKey.Spacebar || !choiceArgs.CanBeQuit)
            {
                ConsoleRenderer.GotoCheckpoint(insertCursorPosition);
                goto GOTO_INPUT_STARTING;
            }
            
            choiceArgs.InvocationStatusProvider?.Invoke(false);
            
            if (choiceArgs.ClearBeforeQuit)
                ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);
            
            choiceArgs.QuitElement?.Start();
            
            if (choiceArgs.QuitCreatesExecutionBranch)
            {
                if (choiceArgs.ClearAtTheEnd)
                    ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);
                return;
            }
        }
        
        if (choiceArgs.ClearBeforeNext)
            ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);

        NextElement?.Start();

        if (choiceArgs.ClearAtTheEnd)
            ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);
    }

    public MultipleChoiceConversationElement(
        (string Title, IConversationElement? Choice)[] choices,
        ChoiceElementArgs choiceArgs,
        IConversationElement? nextElement)
    {
        ChoiceArgs = choiceArgs;
        NextElement = nextElement;
        ChoicesTitles = new string[choices.Length];
        ChoicesElements = new IConversationElement?[choices.Length];

        for (var i = 0; i < choices.Length; i++)
        {
            var current = choices[i];
            ChoicesTitles[i] = current.Title;
            ChoicesElements[i] = current.Choice;
        }
    }
}