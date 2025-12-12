namespace NatLib.Arrays;

/// <summary>
/// Interface to communicate with PointerArray a bit faster (WIP). Does not own the pointer directly,
/// and do not perform memory management.
/// </summary>
/// <typeparam name="T"> Unmanaged datatype</typeparam>
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
    /// Gets an object by reference from an array by integer index
    /// </summary>
    /// <param name="index"> Index of an object</param>
    /// <exception cref="IndexOutOfRangeException"> Index is out of range of array</exception>
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