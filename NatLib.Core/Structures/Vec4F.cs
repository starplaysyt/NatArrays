using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatLib.Arrays;

/// <summary>
/// Represents a floating-point vector with 4 values.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Vec4F : IEquatable<Vec4F>
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    /// <summary>
    /// Creates vector from given values.
    /// </summary>
    public Vec4F(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Creates zero vector.
    /// </summary>
    public Vec4F() : this(0, 0, 0, 0) { }

    /// <summary>
    /// Creates a vector based on 
    /// </summary>
    /// <param name="ptr"></param>
    public unsafe Vec4F(float* ptr)
    {
        X = ptr[0];
        Y = ptr[1];
        Z = ptr[2];
        W = ptr[3];
    }

    public override string ToString()
    {
        return $"[ {X}, {Y}, {Z}, {W} ]";
    }

    public bool Equals(Vec4F other) =>
        X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);

    public override bool Equals(object? obj) =>
        obj is Vec4F other && Equals(other);

    public override int GetHashCode() => 
        HashCode.Combine(X, Y, Z, W);
}