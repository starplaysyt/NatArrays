using System.Runtime.InteropServices;

namespace NatLib.Arrays;

/// <summary>
/// Implementation of a simple array in unmanaged memory.
/// </summary>
/// <typeparam name="T"> Unmanaged datatype</typeparam>

public sealed class PointerArray<T> : IDisposable where T : unmanaged
{
    internal unsafe T* Pointer = null;

    /// <summary>
    /// Returns length of the array.
    /// </summary>
    public int Length { get; private set; } 

    /// <summary>
    /// Returns whether array is allocated or not.
    /// </summary>
    public bool IsAllocated { get { unsafe { return Pointer != null; } } } 
    
    /// <summary>
    /// Gets length of the array in byte representation.
    /// </summary>
    public ulong ByteLength { get { unsafe { return (ulong)(sizeof(T) * Length); } } }

    /// <summary>
    /// Returns reference on the element in array.
    /// </summary>
    /// <param name="i"> Index in the array.</param>
    /// <exception cref="IndexOutOfRangeException"> Throws when index is out of range of array.</exception>
    public T this[int i] { get { unsafe {
        if (!IsAllocated) throw new InvalidOperationException("Array is not allocated.");
        if (i < 0 || i >= Length) throw new IndexOutOfRangeException($"Index {i} is out of range {Length}.");
        
        return Pointer[i];
    } } }

    #region ** Unsafe Region **
    /// <summary>
    /// Returns ref by i to element without any checks.
    /// </summary>
    public ref T UnsafeRef(int i) { unsafe { return ref Pointer[i]; } }
    /// <summary>
    /// Returns element by i without any checks.
    /// </summary>
    public T UnsafeGet(int i) { unsafe { return Pointer[i]; } }
    /// <summary>
    /// Sets element by i without any checks.
    /// </summary>
    public void UnsafeSet(int i, T value) { unsafe { Pointer[i] = value; } }
    #endregion

    /// <summary>
    /// Gets <c>Span&lt;T&gt;</c> for an allocated pointer array. 
    /// </summary>
    /// <exception cref="InvalidOperationException"> Throws when tried to get span from a deallocated array.</exception>
    /// <remarks> Unsafe when use reallocate/deallocate/allocate when PointerSpan is in the context.</remarks>
    public Span<T> AsSpan() { unsafe
    {
        return IsAllocated
            ? new Span<T>(Pointer, Length)
            : throw new InvalidOperationException("Array is not allocated.");
    } }

    /// <summary>
    /// Allocates memory for an array.
    /// </summary>
    /// <param name="length"> Length of an array.</param>
    /// <param name="initMode"></param>
    /// <exception cref="InvalidOperationException">Throws when memory already allocated.</exception>
    /// <exception cref="ArgumentException">Throws when length is negative or zero.</exception>
    /// <exception cref="OutOfMemoryException">Throws when allocating memory in bytes failed.</exception>
    public void Allocate(int length, InitializationMode initMode = InitializationMode.Nothing)
    {
        if (IsAllocated) throw new InvalidOperationException("Array is already allocated.");
        if (length <= 0) throw new ArgumentException("Length must be positive.");

        unsafe
        {
            var ptr = (T*)NativeMemory.Alloc((nuint)length, (nuint)sizeof(T));
            Pointer = ptr;
            Length = length;

            switch (initMode)
            {
                case InitializationMode.Nothing:
                    return;
                case InitializationMode.Zeroes:
                    NativeMemory.Clear(Pointer, (nuint)ByteLength);
                    return;
                case InitializationMode.Constructor:
                    new Span<T>(Pointer, Length).Fill(new T());
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(initMode), initMode, null);
            }
        }
    }

    /// <summary>
    /// Resizes array to the desired length.
    /// </summary>
    /// <param name="length"> New length of an array.</param>
    /// <param name="initMode"></param>
    /// <exception cref="ArgumentException"> Throws when got length is negative or zero.</exception>
    /// <exception cref="InvalidOperationException"> Throws when an array is not allocated.</exception>
    /// <exception cref="OutOfMemoryException"> Throws when reallocating memory in bytes failed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"> Throws when given irregular InitializationMode. </exception>
    public void Resize(int length, InitializationMode initMode = InitializationMode.Nothing)
    {
        if (!IsAllocated) throw new InvalidOperationException("Array is not allocated."); // State checking first.
        if (length == Length) return; // When nothing changed - do nothing.
        if (length <= 0) throw new ArgumentException("Length must be positive.");
        
        unsafe
        {
            var oldLength = Length;
            var ptr = (T*)NativeMemory.Realloc(Pointer, (nuint)(length * sizeof(T)));
            Pointer = ptr;
            Length = length;


            if (length <= Length) return; // No initialization need to be done.
            
            switch (initMode)
            {
                case InitializationMode.Nothing:
                    return;
                case InitializationMode.Zeroes:
                    var byteDiff = (nuint)((length - Length) * sizeof(T));
                    NativeMemory.Clear(&Pointer[Length], byteDiff);
                    return;
                case InitializationMode.Constructor:
                    new Span<T>(Pointer, Length)[oldLength..].Fill(new T());
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(initMode), initMode, null);
            }
        }
    }

    /// <summary>
    /// Allocates an unmanaged array with elements from the managed array.
    /// </summary>
    /// <param name="array"> Managed array</param>
    /// <exception cref="InvalidOperationException"> Throws when the array is already allocated. </exception>
    public void FromManaged(T[] array)
    {
        if (IsAllocated) throw new InvalidOperationException("Array is already allocated.");

        unsafe
        {
            var ptr = (T*)NativeMemory.Alloc((nuint)array.Length, (nuint)sizeof(T));
            Pointer = ptr;
            Length = array.Length;
            
            array.AsSpan().CopyTo(new Span<T>(Pointer, Length));
        }
    }
    
    /// <summary>
    /// Returns a managed array from this unmanaged array.
    /// </summary>
    /// <returns> New managed array, what is not connected with this array. </returns>
    /// <exception cref="InvalidOperationException"> Throws when the array is not allocated</exception>
    public T[] ToManaged()
    {
        if (!IsAllocated) throw new InvalidOperationException("Array is not allocated.");

        unsafe
        {
            return new Span<T>(Pointer, Length).ToArray();
        }
    }

    /// <summary>
    /// Deallocates all memory used by this array.
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
    
    /// <summary>
    /// Calls <c>Deallocate()</c> and do <c>SuppressFinalize(this)</c>
    /// </summary>
    public void Dispose()
    {
        Deallocate();
        GC.SuppressFinalize(this);
    }

    ~PointerArray() => Dispose();
}