using System.Collections;
using NatLib.DI.Interfaces;
using NatLib.DI.Models;
using IServiceProvider = NatLib.DI.Interfaces.IServiceProvider;

namespace NatLib.DI.Core;

public class ServiceCollection : IServiceCollection
{
    private readonly List<ServiceDescriptor> _descriptors = [];

    public ServiceDescriptor this[int index]
    {
        get => _descriptors[index];
        set => _descriptors[index] = value;
    }

    public int Count => _descriptors.Count;
    public bool IsReadOnly => false;

    public void Add(ServiceDescriptor item) => _descriptors.Add(item);
    public void Clear() => _descriptors.Clear();
    public bool Contains(ServiceDescriptor item) => _descriptors.Contains(item);
    public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => _descriptors.CopyTo(array, arrayIndex);
    public IEnumerator<ServiceDescriptor> GetEnumerator() => _descriptors.GetEnumerator();
    public int IndexOf(ServiceDescriptor item) => _descriptors.IndexOf(item);
    public void Insert(int index, ServiceDescriptor item) => _descriptors.Insert(index, item);
    public bool Remove(ServiceDescriptor item) => _descriptors.Remove(item);
    public void RemoveAt(int index) => _descriptors.RemoveAt(index);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IServiceProvider BuildServiceProvider()
    {
        return new ServiceProvider(_descriptors.ToList());
    }
}