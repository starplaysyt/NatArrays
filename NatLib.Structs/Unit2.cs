using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NatLib.Structs;

public struct Unit2<T> : IEquatable<Unit2<T>> where T : INumber<T>
{
    public T X;
    public T Y;

    public Unit2(T x, T y) { X = x; Y = y; }

    public Unit2() : this(T.Zero, T.Zero) { }

    public static Unit2<T> operator+(Unit2<T> u1, Unit2<T> u2)
        => new(u1.X + u2.X, u1.Y + u2.Y);
    
    public static Unit2<T> operator-(Unit2<T> u1, Unit2<T> u2)
        => new(u1.X - u2.X, u1.Y - u2.Y);
    
    public static Unit2<T> operator*(Unit2<T> u1, Unit2<T> u2)
        => new(u1.X * u2.X, u1.Y * u2.Y);
    
    public static Unit2<T> operator/(Unit2<T> u1, Unit2<T> u2)
        => new(u1.X / u2.X, u1.Y / u2.Y);
    
    public static Unit2<T> operator+(Unit2<T> u1, T u2)
        => new(u1.X + u2, u1.Y + u2);
    
    public static Unit2<T> operator-(Unit2<T> u1, T u2)
        => new(u1.X - u2, u1.Y - u2);
    
    public static Unit2<T> operator*(Unit2<T> u1, T u2)
        => new(u1.X * u2, u1.Y * u2);
    
    public static Unit2<T> operator/(Unit2<T> u1, T u2)
        => new(u1.X / u2, u1.Y / u2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T value) { X += value; Y += value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Subtract(T value) { X -= value; Y -= value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Multiply(T value) { X *= value; Y *= value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Divide(T value) { X /= value; Y /= value; }
    
    public override string ToString() => $"({X}, {Y})";
    
    public override bool Equals([NotNullWhen(true)] object? obj) => 
        obj is Unit2<T> unit2 && Equals(unit2);
    
    public override int GetHashCode() => HashCode.Combine(X, Y);
    
    public bool Equals(Unit2<T> other) => X.Equals(other.X) && Y.Equals(other.Y);

    public static bool operator ==(Unit2<T> left, Unit2<T> right) =>
        left.Equals(right);

    public static bool operator !=(Unit2<T> left, Unit2<T> right) => !(left == right);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out T x, out T y) => (x, y) = (X, Y);
}