using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NatLib.Arrays;

namespace NatLib.ECS;

public class ComponentStorage<T> : IDisposable where T : unmanaged
{
    private unsafe int* _sparse; 
    private unsafe int* _dense;

    private unsafe T* _data;

    private const int DefaultCapacity = 64;
    private const int InvalidIndex = -1;

    public ComponentStorage(int capacity = DefaultCapacity)
    {
        
    }


    public void Dispose()
    {
        // TODO release managed resources here
    }
}