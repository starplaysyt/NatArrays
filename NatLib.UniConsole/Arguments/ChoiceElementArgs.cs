using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Arguments;

public struct ChoiceElementArgs
{
    public Func<string> ChoicesTitle = () => "Choose a value";
    
    public Func<string> RetryMessage { get; set; } = () => "Exception. Unsupported choice.";
    
    /// <summary>
    /// 
    /// </summary>
    public bool CanBeQuit { get; set; } = true;
    
    /// <summary>
    /// Defines an element that will be executed when user requested quit.
    /// </summary>
    public IConversationElement? QuitElement { get; set; } = null;
    
    /// <summary>
    /// Defines either QuitElement should be performed instead of next element when quit requested, or not.
    /// </summary>
    public bool QuitCreatesExecutionBranch { get; set; } = false;
    
    /// <summary>
    /// Provides status information about execution - <br/>
    /// either enter succeed (true), or user quit (false)
    /// </summary>
    public Action<bool>? InvocationStatusProvider { get; set; }
    
    /// <summary>
    /// Provides user-selected index.
    /// </summary>
    public Action<int>? ChoiceIndexProvider { get; set; }
    
    /// <summary>
    /// Returns user input as it is. Invokes right after user input.
    /// </summary>
    public Action<string>? RawUserInputProvider { get; set; }
    
    /// <summary>
    /// Defines either element should be removed from screen before calling next element, or not.
    /// </summary>
    public bool ClearBeforeNext { get; set; } = false;
    
    /// <summary>
    /// Defines either element should be removed from screen before calling chosen element, or not.
    /// </summary>
    public bool ClearBeforeChoice { get; set; } = false;

    /// <summary>
    /// Defines either element should be removed from screen before calling quit element, or not.
    /// </summary>
    public bool ClearBeforeQuit { get; set; } = false;

    /// <summary>
    /// Defines either element should be removed from screen after it's child execution is over, or not.
    /// </summary>
    public bool ClearAtTheEnd { get; set; } = true;
    
    /// <summary>
    /// Defines either inputting should use ReadLine or ReadKey to chose single-numbered choices.
    /// </summary>
    public bool UseFastInputSystem { get; set; } = false;
    
    /// <summary>
    /// Defines either number inputting area clears before calling choice or not.
    /// </summary>
    public bool ClearSelectionBeforeChoice { get; set; } = false;

    /// <summary>
    /// Defines either element should become a call-cycle
    /// </summary>
    public bool CycleRequesting { get; set; } = false;

    public ChoiceElementArgs()
    {
        
    }
}