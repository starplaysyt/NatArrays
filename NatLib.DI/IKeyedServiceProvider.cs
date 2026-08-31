namespace NatLib.DI;

public interface IKeyedServiceProvider : IServiceProvider
{
    object? GetKeyedService(Type serviceType, object key);
}