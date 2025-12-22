using System.Runtime.InteropServices;

namespace NatLib.Arrays;

/// <summary>
/// Implementation of a sequentially mapped matrix (row-major).
/// </summary>
/// <typeparam name="T"> Unmanaged datatype</typeparam>
public sealed class PointerSeqMatrix<T> : IDisposable where T : unmanaged
{
    internal unsafe T* Pointer = null;

    /// <summary>
    /// Count of matrix elements by X-coordinate
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// Count of matrix elements by Y-coordinate
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// Returns whether matrix is allocated or not
    /// </summary>
    public bool IsAllocated { get { unsafe { return Pointer != null; } } }

    /// <summary>
    /// Gets absolute length of the matrix
    /// </summary>
    public int Length => Width * Height;

    /// <summary>
    /// Gets length of the matrix in byte representation
    /// </summary>
    public ulong ByteLength { get { unsafe { return (ulong)(sizeof(T) * Length); } } }

    /// <summary>
    /// Gets reference of the element in the matrix by its X and Y
    /// </summary>
    /// <param name="x"> Coordinate by Width</param>
    /// <param name="y"> Coordinate by Height</param>
    /// <exception cref="IndexOutOfRangeException"> Throws when given x or y is out of bounds of the
    /// matrix</exception>
    public ref T this[int x, int y] { get { unsafe {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
        if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException(nameof(x), x, null);
        if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException(nameof(y), y, null);

        return ref Pointer[y * Width + x];
    } } }

    /// <summary>
    /// Gets reference of the element in the matrix by its absolute index
    /// </summary>
    /// <param name="i"> Absolute element index</param>
    /// <exception cref="IndexOutOfRangeException"> Throws when given element index is out of bounds of the
    /// matrix</exception>
    public ref T this[int i] { get { unsafe {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
        if ((uint)i >= (uint)(Width * Height) || i < 0) throw new ArgumentOutOfRangeException(nameof(i), i, null);
        
        return ref Pointer[i];
    } } }

    #region ** Unsafe Region **
    /// <summary>
    /// Returns ref by x and y to element without any checks.
    /// </summary>
    public ref T UnsafeRef(int x, int y) { unsafe { return ref Pointer[y * Width + x]; } }
    /// <summary>
    /// Returns ref by i to element without any checks.
    /// </summary>
    public ref T UnsafeRef(int i) { unsafe { return ref Pointer[i]; } }
    /// <summary>
    /// Returns element by x and y without any checks.
    /// </summary>
    public T UnsafeGet(int x, int y) {  unsafe { return Pointer[y * Width + x]; } }
    /// <summary>
    /// Returns element by i without any checks.
    /// </summary>
    public T UnsafeGet(int i) {  unsafe { return Pointer[i]; } }
    /// <summary>
    /// Sets element by x and y without any checks.
    /// </summary>
    public T UnsafeSet(int x, int y, T value) { unsafe { return Pointer[y * Width + x] = value; } }
    /// <summary>
    /// Sets element by i without any checks.
    /// </summary>
    public T UnsafeSet(int i, T value) { unsafe { return Pointer[i] = value; } }
    #endregion

    public Span<T> AsSpan() { unsafe { 
        return IsAllocated 
            ? new Span<T>(Pointer, Length)
            : throw new InvalidOperationException("Matrix is not allocated."); } }
    
    /// <summary>
    /// Allocates memory for the matrix.
    /// </summary>
    /// <param name="width"> New matrix width</param>
    /// <param name="height"> New matrix height</param>
    /// <param name="initMode"> Defines what to do with new elements. </param>
    /// <exception cref="InvalidOperationException"> Throws when an matrix is allocated already.</exception>
    /// <exception cref="ArgumentException"> Throws when width or height is negative or zero.</exception>
    /// <exception cref="OutOfMemoryException"> Throws when reallocating memory in bytes failed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"> Throws when given irregular InitializationMode </exception>
    /// <remarks> See more about <see cref="InitializationMode"/>, that might be useful.</remarks>
    public void Allocate(int width, int height, InitializationMode initMode = InitializationMode.Nothing) 
    {
        if (IsAllocated) throw new InvalidOperationException("Matrix is already allocated.");
        if (width <= 0) throw new ArgumentException("Width must be positive.");
        if (height <= 0) throw new ArgumentException("Height must be positive.");

        unsafe
        {
            var ptr = (T*)NativeMemory.Alloc((nuint)(width * height), (nuint)sizeof(T));

            Pointer = ptr;
            Width = width;
            Height = height;
            
            switch (initMode)
            {
                case InitializationMode.Nothing:
                    break;
                case InitializationMode.Zeroes:
                    // Clears all given memory
                    NativeMemory.Clear(Pointer, (nuint)ByteLength);
                    break;
                case InitializationMode.Constructor:
                    new Span<T>(Pointer, width * height).Fill(new T());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(initMode), initMode, null);
            }
        }
    }

    /// <summary>
    /// Resizes the matrix with saving matrix alignment.
    /// </summary>
    /// <param name="newWidth"> New matrix width</param>
    /// <param name="newHeight"> New matrix height</param>
    /// <param name="initMode"> Defines the way of initialization for new elements</param>
    /// <exception cref="ArgumentException"> Throws when width or height is negative or zero.</exception>
    /// <exception cref="InvalidOperationException"> Throws when an matrix is not allocated.</exception>
    /// <exception cref="OutOfMemoryException"> Throws when reallocating memory in bytes failed.</exception>
    /// <exception cref="ArgumentOutOfRangeException"> Throws when given irregular InitializationMode.</exception>
    public void Resize(int newWidth,
        int newHeight,
        InitializationMode initMode = InitializationMode.Nothing) 
    {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
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
            switch (initMode)
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
                    throw new ArgumentOutOfRangeException(nameof(initMode), initMode, null);
            }

            Width = newWidth;
            Height = newHeight;
        }
    }

    /// <summary>
    /// Clears data in matrix.
    /// </summary>
    /// <param name="initMode"> Defines what method will be used</param>
    /// <exception cref="InvalidOperationException"> Throws when an matrix is not allocated.</exception>
    public void Clear(InitializationMode initMode = InitializationMode.Zeroes)
    {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
        
        unsafe
        {
            switch (initMode)
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
                    new Span<T>(Pointer, Length).Fill(new T());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(initMode), initMode, null);
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
            new Span<T>(Pointer, Length).Fill(value);
        }
    }
    
    /// <summary>
    /// Allocates an unmanaged matrix with elements from the managed array.
    /// </summary>
    /// <param name="matrix"> Managed matrix</param>
    /// <exception cref="InvalidOperationException"> Throws when matrix is already allocated</exception>
    public void FromManaged(T[,] matrix) 
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
    /// Returns a managed copy of this matrix.
    /// </summary>
    /// <returns> New managed matrix</returns>
    /// <exception cref="InvalidOperationException"> Throws when matrix is not allocated</exception>
    public T[,] GetManaged() 
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

    ~PointerSeqMatrix() 
    {
        Deallocate();
    }
    
}