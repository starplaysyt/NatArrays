using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatArrays;

public sealed class PointerMatrix<T> where T : unmanaged
{
    internal unsafe T* Pointer = null;
    public int Width = 0;
    public int Height = 0;

    /// <summary>
    /// Returns whether array is allocated or not
    /// </summary>
    public bool IsAllocated
    {
        get
        {
            unsafe
            {
                return Pointer != null;
            }
        }
    }

    /// <summary>
    /// Gets absolute length of the array
    /// </summary>
    public int Length => Width * Height;

    /// <summary>
    /// Gets length of the array in byte representation
    /// </summary>
    public ulong ByteLength
    {
        get
        {
            unsafe
            {
                return (ulong)(sizeof(T) * Length);
            }
        }
    }

    /// <summary>
    /// Gets reference of the element in the array by its X and Y
    /// </summary>
    /// <param name="x"> Coordinate by Width</param>
    /// <param name="y"> Coordinate by Height</param>
    /// <exception cref="IndexOutOfRangeException"> Throws when given x or y is out of bounds of the array</exception>
    public ref T this[int x, int y]
    {
        get
        {
            unsafe
            {
                if ((uint)x >= (uint)Width || (uint)y >= (uint)Height)
                    throw new IndexOutOfRangeException($"Index x = {x}, y = {y} out of range.");

                return ref Pointer[y * Width + x];
            }
        }
    }

    /// <summary>
    /// Gets reference of the element in the array by its absolute index
    /// </summary>
    /// <param name="i"> Absolute element index</param>
    /// <exception cref="IndexOutOfRangeException"> Throws when given element index is out of bounds of the array</exception>
    public ref T this[int i]
    {
        get
        {
            unsafe
            {
                if ((uint)i >= (uint)(Width * Height))
                    throw new IndexOutOfRangeException($"Index i = {i} is out of range.");
                
                return ref Pointer[i];
            }
        }
    }
    
    /// <summary>
    /// Allocates memory for the array.
    /// </summary>
    /// <param name="width"> New matrix width</param>
    /// <param name="height"> New matrix height</param>
    /// <param name="allocatingType"> Defines what to do with new elements</param>
    /// <exception cref="InvalidOperationException"> Throws when an array is allocated already.</exception>
    /// <exception cref="ArgumentException"> Throws when width or height is negative or zero.</exception>
    /// <exception cref="OutOfMemoryException"> Throws when reallocating memory in bytes failed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"> Throws when given irregular AllocatingType </exception>
    public void Allocate(int width, int height, AllocatingType allocatingType)
    {
        if (IsAllocated) throw new InvalidOperationException("Array is already allocated.");
        if (width <= 0) throw new ArgumentException("Width must be positive.");
        if (height <= 0) throw new ArgumentException("Height must be positive.");

        unsafe
        {
            Pointer = (T*)NativeMemory.Alloc((nuint)(width * height), (nuint)sizeof(T));
            if (Pointer == null)
                throw new OutOfMemoryException($"Failed to allocate {ByteLength} bytes.");


            switch (allocatingType)
            {
                case AllocatingType.Nothing:
                    break;
                case AllocatingType.Zeroes:
                    // Clears all given memory
                    NativeMemory.Clear(Pointer, (nuint)ByteLength);
                    break;
                case AllocatingType.Constructor:
                    // Calls constructor for every object
                    for (var i = 0; i < width * height; i++)
                        Pointer[i] = new T();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(allocatingType), allocatingType, null);
            }
        }

        Width = width;
        Height = height;
    }
    
    /// <summary>
    /// Returns unsafe reference on the element in the array by its X and Y
    /// </summary>
    /// <param name="x"> Coordinate by width</param>
    /// <param name="y"> Coordinate by height</param>
    /// <returns> Reference on the element in matrix</returns>
    /// <remarks> This function <b>directly access to pointer index. Suppresses all checkups of bounds</b>,
    /// unlike this[int x, int y] operator. <br/> Use it like that to achieve maximum performance. 
    /// <code>
    /// for (int y = 0; y &lt; matrix.Height; y++)
    /// {
    ///     for (int x = 0; x &lt; matrix.Width; x++)
    ///     {
    ///         ref var value = ref matrix.GetRefUnsafe(x, y);
    ///         value = default; // or any operation
    ///     }
    /// }
    /// </code>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetRefUnsafe(int x, int y)
    {
        unsafe
        {
            return ref Pointer[y * Width + x];
        }
    }
    
    /// <summary>
    /// Returns unsafe reference on the element in matrix.
    /// </summary>
    /// <param name="i"> Index in the array</param>
    /// <returns> Reference on the element in matrix</returns>
    /// <remarks> This function <b>directly access to pointer index. Suppresses all checkups of bounds</b>,
    /// unlike this[int i] operator. <br/> Use it like that to achieve maximum performance. 
    /// <code>
    /// For (int i = 0; i &lt; matrix.Length; y++)
    /// {
    ///     ref var value = ref matrix.GetRefUnsafe(i);
    ///     value = default; // or any operation
    /// }
    /// </code>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetRefUnsafe(int i)
    {
        unsafe
        {
            return ref Pointer[i];
        }
    }

    /// <summary>
    /// Resizes a matrix array with saving matrix alignment.
    /// </summary>
    /// <param name="newWidth"> New matrix width</param>
    /// <param name="newHeight"> New matrix height</param>
    /// <param name="allocatingType"> Defines what to do with new elements</param>
    /// <exception cref="ArgumentException"> Throws when width or height is negative or zero.</exception>
    /// <exception cref="InvalidOperationException"> Throws when an array is not allocated.</exception>
    /// <exception cref="OutOfMemoryException"> Throws when reallocating memory in bytes failed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"> Throws when given irregular AllocatingType </exception>
    /// <remarks> Neither call constructor, nor setting data to zeroes - only reallocates memory.</remarks>
    public void Resize(int newWidth, int newHeight, AllocatingType allocatingType = AllocatingType.Nothing)
    {
        if (!IsAllocated) throw new InvalidOperationException("Array is not allocated.");
        if (newWidth <= 0) throw new ArgumentException("Width must be positive.");
        if (newHeight <= 0) throw new ArgumentException("Height must be positive.");
        if (newWidth == Width && newHeight == Height) return; //nothing changed - do nothing

        unsafe
        {
            var oldWidth = Width;
            var oldHeight = Height;
            var copyWidth = Math.Min(oldWidth, newWidth); //how much data we need to copy further
            var copyHeight = Math.Min(oldHeight, newHeight);
            var reallocateBytes = (nuint)(newWidth * newHeight * sizeof(T));
            var copyBytes = (nuint)(copyWidth * sizeof(T));

            // performing reallocation firstly when array length increasing
            if (newWidth * newHeight > oldWidth * oldHeight)
            {
                var localPtr = (T*)NativeMemory.Realloc(Pointer, reallocateBytes);
                if (localPtr == null)
                    throw new OutOfMemoryException($"Failed to allocate {ByteLength} bytes.");
                Pointer = localPtr;
            }

            // moving data by lines
            if (newWidth > oldWidth)
            {
                // going from the end for not rewriting necessary memory
                for (int y = oldHeight - 1; y >= 0; y--)
                {
                    var src = &Pointer[oldWidth * y]; //that is more efficient and safe
                    var dst = &Pointer[newWidth * y];

                    Buffer.MemoryCopy(src, dst, copyBytes, copyBytes);
                }
            }
            else if (newWidth < oldWidth)
            {
                for (int y = 0; y < copyHeight; y++)
                {
                    var src = &Pointer[oldWidth * y];
                    var dst = &Pointer[newWidth * y];

                    Buffer.MemoryCopy(src, dst, copyBytes, copyBytes);
                }
            }

            // performing reallocation at the end when array length decreasing
            if (newWidth * newHeight < oldWidth * oldHeight)
            {
                var localPtr = (T*)NativeMemory.Realloc(Pointer, (nuint)(newWidth * newHeight * sizeof(T)));
                if (localPtr == null)
                    throw new OutOfMemoryException($"Failed to allocate {ByteLength} bytes.");
                Pointer = localPtr;
            }

            switch (allocatingType)
            {
                case AllocatingType.Nothing:
                    break;
                case AllocatingType.Zeroes:
                    
                    // Clear extended parts of existing rows
                    for (var y = 0; y < Math.Min(oldHeight, newHeight); y++)
                    {
                        var clearCount = newWidth - oldWidth;
                        if (clearCount > 0)
                        {
                            NativeMemory.Clear(&Pointer[y * newWidth + oldWidth],
                                (nuint)(clearCount * sizeof(T)));
                        }
                    }

                    // Clear fully new rows if height increased
                    if (newHeight > oldHeight)
                    {
                        NativeMemory.Clear(&Pointer[oldHeight * newWidth],
                            (nuint)((newHeight - oldHeight) * newWidth * sizeof(T)));
                    }
                    break;
                case AllocatingType.Constructor:
                    for (var y = 0; y < newHeight; y++)
                    {
                        // Start creating from oldWidth (when need to add some objects to the line)
                        // When it is a new line - filling all lines
                        var startX = y < oldHeight ? oldWidth : 0; 
                
                        for (var x = startX; x < newWidth; x++)
                        {
                            Pointer[y * newWidth + x] = new T();
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(allocatingType), allocatingType, null);
            }

            Width = newWidth;
            Height = newHeight;
        }
    }
}