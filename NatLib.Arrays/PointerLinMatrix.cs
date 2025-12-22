using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatLib.Arrays;

public sealed class PointerLinMatrix<T> : IDisposable where T : unmanaged
{
    //TODO: Remove ref from [] getter, make for getting by-ref separated method.
    // Reference:
    // when T < 16 bytes - faster to use object copies,
    // when 16 < T < 32 bytes - object copies and refs performance equal,
    // when T > 32 bytes - faster to use ref 
    
    internal unsafe T** Pointer = null;
    
    public int Width { get; private set; }
    
    public int Height { get; private set; }

    public bool IsAllocated
    { get { unsafe { return Pointer != null; } } } 
    
    public ref T this[int x, int y] { get { unsafe {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
        if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException(nameof(x), x, null);
        if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException(nameof(y), y, null);
        
        return ref Pointer[y][x];
    } } }
    
    #region ** Unsafe Region **
    /// <summary>
    /// Returns ref by x and y to element without any checks.
    /// </summary>
    public ref T UnsafeRef(int x, int y) { unsafe { return ref Pointer[y][x]; } }
    /// <summary>
    /// Returns element by x and y without any checks.
    /// </summary>
    public T UnsafeGet(int x, int y) {  unsafe { return Pointer[y][x]; } }
    /// <summary>
    /// Sets element by x and y without any checks.
    /// </summary>
    public T UnsafeSet(int x, int y, T value) { unsafe { return Pointer[y][x] = value; } }
    #endregion
    
    /// <summary>
    /// Returns span for a one row without any checks.
    /// </summary>
    public Span<T> AsSpanUnsafe(int y) { unsafe { 
        return new Span<T>(Pointer[y], Width);
    } }
    
    /// <summary>
    /// Returns span for a one row.
    /// </summary>
    public Span<T> AsSpan(int y)
    {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
        if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException(nameof(y), y, null);
        
        unsafe
        {
            return new Span<T>(Pointer[y], Width);
        }
    }
    
    public void Allocate(int width, int height, InitializationMode mode = InitializationMode.Nothing)
    {
        if (IsAllocated) throw new InvalidOperationException("Matrix is already allocated.");
        if (width <= 0) throw new ArgumentException("Width must be positive.");
        if (height <= 0) throw new ArgumentException("Height must be positive.");

        unsafe
        {
            //reserving memory for the height array (store pointers on default arrays)
            var ptr = (T**)NativeMemory.Alloc((nuint)height, (nuint)sizeof(T*));
            Pointer = ptr;
            
            for (var y = 0; y < height; y++)
            {
                ptr[y] = (T*)NativeMemory.Alloc((nuint)width, (nuint)sizeof(T));

                switch (mode)
                {
                    case InitializationMode.Nothing:
                        break;
                    case InitializationMode.Zeroes:
                        NativeMemory.Clear(Pointer[y], (nuint)(width * sizeof(T)));
                        break;
                    case InitializationMode.Constructor:
                        new Span<T>(Pointer[y], width).Fill(new T());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
            }
            
            Width = width;
            Height = height;
        }
    }

    public void Resize(int newWidth, int newHeight, InitializationMode mode = InitializationMode.Nothing)
    {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
        if (newWidth <= 0) throw new ArgumentException("Width must be positive.");
        if (newHeight <= 0) throw new ArgumentException("Height must be positive.");
        if (newWidth == Width && newHeight == Height) return; // Nothing changed - do nothing

        unsafe
        {
            // Modifying height first, width modifying will be further
            if (Height != newHeight)
            {
                // Free memory on unused pointers will happen only when newHeight less than Height
                for (var i = newHeight; i < Height; i++)
                    NativeMemory.Free(Pointer[i]); 
                
                // Reallocating array according to new height. it can be either greater than Height, either less.
                Pointer = (T**)NativeMemory.Realloc(Pointer, (nuint)(sizeof(T*) * newHeight));

                // Allocating new memory for newly created cells
                for (var i = Height; i < newHeight; i++)
                {
                    Pointer[i] = (T*)NativeMemory.Alloc((nuint)(sizeof(T) * newWidth));
                    switch (mode)
                    {
                        case InitializationMode.Nothing:
                            break;
                        case InitializationMode.Zeroes:
                            NativeMemory.Clear(Pointer[i], (nuint)(Width * sizeof(T)));
                            break;
                        case InitializationMode.Constructor:
                            new Span<T>(Pointer[i], Width).Fill(new T());
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                    }
                }
            }
            
            // Modifying width now
            if (Width != newWidth)
            {
                // Width reallocation - just expanding array internal arrays by references
                for (var i = 0; i < Height; i++)
                {
                    // Using realloc to resize each array
                    Pointer[i] = (T*)NativeMemory.Realloc(Pointer[i], (nuint)(newWidth * sizeof(T)));

                    // When reallocated array is larger than previous - initialize new cells.
                    // When reallocated array is smaller - just do nothing.
                    if (newWidth > Width)
                    {
                        switch (mode)
                        {
                            case InitializationMode.Nothing:
                                break;
                            // Filling starts from the beginning of resized area
                            case InitializationMode.Zeroes:
                                NativeMemory.Clear(&Pointer[i][Width], (nuint)((newWidth - Width) * sizeof(T)));
                                break;
                            // Same, but in the cycle
                            case InitializationMode.Constructor:
                                new Span<T>(Pointer[i], newWidth)[Width..].Fill(new T());
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                        }
                    }
                }
            }
            
            // Either Width or Height must be changed to reach that point, so they need to be updated
            Width = newWidth;
            Height = newHeight;
        }
    }
    
    public T[,] ToManaged()
    {
        if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
        
        var output = new T[Width, Height];

        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                output[y,x] = this[x, y];
        
        return output;
    }

    public void Deallocate()
    {
        if (!IsAllocated) return;

        unsafe
        {
            for (var y = 0; y < Height; y++)
                NativeMemory.Free(Pointer[y]);
            
            NativeMemory.Free(Pointer);
            
            Width = 0;
            Height = 0;
            Pointer = null;
        }
    }


    
    public void Dispose()
    {
        Deallocate();
        GC.SuppressFinalize(this);
    }

    ~PointerLinMatrix()
    {
        Dispose();
    }
}