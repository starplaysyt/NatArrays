using NatLib.Core.Interfaces;
using NatLib.UniConsole.Interfaces;

namespace NatLib.UniConsole.Arguments;

public struct RequestElementArgs<T>
{
    public Func<string> Message { get; set; } = () => "Insert int value:";
    public Func<string> RetryMessage { get; set; } = () => "Exception. Unsuitable value.";
    
    /// <summary>
    /// Defines either user can quit from value insertion. 
    /// </summary>
    public bool CanBeQuit { get; set; } = false;
    
    /// <summary>
    /// Provides a way of value validation.
    /// Invokes when parsing succeed, right after. <br/>
    /// When is not set, validation is skipped.
    /// </summary>
    public Func<T, bool>? ValidationDelegate { get; set; } = null;
    
    /// <summary>
    /// Returns user input, converted to desired type. Invokes after value validation.
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
    public IStringPresenter? ReferencePresenter { get; set; } = null;
    
    /// <summary>
    /// Defines behavior before calling the next item,<br/>
    /// true - clear this element.  <br/>
    /// false - leave this element on screen.
    /// </summary>
    public bool DistinctAfterUsage { get; set; } = false;
    /// <summary>
    /// Defines behavior before calling quit element,
    /// true - clear this element.  <br/>
    /// false - leave this element on screen.
    /// </summary>
    public bool DistinctAfterQuit { get; set; } = false;
    public bool ClearAtTheEnd { get; set; } = true;
    /// <summary>
    /// In normal quit terms, QuitElement is called first, but after its' execution
    /// NextElement will be called.
    /// To start completely new execution branch - set this property to true.
    /// This will prevent starting NextElement, and will create a new execution branch.
    /// </summary>
    public bool QuitElementOverridesNextElement { get; set; } = false;
    public IConversationElement? QuitElement { get; set; } = null;

    public RequestElementArgs()
    {
        
    }
}