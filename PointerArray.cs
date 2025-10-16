using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatArrays;

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
    public ref T this[int i] { get { unsafe {
        if (i >= 0 && i < Length) return ref Pointer[i];
        throw new IndexOutOfRangeException($"Index {i} is out of range {Length}.");
    } } }

    /// <summary>
    /// Returns unsafe reference on the element in array.
    /// </summary>
    /// <param name="i"> Index in the array.</param>
    /// <returns> Reference on the element in array.</returns>
    /// <remarks> This function <b>directly access to pointer index. Suppresses all checkups of bounds</b>,
    /// unlike this[int i] operator. <br/> 
    /// </remarks>
    /// <example>
    /// Use it like that to achieve maximum performance. 
    /// <code>
    /// for (int i = 0; i &lt; array.Length; y++)
    /// {
    ///     ref var value = ref array.GetRefUnsafe(i);
    ///     value = default; // or any operation
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetRefUnsafe(int i)
    {
        unsafe
        {
            return ref Pointer[i];
        }
    }

    /// <summary>
    /// Allocates memory for an array.
    /// </summary>
    /// <param name="length"> Length of an array.</param>
    /// <param name="initializationMode"></param>
    /// <exception cref="InvalidOperationException">Throws when memory already allocated.</exception>
    /// <exception cref="ArgumentException">Throws when length is negative or zero.</exception>
    /// <exception cref="OutOfMemoryException">Throws when allocating memory in bytes failed.</exception>
    public void Allocate(int length, InitializationMode initializationMode = InitializationMode.Nothing)
    {
        if (length <= 0) throw new ArgumentException("Length must be positive.");
        if (IsAllocated) throw new InvalidOperationException("Array is already allocated.");

        unsafe
        {
            Length = length;
            Pointer = (T*)NativeMemory.Alloc((UIntPtr)length, (UIntPtr)sizeof(T));
            if (Pointer == null)
                throw new OutOfMemoryException($"Failed to allocate {ByteLength} bytes.");

            switch (initializationMode)
            {
                case InitializationMode.Nothing:
                    return;
                case InitializationMode.Zeroes:
                    NativeMemory.Clear(Pointer, (UIntPtr)ByteLength);
                    break;
                case InitializationMode.Constructor:
                    for (var i = 0; i < Length; i++)
                        Pointer[i] = new T();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(initializationMode), initializationMode, null);
            }
        }
    }

    /// <summary>
    /// Resizes array to the desired length.
    /// </summary>
    /// <param name="length"> New length of an array.</param>
    /// <param name="initializationMode"></param>
    /// <exception cref="ArgumentException"> Throws when got length is negative or zero.</exception>
    /// <exception cref="InvalidOperationException"> Throws when an array is not allocated.</exception>
    /// <exception cref="OutOfMemoryException"> Throws when reallocating memory in bytes failed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"> Throws when given irregular InitializationMode. </exception>
    public void Resize(int length, InitializationMode initializationMode = InitializationMode.Nothing)
    {
        if (length == Length) return;
        if (!IsAllocated) throw new InvalidOperationException("Array is not allocated.");
        if (length <= 0) throw new ArgumentException("Length must be positive.");
        
        unsafe
        {
            var newPtr = (T*)NativeMemory.Realloc(Pointer, (UIntPtr)(length * sizeof(T)));
            if (newPtr == null)
                throw new OutOfMemoryException($"Failed to reallocate {ByteLength} bytes.");
            
            Pointer = newPtr;

            if (length > Length)
            {
                switch (initializationMode)
                {
                    case InitializationMode.Nothing:
                        break;
                    case InitializationMode.Zeroes:
                        var byteDiff = (nuint)((length - Length) * sizeof(T));
                        NativeMemory.Clear(&Pointer[Length], byteDiff);
                        break;
                    case InitializationMode.Constructor:
                        for (var i = Length; i < length; i++)
                            Pointer[i] = new T();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(initializationMode), initializationMode, null);
                }
            }

            Length = length;
        }
    }
    
    /// <summary>
    /// Gets pointer span for an allocated pointer array. 
    /// </summary>
    /// <exception cref="InvalidOperationException"> Throws when tried to get span from a deallocated array.</exception>
    /// <remarks> Unsafe when use reallocate/deallocate/allocate when PointerSpan is in context.</remarks>
    public PointerSpan<T> GetSpan()
    {
        if (!IsAllocated) throw new InvalidOperationException("Tried to get span from a deallocated array.");
        unsafe
        {
            return new PointerSpan<T>(this);
        }
    }

    /// <summary>
    /// Allocates an unmanaged array with elements from the managed array.
    /// </summary>
    /// <param name="array"> Managed array</param>
    /// <exception cref="InvalidOperationException"> Throws when the array is already allocated. </exception>
    public void AllocateManaged(T[] array)
    {
        if (IsAllocated) throw new InvalidOperationException("Array is already allocated.");

        unsafe
        {
            Pointer = (T*)NativeMemory.Alloc((UIntPtr)array.Length, (UIntPtr)array.Length);
        
            Length = array.Length;
            
            for (var i = 0; i < Length; i++)
            {
                Pointer[i] = array[i];
            }
        }
    }
    
    /// <summary>
    /// Returns a managed array from this unmanaged array.
    /// </summary>
    /// <returns> New managed array</returns>
    /// <exception cref="InvalidOperationException"> Throws when the array is not allocated</exception>
    public T[] ToManagedArray()
    {
        if (!IsAllocated) throw new InvalidOperationException("Array is not allocated.");
        
        var output = new T[Length];
        for (var i = 0; i < Length; i++)
        {
            output[i] = GetRefUnsafe(i);
        }
        return output;
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