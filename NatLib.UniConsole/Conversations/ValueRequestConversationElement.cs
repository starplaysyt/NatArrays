using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Graphics;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Conversations;

public class ValueRequestConversationElement<T> : IConversationElement where T : IParsable<T>
{
    public (int, int) EntryCursorLocation { get; set; }
    public IConversationElement? NextElement { get; set; }

    public RequestElementArgs<T> RequestArgs { get; set; }

    public ValueRequestConversationElement(
        RequestElementArgs<T> requestArgs,
        IConversationElement? nextElement = null)
    {
        RequestArgs = requestArgs;
        NextElement = nextElement;
    }

    public void Start()
    {
        var reqArgs = RequestArgs;
        EntryCursorLocation = ConsoleRenderer.GetCheckpoint();
        ConsoleRenderer.WriteTopBorder();
        ConsoleRenderer.WriteMessageInBounds(reqArgs.Message());

        if (reqArgs.ReferencePresenter != null) // Reference generation
        {
            ConsoleRenderer.WriteSeparator();
            reqArgs.ReferencePresenter.PresentString();
        }

        ConsoleRenderer.WriteBottomBorder();

        var insertCursorPosition = ConsoleRenderer.GetCheckpoint();
        string inputString;

        GOTO_INPUT_STARTING:

        ConsoleRenderer.Write(">>> ");
        inputString = Console.ReadLine() ?? "";
        reqArgs.RawValueBinding?.Invoke(inputString);

        if (T.TryParse(inputString, null, out var inputParsed)
            && reqArgs.ValidationDelegate?.Invoke(inputParsed) != false)
        {
            reqArgs.InvocationStatusProvider?.Invoke(true);
            reqArgs.ConvertedValueBinding?.Invoke(inputParsed);
        }
        else
        {
            ConsoleRenderer.WriteTopBorder();
            ConsoleRenderer.WriteMessageInBounds(reqArgs.RetryMessage());
            ConsoleRenderer.WriteSeparator();

            ConsoleRenderer.WriteMessageInBounds(reqArgs.CanBeQuit ? "Press any key to retry, or Space key to leave..." : "Press any key to retry...");
            ConsoleRenderer.WriteBottomBorder();
            var key = Console.ReadKey(true).Key;

            if (key != ConsoleKey.Spacebar || !reqArgs.CanBeQuit)
            {
                ConsoleRenderer.GotoCheckpoint(insertCursorPosition);
                goto GOTO_INPUT_STARTING;
            }

            reqArgs.InvocationStatusProvider?.Invoke(false);

            if (reqArgs.DistinctAfterQuit)
                ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);

            reqArgs.QuitElement?.Start();

            if (reqArgs.QuitElementOverridesNextElement)
            {
                if (reqArgs.ClearAtTheEnd)
                    ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);
                return;
            }
        }

        if (reqArgs.DistinctAfterUsage)
            ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);

        NextElement?.Start();

        if (reqArgs.ClearAtTheEnd)
            ConsoleRenderer.GotoCheckpoint(EntryCursorLocation);
    }
}