using NatLib.Core.Interfaces;

namespace NatLib.UniConsole.Arguments;

public struct RequestElementArgs<T>
{
    public string MainMessage { get; set; }
    public bool Quitable { get; set; }
    public Func<T, bool> ValidationDelegate { get; set; }
    public Action<T> ValueSetterDelegate { get; set; }
    public string RetryMessage { get; set; }
    public IStringPresenter ReferencePresenter { get; set; }
    public bool ReferenceNumeration { get; set; }
}