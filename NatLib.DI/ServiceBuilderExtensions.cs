namespace NatLib.DI;

public static class ServiceBuilderExtensions
{
    extension(ServiceBuilder builder)
    {
        public ServiceBuilder AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return builder.AddService(ServiceDescriptor.FromType(
                typeof(TService), typeof(TImplementation), ServiceLifetimeType.Singleton));
        }

        public ServiceBuilder AddSingleton<TService>()
            where TService : class
        {
            return builder.AddService(ServiceDescriptor.FromType(
                typeof(TService), typeof(TService), ServiceLifetimeType.Singleton));
        }

        public ServiceBuilder AddSingleton<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            return builder.AddService(ServiceDescriptor.FromFactory(
                typeof(TService), factory, ServiceLifetimeType.Singleton));
        }

        public ServiceBuilder AddSingleton(Type serviceType, Type implementationType)
        {
            return builder.AddService(ServiceDescriptor.FromType(
                serviceType, implementationType, ServiceLifetimeType.Singleton));
        }

        public ServiceBuilder AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return builder.AddService(ServiceDescriptor.FromType(
                typeof(TService), typeof(TImplementation), ServiceLifetimeType.Scoped));
        }

        public ServiceBuilder AddScoped<TService>()
            where TService : class
        {
            return builder.AddService(ServiceDescriptor.FromType(
                typeof(TService), typeof(TService), ServiceLifetimeType.Scoped));
        }

        public ServiceBuilder AddScoped<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            return builder.AddService(ServiceDescriptor.FromFactory(
                typeof(TService), factory, ServiceLifetimeType.Scoped));
        }

        public ServiceBuilder AddScoped(Type serviceType, Type implementationType)
        {
            return builder.AddService(ServiceDescriptor.FromType(
                serviceType, implementationType, ServiceLifetimeType.Scoped));
        }

        public ServiceBuilder AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return builder.AddService(ServiceDescriptor.FromType(
                typeof(TService), typeof(TImplementation), ServiceLifetimeType.Transient));
        }

        public ServiceBuilder AddTransient<TService>()
            where TService : class
        {
            return builder.AddService(ServiceDescriptor.FromType(
                typeof(TService), typeof(TService), ServiceLifetimeType.Transient));
        }

        public ServiceBuilder AddTransient<TService>(Func<IServiceProvider, TService> factory)
            where TService : class
        {
            return builder.AddService(ServiceDescriptor.FromFactory(
                typeof(TService), factory, ServiceLifetimeType.Transient));
        }

        public ServiceBuilder AddTransient(Type serviceType, Type implementationType)
        {
            return builder.AddService(ServiceDescriptor.FromType(
                serviceType, implementationType, ServiceLifetimeType.Transient));
        }

        public ServiceBuilder AddKeyedSingleton<TService, TImplementation>(object key)
            where TService : class
            where TImplementation : class, TService
        {
            return builder.AddService(ServiceDescriptor.FromType(
                typeof(TService), typeof(TImplementation), ServiceLifetimeType.Singleton, key));
        }

        public ServiceBuilder AddKeyedSingleton<TService>(object key, Func<IServiceProvider, TService> factory)
            where TService : class
        {
            return builder.AddService(ServiceDescriptor.FromFactory(
                typeof(TService), factory, ServiceLifetimeType.Singleton, key));
        }

        public ServiceBuilder AddKeyedScoped<TService, TImplementation>(object key)
            where TService : class
            where TImplementation : class, TService
        {
            return builder.AddService(ServiceDescriptor.FromType(
                typeof(TService), typeof(TImplementation), ServiceLifetimeType.Scoped, key));
        }

        public ServiceBuilder AddKeyedScoped<TService>(object key, Func<IServiceProvider, TService> factory)
            where TService : class
        {
            return builder.AddService(ServiceDescriptor.FromFactory(
                typeof(TService), factory, ServiceLifetimeType.Scoped, key));
        }

        public ServiceBuilder AddKeyedTransient<TService, TImplementation>(object key)
            where TService : class
            where TImplementation : class, TService
        {
            return builder.AddService(ServiceDescriptor.FromType(
                typeof(TService), typeof(TImplementation), ServiceLifetimeType.Transient, key));
        }

        public ServiceBuilder AddKeyedTransient<TService>(object key, Func<IServiceProvider, TService> factory)
            where TService : class
        {
            return builder.AddService(ServiceDescriptor.FromFactory(
                typeof(TService), factory, ServiceLifetimeType.Transient, key));
        }
    }
}