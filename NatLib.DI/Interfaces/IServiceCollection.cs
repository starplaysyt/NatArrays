using NatLib.DI.Models;

namespace NatLib.DI.Interfaces;

public interface IServiceCollection : IList<ServiceDescriptor>
{
    public IServiceProvider BuildServiceProvider();
}