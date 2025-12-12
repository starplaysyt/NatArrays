using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NatLib.Structs;

public struct Unit3<T> : IEquatable<Unit3<T>> where T : INumber<T>
{
    public T X;
    public T Y;
    public T Z;

    public Unit3(T x, T y, T z) { X = x; Y = y; Z = z; }

    public Unit3() : this(T.Zero, T.Zero, T.Zero) { }

    public static Unit3<T> operator+(Unit3<T> u1, Unit3<T> u2)
        => new(u1.X + u2.X, u1.Y + u2.Y, u1.Z + u2.Z);
    
    public static Unit3<T> operator-(Unit3<T> u1, Unit3<T> u2)
        => new(u1.X - u2.X, u1.Y - u2.Y, u1.Z - u2.Z);
    
    public static Unit3<T> operator*(Unit3<T> u1, Unit3<T> u2)
        => new(u1.X * u2.X, u1.Y * u2.Y,  u1.Z * u2.Z);
    
    public static Unit3<T> operator/(Unit3<T> u1, Unit3<T> u2)
        => new(u1.X / u2.X, u1.Y / u2.Y,  u1.Z / u2.Z);
    
    public static Unit3<T> operator+(Unit3<T> u1, T u2)
        => new(u1.X + u2, u1.Y + u2, u1.Z + u2);
    
    public static Unit3<T> operator-(Unit3<T> u1, T u2)
        => new(u1.X - u2, u1.Y - u2, u1.Z - u2);
    
    public static Unit3<T> operator*(Unit3<T> u1, T u2)
        => new(u1.X * u2, u1.Y * u2, u1.Z * u2);
    
    public static Unit3<T> operator/(Unit3<T> u1, T u2)
        => new(u1.X / u2, u1.Y / u2, u1.Z / u2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T value) { X += value; Y += value; Z += value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Subtract(T value) { X -= value; Y -= value; Z -= value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Multiply(T value) { X *= value; Y *= value; Z *= value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Divide(T value) { X /= value; Y /= value; Z /= value; }
    
    public override string ToString() => $"({X}, {Y}, {Z})";
    
    public override bool Equals([NotNullWhen(true)] object? obj) => 
        obj is Unit3<T> unit3 && Equals(unit3);
    
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    
    public bool Equals(Unit3<T> other) => 
        X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

    public static bool operator ==(Unit3<T> left, Unit3<T> right) =>
        left.Equals(right);

    public static bool operator !=(Unit3<T> left, Unit3<T> right) => !(left == right);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out T x, out T y, out T z) => (x, y, z) = (X, Y, Z);
}