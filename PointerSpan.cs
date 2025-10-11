namespace NatArrays;

/// <summary>
/// Interface to communicate with PointerArray a bit faster (WIP). Does not own the pointer directly,
/// and do not perform memory management, as a structure should do.
/// </summary>
/// <typeparam name="T">unmanaged datatype</typeparam>
public struct PointerSpan<T> where T : unmanaged
{
    internal unsafe T* Pointer = null;
    public int Length { get; private set; } = 0;
    
    internal PointerSpan(PointerArray<T> array)
    {
        unsafe
        {
            Pointer = array.Pointer;
            Length = array.Length;
        }
    }
    
    /// <summary>
    /// Synchronize PointerArray with PointerSpan, use after PointerArray allocate/reallocate/deallocate
    /// to avoid unsafe usage
    /// </summary>
    /// <exception cref="InvalidOperationException">Throws when trying to sync a span with a deallocated array.</exception>
    /// <param name="array">array to synchronize with span</param>
    public void SyncWithArray(PointerArray<T> array)
    {
        if (!array.IsAllocated) throw new InvalidOperationException("Tried to sync span with a deallocated array.");
        unsafe
        {
            Pointer = array.Pointer;
            Length = array.Length;
        }
    }
    
    /// <summary>
    /// Gets an object by reference from an array by integer index
    /// </summary>
    /// <param name="index">index of an object</param>
    /// <exception cref="IndexOutOfRangeException">index is out of range of array</exception>
    public ref T this[int index]
    {
        get
        {
            if (index >= Length || index < 0)
                throw new IndexOutOfRangeException();
            unsafe
            {
                return ref Pointer[index];
            }
        }
    }
}