using NatLib.DI.Enums;
using NatLib.DI.Interfaces;
using NatLib.DI.Models;
using IServiceProvider = NatLib.DI.Interfaces.IServiceProvider;

namespace NatLib.DI.Extensions;

public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddTransient<TService, TImplementation>()
                where TService : class
                where TImplementation : class, TService
            {
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
                return services;
            }

            public IServiceCollection AddTransient<TService>()
                where TService : class
            {
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TService), ServiceLifetime.Transient));
                return services;
            }

            public IServiceCollection AddTransient<TService>(Func<IServiceProvider, TService> factory)
                where TService : class
            {
                services.Add(new ServiceDescriptor(typeof(TService), factory, ServiceLifetime.Transient));
                return services;
            }

            public IServiceCollection AddTransient(Type serviceType, 
                Type implementationType)
            {
                services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
                return services;
            }

            public IServiceCollection AddScoped<TService, TImplementation>()
                where TService : class
                where TImplementation : class, TService
            {
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped));
                return services;
            }

            public IServiceCollection AddScoped<TService>()
                where TService : class
            {
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TService), ServiceLifetime.Scoped));
                return services;
            }

            public IServiceCollection AddScoped<TService>(Func<IServiceProvider, TService> factory)
                where TService : class
            {
                services.Add(new ServiceDescriptor(typeof(TService), factory, ServiceLifetime.Scoped));
                return services;
            }

            public IServiceCollection AddScoped(Type serviceType, 
                Type implementationType)
            {
                services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Scoped));
                return services;
            }

            public IServiceCollection AddSingleton<TService, TImplementation>()
                where TService : class
                where TImplementation : class, TService
            {
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
                return services;
            }

            public IServiceCollection AddSingleton<TService>()
                where TService : class
            {
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TService), ServiceLifetime.Singleton));
                return services;
            }

            public IServiceCollection AddSingleton<TService>(Func<IServiceProvider, TService> factory)
                where TService : class
            {
                services.Add(new ServiceDescriptor(typeof(TService), factory, ServiceLifetime.Singleton));
                return services;
            }

            public IServiceCollection AddSingleton<TService>(TService instance)
                where TService : class
            {
                services.Add(new ServiceDescriptor(typeof(TService), instance));
                return services;
            }

            public IServiceCollection AddSingleton(Type serviceType, 
                Type implementationType)
            {
                services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
                return services;
            }

            public IServiceCollection TryAddTransient<TService, TImplementation>()
                where TService : class
                where TImplementation : class, TService
            {
                if (services.All(d => d.ServiceType != typeof(TService)))
                {
                    services.AddTransient<TService, TImplementation>();
                }
                return services;
            }

            public IServiceCollection TryAddScoped<TService, TImplementation>()
                where TService : class
                where TImplementation : class, TService
            {
                if (services.All(d => d.ServiceType != typeof(TService)))
                {
                    services.AddScoped<TService, TImplementation>();
                }
                return services;
            }

            public IServiceCollection TryAddSingleton<TService, TImplementation>()
                where TService : class
                where TImplementation : class, TService
            {
                if (services.All(d => d.ServiceType != typeof(TService)))
                {
                    services.AddSingleton<TService, TImplementation>();
                }
                return services;
            }

            public IServiceCollection Replace<TService, TImplementation>(ServiceLifetime lifetime)
                where TService : class
                where TImplementation : class, TService
            {
                var existing = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
                if (existing != null)
                {
                    services.Remove(existing);
                }
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));
                return services;
            }
        }
    }
    
