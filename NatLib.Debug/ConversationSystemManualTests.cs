using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Conversations;

namespace NatLib.Debug;

public static class ConversationSystemManualTests
{
    public static void MultipleChoiceConversationTest()
    {
        var entry = new MultipleChoiceConversationElement(
            [
                (
                    Title: "title1",
                    Choice: new MessageConversationElement(
                        new MessageElementArgs
                        {
                            Message = "title1 message", WaitForUserKey = true
                        },
                        null)
                ),
                (
                    Title: "title2",
                    Choice: new MessageConversationElement(
                        new MessageElementArgs()
                        {
                            Message = "title2 message", WaitForUserKey = true
                        },
                        null)
                ),
                (
                    Title: "title3",
                    Choice: new MessageConversationElement(
                        new MessageElementArgs()
                        {
                            Message = "title3 message", WaitForUserKey = true
                        },
                        null)
                )
            ],
            new ChoiceElementArgs
            {
                UseFastInputSystem = false,
                ClearSelectionBeforeChoice = true,
                CycleRequesting = true
            },
        null);
        
        entry.Start();
    }
}