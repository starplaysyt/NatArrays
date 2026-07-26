using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatLib.Arrays;

public class SparseSet<T> : IDisposable where T : unmanaged
{
    private unsafe int* _sparse;
    private int _sparseCapacity;
    
    private unsafe T* _data;
    private unsafe int* _denseEntities;
    private int _denseCount;
    private int _denseCapacity;
    
    private const int DefaultSparseCapacity = 256;
    private const int DefaultDenseCapacity = 64;
    private const int InvalidIndex = -1;

    public SparseSet(int sparseCapacity = DefaultSparseCapacity,
        int denseCapacity = DefaultDenseCapacity)
    {
        unsafe
        {
            _sparseCapacity = sparseCapacity;
            _denseCapacity = denseCapacity;
            _denseCount = 0;
            
            _sparse = AllocArray<int>(sparseCapacity);
            _data = AllocArray<T>(denseCapacity);
            _denseEntities = AllocArray<int>(denseCapacity);
            
            for (int i = 0; i < sparseCapacity; i++) 
                _sparse[i] = InvalidIndex;
        }
    }

    public void Dispose()
    {
        unsafe
        {
            if (_sparse == null && _data == null && _denseEntities == null) return;
            
            NativeMemory.Free(_sparse);
            NativeMemory.Free(_data);
            NativeMemory.Free(_denseEntities);

            _sparse = null;
            _data = null;
            _denseEntities = null;

            GC.SuppressFinalize(this);
        }
    }

    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _denseCount;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has(int entityId)
    {
        if ((uint)entityId >= (uint)_sparseCapacity)
            return false;

        unsafe
        {
            // (uint)(-1) = 4294967295, что всегда >= _denseCount
            return (uint)_sparse[entityId] < (uint)_denseCount;
        }
    }

    public ref T this[int entityId]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            unsafe
            {
                return ref _data[_sparse[entityId]];
            }
        }
    }
    
    public ref T Set(int entityId, T component = default)
    {
        if (entityId >= _sparseCapacity)
            GrowSparse(entityId + 1);
        
        if (_denseCount >= _denseCapacity)
            GrowDense(_denseCapacity * 2);

        var denseIndex = _denseCount;

        unsafe
        {
            _sparse[entityId] = denseIndex;
            _denseEntities[denseIndex] = entityId;
            _data[denseIndex] = component;
            
            _denseCount++;

            return ref _data[denseIndex];
        }
    }
    
    public bool Remove(int entityId)
    {
        if (!Has(entityId))
            return false;

        unsafe
        {
            var removedDense = _sparse[entityId];
            var lastDense = _denseCount - 1;

            if (removedDense != lastDense)
            {
                var lastEntity = _denseEntities[lastDense];

                _data[removedDense] = _data[lastDense];
                _denseEntities[removedDense] = lastEntity;
                
                _sparse[lastEntity] = removedDense;
            }
            
            _sparse[entityId] = InvalidIndex;
            _denseCount--;
        }
        
        return true;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetOrAdd(int entityId)
    {
        if (Has(entityId))
            return ref this[entityId];
        return ref Set(entityId);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsDataSpan()
    {
        unsafe
        {
            return new Span<T>(_data, _denseCount);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<int> AsEntitySpan()
    {
        unsafe
        {
            return new Span<int>(_denseEntities, _denseCount);
        }
    }
    
    public unsafe T* RawData
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data;
    }

    public unsafe int* RawEntities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _denseEntities;
    }

    private void GrowSparse(int minCapacity)
    {
        unsafe
        {
            var newCap = _sparseCapacity;
            while (newCap < minCapacity)
                newCap *= 2;

            _sparse = ReallocArray(_sparse, newCap);
            
            for (var i = _sparseCapacity; i < newCap; i++)
                _sparse[i] = InvalidIndex;

            _sparseCapacity = newCap;
        }
    }

    private void GrowDense(int newCapacity)
    {
        unsafe
        {
            _data = ReallocArray(_data, newCapacity);
            _denseEntities = ReallocArray(_denseEntities, newCapacity);
            _denseCapacity = newCapacity;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe TItem* AllocArray<TItem>(int count) where TItem : unmanaged
    {
        var ptr = NativeMemory.AllocZeroed(
            (nuint)count, (nuint)sizeof(TItem));
        return (TItem*)ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe TItem* ReallocArray<TItem>(
        TItem* old, int newCount) where TItem : unmanaged
    {
        var ptr = NativeMemory.Realloc(
            old, (nuint)newCount * (nuint)sizeof(TItem));
        return (TItem*)ptr;
    }
    
    ~SparseSet()
    {
        Dispose();
    }
}