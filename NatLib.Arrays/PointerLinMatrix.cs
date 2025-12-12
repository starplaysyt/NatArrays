using System.Runtime.InteropServices;

namespace NatLib.Arrays;

public sealed class PointerLinMatrix<T> : IDisposable where T : unmanaged
{
    internal unsafe T** _pointer = null;
    
    public int Width { get; private set; }
    
    public int Height { get; private set; }

    public bool IsAllocated
    {
        get
        {
            unsafe
            {
                return _pointer != null;
            }
        }
    } 

    public ref T this[int x, int y]
    {
        get
        {
            unsafe
            {
                if (!IsAllocated) throw new InvalidOperationException("Matrix is not allocated.");
                if (x < 0 || x >= Width || y < 0 || y >= Height) throw new IndexOutOfRangeException();
                return ref _pointer[y][x];
            }
        } 
    }

    public ref T GetRefUnsafe(int x, int y)
    {
        unsafe
        {
            return ref _pointer[y][x];
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
            _pointer = (T**)NativeMemory.Alloc((nuint)(height), (nuint)(sizeof(T*)));
            
            for (var y = 0; y < height; y++)
            {
                _pointer[y] = (T*)NativeMemory.Alloc((nuint)(width), (nuint)(sizeof(T)));

                switch (mode)
                {
                    case InitializationMode.Nothing:
                        break;
                    case InitializationMode.Zeroes:
                        NativeMemory.Clear(_pointer[y], (nuint)(width * sizeof(T)));
                        break;
                    case InitializationMode.Constructor:
                        for (var x = 0; x < width; x++)
                            _pointer[y][x] = new T();
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

        var minHeight = Math.Min(newHeight, Height);

        unsafe
        {
            //modifying width first, height modifying lower will allocate arrays with correct height
            if (Height != newHeight)
            {
                //free memory on unused pointers will happen only when newHeight less than Height
                for (var i = newHeight; i < Height; i++)
                    NativeMemory.Free(_pointer[i]); 
                
                //reallocating array according to new height. it can be either greater than Height, either less.
                NativeMemory.Realloc(_pointer, (nuint)(sizeof(T*) * newHeight));

                //allocating new memory for newly created cells
                for (var i = Height; i < newHeight; i++)
                {
                    _pointer[i] = (T*)NativeMemory.Alloc((nuint)(sizeof(T) * newWidth));
                    switch (mode)
                    {
                        case InitializationMode.Nothing:
                            break;
                        case InitializationMode.Zeroes:
                            NativeMemory.Clear(_pointer[i], (nuint)(Width * sizeof(T)));
                            break;
                        case InitializationMode.Constructor:
                            for (var x = 0; x < Width; x++)
                                _pointer[i][x] = new T();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                    }
                }
            }

            if (Width != newWidth)
            {
                for (var i = 0; i < Height; i++)
                {
                    _pointer[i] = (T*)NativeMemory.Realloc(_pointer[i], (nuint)(newWidth * sizeof(T)));

                    if (Width < newWidth)
                    {
                        switch (mode)
                        {
                            case InitializationMode.Nothing:
                                break;
                            case InitializationMode.Zeroes:
                                NativeMemory.Clear(&_pointer[i][Width], (nuint)((newWidth - Width) * sizeof(T)));
                                break;
                            case InitializationMode.Constructor:
                                for (var x = 0; x < Width; x++)
                                    _pointer[i][x] = new T();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                        }
                    }
                }
            }
            
            Width = newWidth;
            Height = newHeight;
        }
    }
    
    public T[,] GetManaged()
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
                NativeMemory.Free(_pointer[y]);
            
            NativeMemory.Free(_pointer);
            
            Width = 0;
            Height = 0;
            _pointer = null;
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