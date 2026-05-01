using NatLib.UniConsole.Arguments;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Builders;

public static class ChoiceElementBuilder
{
    public static T InsetValue<T>(T value, Action<T> insetAction)
    {
        insetAction(value);
        return value;
    }
    
    extension(ChoiceElementArgs elementArgs)
    {
        public ChoiceElementArgs WithTitle(Func<string> title) =>
            InsetValue(elementArgs, elementArgs => elementArgs.OnTitle = title);
        
        public ChoiceElementArgs WithRetryMessage(Func<string, string> title) =>
            InsetValue(elementArgs, elementArgs => elementArgs.RetryMessage = title);

        public ChoiceElementArgs AllowQuit() =>
            InsetValue(elementArgs, elementArgs => elementArgs.CanQuit = true);
        
        public ChoiceElementArgs UseBranchQuit() =>
            InsetValue(elementArgs, elementArgs => elementArgs.NewBranchOnQuit = true);

        public ChoiceElementArgs WithQuitElement(IConversationElement element) =>
            InsetValue(elementArgs, elementArgs => elementArgs.QuitElement = element);

        public ChoiceElementArgs WithStatusObserver(Action<ExecutionStatus> statusObserver) =>
            InsetValue(elementArgs, elementArgs => elementArgs.StatusObserver = statusObserver);
        
        public ChoiceElementArgs WithChoiceObserver(Action<int> choiceObserver) =>
            InsetValue(elementArgs, elementArgs => elementArgs.ChoiceObserver = choiceObserver);
        
        public ChoiceElementArgs WithRawInputObserver(Action<string> rawInputObserver) =>
            InsetValue(elementArgs, elementArgs => elementArgs.RawInputObserver = rawInputObserver);
        
        public ChoiceElementArgs ClearBeforeNext() =>
            InsetValue(elementArgs, elementArgs => elementArgs.ClearBeforeNext = true);

        public ChoiceElementArgs ClearBeforeChoice() =>
            InsetValue(elementArgs, elementArgs => elementArgs.ClearBeforeChoice = true);
        
        public ChoiceElementArgs ClearBeforeQuit() =>
            InsetValue(elementArgs, elementArgs => elementArgs.ClearBeforeQuit = true);

        public ChoiceElementArgs UseFastInputSystem() =>
            InsetValue(elementArgs, elementArgs => elementArgs.UseFastInputSystem = true);
        
        public ChoiceElementArgs UseCycleRequesting() =>
            InsetValue(elementArgs, elementArgs => elementArgs.CycleRequesting = true);
    }
}