namespace NatLib.DI.Interfaces;

public interface IServiceScope : IDisposable
{
    public IServiceProvider ServiceProvider { get; }
}