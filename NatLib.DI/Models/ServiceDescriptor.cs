using NatLib.DI.Enums;
using IServiceProvider = NatLib.DI.Interfaces.IServiceProvider;

namespace NatLib.DI.Models;

public class ServiceDescriptor
{
    public Type ServiceType { get; }

    public Type? ImplementationType { get; }

    public Func<IServiceProvider, object>? Factory { get; }

    public object? Instance { get; }

    public ServiceLifetime Lifetime { get; }

    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        ImplementationType = implementationType ??
                             throw new ArgumentNullException(nameof(implementationType));
        Lifetime = lifetime;
    }

    public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory,
        ServiceLifetime lifetime)
    {
        ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        Lifetime = lifetime;
    }

    public ServiceDescriptor(Type serviceType, object instance)
    {
        ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        Lifetime = ServiceLifetime.Singleton;
    }
}