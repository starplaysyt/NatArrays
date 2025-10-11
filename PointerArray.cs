using System.Runtime.InteropServices;

namespace NatArrays;

/// <summary>
/// Implementation of an array in unmanaged memory.
/// </summary>
/// <typeparam name="T">unmanaged datatype</typeparam>
public sealed class PointerArray<T> : IDisposable where T : unmanaged
{
    internal unsafe T* Pointer = null;

    public int Length { get; private set; } = 0;

    public bool IsAllocated { get { unsafe { return Pointer != null; } } } 
    public ulong ByteLength { get { unsafe { return (ulong)(sizeof(T) * Length); } } }

    /// <summary>
    /// Gets an object by reference from an array by integer index
    /// </summary>
    /// <param name="index">index of an object</param>
    /// <exception cref="IndexOutOfRangeException">index is out of range of array</exception>
    public ref T this[int index] { get { unsafe {
        if (index >= 0 && index < Length) return ref Pointer[index];
        throw new IndexOutOfRangeException($"Index {index} is out of range {Length}.");
    } } }
    
    /// <summary>
    /// Reallocates memory for an already created array with setting new array size
    /// </summary>
    /// <param name="length">new length of an array</param>
    /// <exception cref="ArgumentException">Throws when got length is negative or zero.</exception>
    /// <exception cref="InvalidOperationException">Throws when an array is not allocated.</exception>
    /// <exception cref="OutOfMemoryException">Throws when reallocating memory in bytes failed.</exception>
    public void Reallocate(int length)
    {
        if (length <= 0) throw new ArgumentException("Length must be positive.");
        if (!IsAllocated) throw new InvalidOperationException("Array is not allocated.");

        unsafe
        {
            var newPtr = (T*)NativeMemory.Realloc(Pointer, (UIntPtr)(length * sizeof(T)));
            if (newPtr == null)
                throw new OutOfMemoryException($"Failed to reallocate {ByteLength} bytes.");
            
            Pointer = newPtr;
            Length = length;
        }
    }
    
    /// <summary>
    /// Allocates memory for an array.
    /// </summary>
    /// <param name="length">length of an array</param>
    /// <exception cref="InvalidOperationException">Throws when memory already allocated.</exception>
    /// <exception cref="ArgumentException">Throws when length is negative or zero.</exception>
    /// <exception cref="OutOfMemoryException">Throws when allocating memory in bytes failed.</exception>
    public void Allocate(int length)
    {
        if (length <= 0) throw new ArgumentException("Length must be positive.");
        if (IsAllocated) throw new InvalidOperationException("Array is already allocated.");

        unsafe
        {
            Length = length;
            Pointer = (T*)NativeMemory.Alloc((UIntPtr)length, (UIntPtr)sizeof(T));
            if (Pointer == null)
                throw new OutOfMemoryException($"Failed to allocate {ByteLength} bytes.");
        }
    }
    
    /// <summary>
    /// Allocates memory for an array and fills it with constructor values. Suitable
    /// </summary>
    /// <param name="length">length of an array</param>
    /// <exception cref="InvalidOperationException">Throws when memory already allocated.</exception>
    /// <exception cref="ArgumentException">Throws when length is negative or zero.</exception>
    /// <exception cref="OutOfMemoryException">Throws when allocating memory in bytes failed.</exception>
    public void AllocateInitialize(int length)
    {
        Allocate(length);
        
        unsafe
        {
            for (int i = 0; i < Length; i++)
                Pointer[i] = new T();
        }
    }

    /// <summary>
    /// Reallocates memory for an already allocated array with setting new array size
    /// </summary>
    /// <param name="length">new length of the array</param>
    public void ReallocateInitialize(int length)
    {
        var prevLength = Length;
        
        Reallocate(length);
        
        unsafe
        {
            for (int i = prevLength; i < Length; i++)
                Pointer[i] = new T();
        }
    }
    
    /// <summary>
    /// Gets pointer span for an allocated pointer array. Unsafe when use reallocate/deallocate/allocate when PointerSpan is in context
    /// </summary>
    /// <exception cref="InvalidOperationException">Throws when tried to get span from a deallocated array</exception>
    public PointerSpan<T> GetSpan()
    {
        if (!IsAllocated) throw new InvalidOperationException("Tried to get span from a deallocated array.");
        unsafe
        {
            return new PointerSpan<T>(this);
        }
    }

    /// <summary>
    /// Deallocates all memory used by this array
    /// </summary>
    public void Deallocate()
    {
        if (!IsAllocated) return;
        
        unsafe
        {
            NativeMemory.Free(Pointer);
            Pointer = null;
            Length = 0;
        }
    }

    ~PointerArray() => Dispose();
    
    public void Dispose()
    {
        Deallocate();
        GC.SuppressFinalize(this);
    }
}