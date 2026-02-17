using System.Runtime.InteropServices;

namespace NatLib.Arrays;

/// <summary>
/// Represents a 4x4 floating-point matrix stored in column-major order.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Mat4F : IEquatable<Mat4F>
{
    /// <summary>
    /// Internal fixed array storing 16 floats in column-major order.
    /// </summary>
    private unsafe fixed float _data[16];

    /// <summary>
    /// The number of rows/columns of the square matrix (4).
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// Accesses the element at the given row and column in column-major order.
    /// </summary>
    public float this[int row, int col]
    {
        get
        {
            unsafe
            {
                if (row < 0 || row >= Size || col < 0 || col >= Size)
                    throw new IndexOutOfRangeException();
                return _data[col * 4 + row];
            }
        }
        set
        {
            unsafe
            {
                if (row < 0 || row >= Size || col < 0 || col >= Size)
                    throw new IndexOutOfRangeException();
                _data[col * 4 + row] = value;
            }
        }
    }

    /// <summary>
    /// Creates empty matrix
    /// </summary>
    public Mat4F() { }

    public Mat4F(params float[] data)
    {
        unsafe
        {
            fixed (float* t = _data)
            {
                var span = new Span<float>(t, Size*Size);
                var spanArray = data.AsSpan();
                
                //spanArray.CopyTo(span);
                
                if (!spanArray.TryCopyTo(span)) throw new ArgumentException("Given \"data\" is longer ({ than size of the matrix.");
            }
        }
    }

    /// <summary>
    /// Creates matrix from given pointer
    /// </summary>
    /// <param name="ptr"></param>
    public unsafe Mat4F(float* ptr)
    {
        fixed (float* f = _data)
            NativeMemory.Copy(ptr, f, Size * Size * sizeof(float));
    }
    
    /// <summary>
    /// Returns a Vec4F matrix row interpretation.
    /// </summary>
    public Vec4F GetRow(int row)
    {
        unsafe
        {
            return new Vec4F(_data[row], _data[Size + row], _data[Size * 2 + row], _data[Size * 3 + row]);
        }
    }

    public void SetRow(int row, Vec4F value)
    {
        unsafe
        {
            _data[row] =  value.X;
            _data[Size + row] = value.Y;
            _data[Size * 2 + row] = value.Z;
            _data[Size * 3 + row] = value.W;
        }
    }

    /// <summary>
    /// Returns a string representation of the matrix in readable 4x4 format (column-major).
    /// </summary>
    public override string ToString()
    {
        unsafe
        {
            var sb = new System.Text.StringBuilder();
            for (var row = 0; row < 4; row++)
            {
                sb.Append('[');
                for (var col = 0; col < 4; col++)
                {
                    sb.Append($"{_data[col * 4 + row],4:0.###} "); // column-major
                }
                sb.AppendLine("],");
            }
            return sb.ToString();
        }
    }

    public override bool Equals(object? obj) => 
        obj is Mat4F other && Equals(other);

    public bool Equals(Mat4F other)
    {
        bool isEqual;
        const int length = Size * Size;

        unsafe
        {
            fixed (float* t = _data)
            {
                var thisSpan = new Span<float>(t, length);
                var otherSpan = new Span<float>(other._data, length);
                
                isEqual = thisSpan.SequenceEqual(otherSpan);
            }
        }

        return isEqual;
    }

    public override int GetHashCode()
    {
        var resolvedHash = 0;
        
        unsafe
        {
            fixed (float* t = _data)
            {
                var span = new Span<float>(t, Size * Size);

                foreach (var t1 in span)
                {
                    resolvedHash = HashCode.Combine(resolvedHash, t1);
                }
            }
        }
        
        return resolvedHash;
    }
}
