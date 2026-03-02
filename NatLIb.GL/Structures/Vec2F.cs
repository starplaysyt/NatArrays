using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NatLib.GL.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Vec2F : IEquatable<Vec2F>
{
    public float X;
    public float Y;

    public Vec2F(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Vec2F() : this(0f, 0f) { }

    public unsafe Vec2F(float* ptr)
    {
        X = ptr[0];
        Y = ptr[1];
    }

    public override string ToString() =>  $"({X}, {Y})";

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Vec2F other && Equals(other);
    
    public bool Equals(Vec2F other) => 
        X.Equals(other.X) && Y.Equals(other.Y);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(Vec2F left, Vec2F right) => 
        left.Equals(right);

    public static bool operator !=(Vec2F left, Vec2F right) => 
        !(left == right);
}