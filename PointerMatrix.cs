using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatArrays;

/// <summary>
/// Implementation of a sequentially mapped matrix (row-major).
/// </summary>
/// <typeparam name="T"> Unmanaged datatype</typeparam>
public sealed class PointerMatrix<T> : IDisposable where T : unmanaged
{
    internal unsafe T* Pointer = null;
    
    /// <summary>
    /// Count of matrix elements by X-coordinate
    /// </summary>
    public int Width { get; private set; }
    
    /// <summary>
    /// Count of matrix elements by Y-coordinate
    /// </summary>
    public int Height  { get; private set; }

    /// <summary>
    /// Returns whether matrix is allocated or not
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
    /// Gets absolute length of the matrix
    /// </summary>
    public int Length => Width * Height;

    /// <summary>
    /// Gets length of the matrix in byte representation
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
    /// Gets reference of the element in the matrix by its X and Y
    /// </summary>
    /// <param name="x"> Coordinate by Width</param>
    /// <param name="y"> Coordinate by Height</param>
    /// <exception cref="IndexOutOfRangeException"> Throws when given x or y is out of bounds of the
    /// matrix</exception>
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
    /// Gets reference of the element in the matrix by its absolute index
    /// </summary>
    /// <param name="i"> Absolute element index</param>
    /// <exception cref="IndexOutOfRangeException"> Throws when given element index is out of bounds of the
    /// matrix</exception>
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
    /// Returns unsafe reference on the element in the matrix by its X and Y
    /// </summary>
    /// <param name="x"> Coordinate by width</param>
    /// <param name="y"> Coordinate by height</param>
    /// <returns> Reference on the element in matrix</returns>
    /// <remarks> This function <b>directly access to pointer index. Suppresses all checkups of bounds</b>,
    /// unlike this[int x, int y] operator.<br/> 
    /// </remarks>
    /// <example>
    /// Use it like that to achieve maximum performance. 
    /// <code>
    ///     for (int y = 0; y &lt; matrix.Height; y++)
    ///     {
    ///         for (int x = 0; x &lt; matrix.Width; x++)
    ///         {
    ///             ref var value = ref matrix.GetRefUnsafe(x, y);
    ///             value = default; // or any operation
    ///         }
    ///     }
    /// </code>
    /// </example>
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
    /// <param name="i"> Index in the matrix.</param>
    /// <returns> Reference on the element in matrix.</returns>
    /// <remarks> This function <b>directly access to pointer index. Suppresses all checkups of bounds.</b>,
    /// unlike this[int i] operator. <br/> 
    /// </remarks>
    /// <example>
    /// Use it like that to achieve maximum performance. 
    /// <code>
    /// for (int i = 0; i &lt; matrix.Length; y++)
    /// {
    ///     ref var value = ref matrix.GetRefUnsafe(i);
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
    /// Allocates memory for the matrix.
    /// </summary>
    /// <param name="width"> New matrix width</param>
    /// <param name="height"> New matrix height</param>
    /// <param name="initializationMode"> Defines what to do with new elements. </param>
    /// <exception cref="InvalidOperationException"> Throws when an matrix is allocated already.</exception>
    /// <exception cref="ArgumentException"> Throws when width or height is negative or zero.</exception>
    /// <exception cref="OutOfMemoryException"> Throws when reallocating memory in bytes failed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"> Throws when given irregular InitializationMode </exception>
    /// <remarks> See more about <see cref="InitializationMode"/>, that might be useful.</remarks>
    public void Allocate(int width, int height, InitializationMode initializationMode) 
    {
        if (IsAllocated) throw new InvalidOperationException("Matrix is already allocated.");
        if (width <= 0) throw new ArgumentException("Width must be positive.");
        if (height <= 0) throw new ArgumentException("Height must be positive.");

        unsafe
        {
            Pointer = (T*)NativeMemory.Alloc((nuint)(width * height), (nuint)sizeof(T));
            if (Pointer == null)
                throw new OutOfMemoryException($"Failed to allocate {ByteLength} bytes.");

            switch (initializationMode)
            {
                case InitializationMode.Nothing:
                    break;
                case InitializationMode.Zeroes:
                    // Clears all given memory
                    NativeMemory.Clear(Pointer, (nuint)ByteLength);
                    break;
                case InitializationMode.Constructor:
                    // Calls constructor for every object
                    for (var i = 0; i < width * height; i++)
                        Pointer[i] = new T();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(initializationMode), initializationMode, null);
            }
        }

        Width = width;
        Height = height;
    }

    /// <summary>
    /// Resizes the matrix with saving matrix alignment.
    /// </summary>
    /// <param name="newWidth"> New matrix width</param>
    /// <param name="newHeight"> New matrix height</param>
    /// <param name="initializationMode"> Defines what to do with new elements</param>
    /// <exception cref="ArgumentException"> Throws when width or height is negative or zero.</exception>
    /// <exception cref="InvalidOperationException"> Throws when an matrix is not allocated.</exception>
    /// <exception cref="OutOfMemoryException"> Throws when reallocating memory in bytes failed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"> Throws when given irregular InitializationMode </exception>
    /// <remarks> Neither call constructor, nor setting data to zeroes - only reallocates memory.</remarks>
    /// <remarks> See more about <see cref="InitializationMode"/>, that might be useful.</remarks>
    public void Resize(int newWidth,
        int newHeight,
        InitializationMode initializationMode = InitializationMode.Nothing) 
    {
        if (!IsAllocated) throw new InvalidOperationException("matrix is not allocated.");
        if (newWidth <= 0) throw new ArgumentException("Width must be positive.");
        if (newHeight <= 0) throw new ArgumentException("Height must be positive.");
        if (newWidth == Width && newHeight == Height) return; // Nothing changed - do nothing

        unsafe
        {
            var oldWidth = Width;
            var oldHeight = Height;
            
            var copyWidth = Math.Min(oldWidth, newWidth); // How much data we need to copy further
            var copyHeight = Math.Min(oldHeight, newHeight);
            
            var reallocateBytes = (nuint)(newWidth * newHeight * sizeof(T)); // How many bytes will be given to allocator
            var copyBytes = (nuint)(copyWidth * sizeof(T)); // How many bytes we need to copy

            // Perform reallocation before moving when matrix length increasing
            if (newWidth * newHeight > oldWidth * oldHeight)
            {
                var localPtr = (T*)NativeMemory.Realloc(Pointer, reallocateBytes);
                if (localPtr == null)
                    throw new OutOfMemoryException($"Failed to allocate {ByteLength} bytes.");
                Pointer = localPtr;
            }

            // Moving data by lines
            if (newWidth > oldWidth)
            {
                // Going from the end for not rewriting necessary data
                for (var y = oldHeight - 1; y >= 0; y--)
                {
                    var src = &Pointer[oldWidth * y]; // That is more efficient and safe
                    var dst = &Pointer[newWidth * y];

                    Buffer.MemoryCopy(src, dst, copyBytes, copyBytes);
                }
            }
            else if (newWidth < oldWidth)
            {
                for (var y = 0; y < copyHeight; y++)
                {
                    var src = &Pointer[oldWidth * y];
                    var dst = &Pointer[newWidth * y];

                    Buffer.MemoryCopy(src, dst, copyBytes, copyBytes);
                }
            }

            // Performing reallocation after moving data. When matrix length decreasing
            if (newWidth * newHeight < oldWidth * oldHeight)
            {
                var localPtr = (T*)NativeMemory.Realloc(Pointer, reallocateBytes);
                if (localPtr == null)
                    throw new OutOfMemoryException($"Failed to allocate {ByteLength} bytes.");
                Pointer = localPtr;
            }
            
            // Post-processing new matrix cells
            switch (initializationMode)
            {
                case InitializationMode.Nothing:
                    break;
                case InitializationMode.Zeroes:
                    
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
                case InitializationMode.Constructor:
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
                    throw new ArgumentOutOfRangeException(nameof(initializationMode), initializationMode, null);
            }

            Width = newWidth;
            Height = newHeight;
        }
    }

    /// <summary>
    /// Clears data in matrix.
    /// </summary>
    /// <param name="initializationMode"> Defines what method will be used</param>
    /// <exception cref="InvalidOperationException"> Throws when an matrix is not allocated.</exception>
    public void Clear(InitializationMode initializationMode = InitializationMode.Zeroes) 
    {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
        
        unsafe
        {
            switch (initializationMode)
            {
                // If you want to do nothing - that's up to you
                case InitializationMode.Nothing: 
                    return;
                // Clearing with zeroes
                case InitializationMode.Zeroes:
                    
                    NativeMemory.Clear(Pointer, (nuint)ByteLength);
                    
                    break;
                // Clearing with constructor
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
    /// Fills matrix cells with given value
    /// </summary>
    /// <param name="value"> Value to be placed in matrix cells</param>
    /// <exception cref="InvalidOperationException"> Throws when matrix is not allocated</exception>
    public void Fill(T value) 
    {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");

        unsafe
        {
            for (var i = 0; i < Length; i++)
                Pointer[i] = value;
        }
    }

    /// <summary>
    /// Returns a managed matrix from this unmanaged array.
    /// </summary>
    /// <returns> New managed matrix</returns>
    /// <exception cref="InvalidOperationException"> Throws when matrix is not allocated</exception>
    public T[,] ToManagedMatrix() 
    {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
        
        var output = new T[Width, Height];

        unsafe
        {
            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++) 
                   output[x, y] = Pointer[y * Width + x];
        }

        return output;
    }
    
    /// <summary>
    /// Allocates an unmanaged matrix with elements from the managed array.
    /// </summary>
    /// <param name="matrix"> Managed matrix</param>
    /// <exception cref="InvalidOperationException"> Throws when matrix is already allocated</exception>
    public void AllocateManaged(T[,] matrix) 
    {
        if (IsAllocated) throw new InvalidOperationException("Matrix is already allocated.");
        ArgumentNullException.ThrowIfNull(matrix);

        unsafe
        {
            Width = matrix.GetLength(0);
            Height = matrix.GetLength(1);
            Pointer = (T*)NativeMemory.Alloc((nuint)(Width * Height), (nuint)sizeof(T));
            
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    Pointer[y * Width + x] = matrix[x, y];
        }
    }
    
    /// <summary>
    /// Deallocates all memory used by this matrix.
    /// </summary>
    public void Deallocate() 
    {
        if (!IsAllocated) return;

        unsafe
        {
            NativeMemory.Clear(Pointer, (nuint)ByteLength);
            Width = 0;
            Height = 0;
            Pointer = null;
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

    ~PointerMatrix() 
    {
        Deallocate();
    }
    
}