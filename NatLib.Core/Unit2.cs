using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NatLib.Core;

/// <summary>
/// Presents simple struct with 2 variables
/// </summary>
/// <typeparam name="T">Type of each variable</typeparam>
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
    
    public static bool operator ==(Unit2<T> left, Unit2<T> right) =>
        left.Equals(right);

    public static bool operator !=(Unit2<T> left, Unit2<T> right) => !(left == right);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T value) { X += value; Y += value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Subtract(T value) { X -= value; Y -= value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Multiply(T value) { X *= value; Y *= value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Divide(T value) { X /= value; Y /= value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => $"({X}, {Y})";
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals([NotNullWhen(true)] object? obj) => 
        obj is Unit2<T> unit2 && Equals(unit2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Unit2<T> other) => X.Equals(other.X) && Y.Equals(other.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(X, Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out T x, out T y) => (x, y) = (X, Y);
}