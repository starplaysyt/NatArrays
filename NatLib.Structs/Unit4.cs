using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NatLib.Structs;

public struct Unit4<T> : IEquatable<Unit4<T>> where T : INumber<T>
{
    public T X;
    public T Y;
    public T W;
    public T H;

    public Unit4(T x, T y, T w, T h) { X = x; Y = y; W = w; H = h; }

    public Unit4() : this(T.Zero, T.Zero, T.Zero, T.Zero) { }

    public static Unit4<T> operator+(Unit4<T> u1, Unit4<T> u2)
        => new(u1.X + u2.X, u1.Y + u2.Y, u1.W + u2.W, u1.H + u2.H);
    
    public static Unit4<T> operator-(Unit4<T> u1, Unit4<T> u2)
        => new(u1.X - u2.X, u1.Y - u2.Y, u1.W - u2.W, u1.H - u2.H);
    
    public static Unit4<T> operator*(Unit4<T> u1, Unit4<T> u2)
        => new(u1.X * u2.X, u1.Y * u2.Y, u1.W * u2.W, u1.H * u2.H);
    
    public static Unit4<T> operator/(Unit4<T> u1, Unit4<T> u2)
        => new(u1.X / u2.X, u1.Y / u2.Y, u1.W / u2.W, u1.H / u2.H);
    
    public static Unit4<T> operator+(Unit4<T> u1, T u2)
        => new(u1.X + u2, u1.Y + u2, u1.W + u2, u1.H + u2);
    
    public static Unit4<T> operator-(Unit4<T> u1, T u2)
        => new(u1.X - u2, u1.Y - u2, u1.W - u2, u1.H - u2);
    
    public static Unit4<T> operator*(Unit4<T> u1, T u2)
        => new(u1.X * u2, u1.Y * u2, u1.W * u2, u1.H * u2);
    
    public static Unit4<T> operator/(Unit4<T> u1, T u2)
        => new(u1.X / u2, u1.Y / u2, u1.W / u2, u1.H / u2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T value) { X += value; Y += value; W += value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Subtract(T value) { X -= value; Y -= value; W -= value; H -= value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Multiply(T value) { X *= value; Y *= value; W *= value; H -= value; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Divide(T value) { X /= value; Y /= value; W /= value; H -= value; }
    
    public override string ToString() => $"({X}, {Y}, {W}, {H})";
    
    public override bool Equals([NotNullWhen(true)] object? obj) => 
        obj is Unit4<T> unit4 && Equals(unit4);
    
    public override int GetHashCode() => HashCode.Combine(X, Y, W, H);
    
    public bool Equals(Unit4<T> other) => 
        X.Equals(other.X) && Y.Equals(other.Y) && W.Equals(other.W) && H.Equals(other.H);

    public static bool operator ==(Unit4<T> left, Unit4<T> right) =>
        left.Equals(right);

    public static bool operator !=(Unit4<T> left, Unit4<T> right) => !(left == right);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out T x, out T y, out T w, out T h) => (x, y, w, h) = (X, Y, W, H);
}