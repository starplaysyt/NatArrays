using System.Collections.Concurrent;
using System.Reflection;
using NatLib.DI.Enums;
using NatLib.DI.Exceptions;
using NatLib.DI.Interfaces;
using NatLib.DI.Models;
using IServiceProvider = NatLib.DI.Interfaces.IServiceProvider;

namespace NatLib.DI.Core;

public class ServiceProvider : IServiceProvider, IDisposable
    {
        private readonly List<ServiceDescriptor> _descriptors;
        
        private readonly ConcurrentDictionary<Type, object> _singletonInstances;
        
        private readonly ConcurrentDictionary<Type, object> _scopedInstances;
        
        private readonly ServiceProvider? _root;
        
        private readonly List<IDisposable> _disposables = new();
        private readonly Lock _disposeLock = new();
        
        private readonly AsyncLocal<HashSet<Type>> _resolveStack = new();
        
        private bool _disposed;
        
        // Root-provider constructor
        public ServiceProvider(List<ServiceDescriptor> descriptors)
        {
            _descriptors = descriptors ?? throw new ArgumentNullException(nameof(descriptors));
            _singletonInstances = new ConcurrentDictionary<Type, object>();
            _scopedInstances = new ConcurrentDictionary<Type, object>();
            _root = null;
            
            _singletonInstances[typeof(IServiceProvider)] = this;
        }
        
        // Scoped-provider constructor
        private ServiceProvider(List<ServiceDescriptor> descriptors, ServiceProvider root)
        {
            _descriptors = descriptors;
            _singletonInstances = root._singletonInstances;
            _scopedInstances = new ConcurrentDictionary<Type, object>();
            _root = root;
            
            _scopedInstances[typeof(IServiceProvider)] = this;
        }

        public object? GetService(Type serviceType)
        {
            ThrowIfDisposed();
            
            try
            {
                return Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public object GetRequiredService(Type serviceType)
        {
            ThrowIfDisposed();
            
            var service = Resolve(serviceType);
            if (service == null)
            {
                throw new DependencyResolutionException(serviceType,
                    $"No service of type '{serviceType.FullName}' has been registered.");
            }
            return service;
        }

        public T? GetService<T>() where T : class
        {
            return GetService(typeof(T)) as T;
        }

        public T GetRequiredService<T>() where T : class
        {
            return (T)GetRequiredService(typeof(T));
        }

        public IServiceScope CreateScope()
        {
            ThrowIfDisposed();
            
            var rootProvider = _root ?? this;
            var scopedProvider = new ServiceProvider(_descriptors, rootProvider);
            return new ServiceScope(scopedProvider);
        }

        private object? Resolve(Type serviceType)
        {
            if (serviceType.IsGenericType && 
                serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return ResolveEnumerable(serviceType);
            }
            
            var descriptor = _descriptors.LastOrDefault(d => d.ServiceType == serviceType);
            if (descriptor == null)
            {
                return null;
            }

            return ResolveDescriptor(descriptor);
        }

        private object ResolveDescriptor(ServiceDescriptor descriptor)
        {
            switch (descriptor.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    return _singletonInstances.GetOrAdd(descriptor.ServiceType, _ =>
                    {
                        var instance = CreateInstance(descriptor);
                        TrackDisposable(instance, isRoot: true);
                        return instance;
                    });

                case ServiceLifetime.Scoped:
                    if (_root == null)
                    {
                    }
                    return _scopedInstances.GetOrAdd(descriptor.ServiceType, _ =>
                    {
                        var instance = CreateInstance(descriptor);
                        TrackDisposable(instance, isRoot: false);
                        return instance;
                    });

                case ServiceLifetime.Transient:
                    var transientInstance = CreateInstance(descriptor);
                    TrackDisposable(transientInstance, isRoot: false);
                    return transientInstance;

                default:
                    throw new DependencyResolutionException(descriptor.ServiceType,
                        $"Unknown lifetime: {descriptor.Lifetime}");
            }
        }

        private object CreateInstance(ServiceDescriptor descriptor)
        {
            if (descriptor.Instance != null)
                return descriptor.Instance;
            
            if (descriptor.Factory != null)
                return descriptor.Factory(this);
            
            if (descriptor.ImplementationType == null)
                throw new DependencyResolutionException(descriptor.ServiceType,
                    $"No implementation type specified for '{descriptor.ServiceType.FullName}'.");

            return ConstructInstance(descriptor.ImplementationType);
        }

        private object ConstructInstance(Type implementationType)
        {
            var stack = _resolveStack.Value ??= [];
            if (!stack.Add(implementationType))
            {
                var chain = string.Join(" -> ", stack.Select(t => t.Name)) + " -> " + implementationType.Name;
                throw new DependencyResolutionException(implementationType,
                    $"Circular dependency detected: {chain}");
            }

            try
            {
                // Sorting services by constructor parameters count
                var constructors = implementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .OrderByDescending(c => c.GetParameters().Length)
                    .ToArray();

                if (constructors.Length == 0)
                    throw new DependencyResolutionException(implementationType,
                        $"No public constructors found for type '{implementationType.FullName}'.");

                foreach (var constructor in constructors)
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new object[parameters.Length];
                    var canResolveAll = true;

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var paramType = parameters[i].ParameterType;
                        var resolved = Resolve(paramType);

                        if (resolved == null)
                        {
                            if (parameters[i].HasDefaultValue)
                                parameterInstances[i] = parameters[i].DefaultValue!;
                            else
                            {
                                canResolveAll = false;
                                break;
                            }
                        }
                        else
                            parameterInstances[i] = resolved;
                    }

                    if (canResolveAll)
                        return constructor.Invoke(parameterInstances);
                }
                
                var bestCtor = constructors[0];
                var unresolvedParams = bestCtor.GetParameters()
                    .Where(p => Resolve(p.ParameterType) == null && !p.HasDefaultValue)
                    .Select(p => $"{p.ParameterType.Name} {p.Name}");

                throw new DependencyResolutionException(implementationType,
                    $"Unable to resolve type '{implementationType.FullName}'. " +
                    $"Unresolved parameters: {string.Join(", ", unresolvedParams)}");
            }
            finally
            {
                stack.Remove(implementationType);
            }
        }

        private object ResolveEnumerable(Type enumerableType)
        {
            var itemType = enumerableType.GetGenericArguments()[0];
            var descriptors = _descriptors.Where(d => d.ServiceType == itemType).ToList();
            
            var array = Array.CreateInstance(itemType, descriptors.Count);
            for (int i = 0; i < descriptors.Count; i++)
            {
                array.SetValue(ResolveDescriptor(descriptors[i]), i);
            }
            
            return array;
        }

        private void TrackDisposable(object instance, bool isRoot)
        {
            if (instance is IDisposable disposable)
            {
                lock (_disposeLock)
                {
                    _disposables.Add(disposable);
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            lock (_disposeLock)
            {
                for (int i = _disposables.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        _disposables[i].Dispose();
                    }
                    catch
                    {
                        // Suppress Dispose exception
                    }
                }
                _disposables.Clear();
            }

            _scopedInstances.Clear();
            
            // Clearing singleton cache only if its root provider
            if (_root == null)
            {
                _singletonInstances.Clear();
            }
            
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (!_disposed) return;
            throw new ObjectDisposedException(nameof(ServiceProvider));
        }
    }