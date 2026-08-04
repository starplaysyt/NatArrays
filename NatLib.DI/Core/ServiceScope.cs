using NatLib.DI.Interfaces;
using IServiceProvider = NatLib.DI.Interfaces.IServiceProvider;

namespace NatLib.DI.Core;

public class ServiceScope : IServiceScope
{
    private readonly ServiceProvider _provider;

    public IServiceProvider ServiceProvider => _provider;

    public ServiceScope(ServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public void Dispose()
    {
        _provider.Dispose();
    }
}