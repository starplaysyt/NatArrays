using NatLib.Core.Interfaces;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Arguments;

public struct RequestElementArgs<T>
{
    public Func<string> Message { get; set; } = () => "Insert value:";
    public Func<string> RetryMessage { get; set; } = () => "Exception. Unsuitable value.";

    /// <summary>
    /// Defines either user can quit from inputting circle, or not.
    /// </summary>
    public bool CanBeQuit { get; set; } = false;

    /// <summary>
    /// Defines a step of value validation, receive T as converted value, and should return validation result.
    /// It is invoked,  
    /// </summary>
    public Func<T, bool>? ValidationDelegate { get; set; } = null;

    /// <summary>
    /// Provides a binding for value
    /// </summary>
    public Action<T>? ConvertedValueBinding { get; set; } = null;

    /// <summary>
    /// Returns user input as it is. Invokes right after user input.
    /// </summary>
    public Action<string>? RawValueBinding { get; set; } = null;

    /// <summary>
    /// Provides status information about execution - <br/>
    /// either enter succeed (true), or user quit (false)
    /// </summary>
    public Action<bool>? InvocationStatusProvider { get; set; } = null;

    /// <summary>
    /// Defines reference, that will be shown as description of input format, available range, ect.
    /// </summary>
    public IStringPresenter? ReferencePresenter { get; set; } = null;

    /// <summary>
    /// Defines either element should be removed from screen before calling next element, or not.
    /// </summary>
    public bool ClearBeforeNext { get; set; } = false;

    /// <summary>
    /// Defines either element should be removed from screen before calling quit element, or not.
    /// </summary>
    public bool ClearBeforeQuit { get; set; } = false;

    /// <summary>
    /// Defines either element should be removed from screen after it's child execution is over, or not.
    /// </summary>
    public bool ClearAtTheEnd { get; set; } = true;
    
    /// <summary>
    /// Defines either QuitElement should be performed instead of next element when quit requested, or not.
    /// </summary>
    public bool QuitCreatesExecutionBranch { get; set; } = false;

    /// <summary>
    /// Defines an element that will be executed when user requested quit.
    /// </summary>
    public IConversationElement? QuitElement { get; set; } = null;

    public RequestElementArgs()
    {

    }
}