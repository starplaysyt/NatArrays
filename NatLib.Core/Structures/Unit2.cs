using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace NatLib.Core.Structures;

/// <summary>
/// Presents simple struct with 2 variables
/// </summary>
/// <typeparam name="T">Type of each variable</typeparam>
public struct Unit2<T> : IEquatable<Unit2<T>> where T : INumber<T>
{
    // Container properties
    public T X;
    public T Y;
    
    // Zero and one static properties
    public static readonly Unit2<T> Zero = new (T.Zero, T.Zero);
    public static readonly Unit2<T> One = new (T.One, T.One);
    
    // Constructors
    public Unit2(T x, T y) { X = x; Y = y; }

    public Unit2() : this(T.Zero, T.Zero) { }
    
    // Default operators

    public static Unit2<T> operator+(Unit2<T> u1, Unit2<T> u2)
        => new(u1.X + u2.X, u1.Y + u2.Y);
    
    public static Unit2<T> operator-(Unit2<T> u1, Unit2<T> u2)
        => new(u1.X - u2.X, u1.Y - u2.Y);
    
    public static Unit2<T> operator*(Unit2<T> u1, Unit2<T> u2)
        => new(u1.X * u2.X, u1.Y * u2.Y);
    
    public static Unit2<T> operator/(Unit2<T> u1, Unit2<T> u2)
        => new(u1.X / u2.X, u1.Y / u2.Y);
    
    // Operators with T
    public static Unit2<T> operator+(Unit2<T> u1, T u2)
        => new(u1.X + u2, u1.Y + u2);
    
    public static Unit2<T> operator-(Unit2<T> u1, T u2)
        => new(u1.X - u2, u1.Y - u2);
    
    public static Unit2<T> operator*(Unit2<T> u1, T u2)
        => new(u1.X * u2, u1.Y * u2);
    
    public static Unit2<T> operator/(Unit2<T> u1, T u2)
        => new(u1.X / u2, u1.Y / u2);
    
    // Compare operators 
    public static bool operator ==(Unit2<T> left, Unit2<T> right) =>
        left.Equals(right);

    public static bool operator !=(Unit2<T> left, Unit2<T> right) => !(left == right);
    
    // Operations with existing struct
    public static void Add(ref Unit2<T> left, T value) { left.X += value; left.Y += value; }
    public static void Subtract(ref Unit2<T> left, T value) { left.X -= value; left.Y -= value; }
    public static void Multiply(ref Unit2<T> left, T value) { left.X *= value; left.Y *= value; }
    public static void Divide(ref Unit2<T> left, T value) { left.X /= value; left.Y /= value; }
    
    // Overrides
    public override string ToString() => $"({X}, {Y})";
    
    public override bool Equals([NotNullWhen(true)] object? obj) => 
        obj is Unit2<T> unit2 && Equals(unit2);
    
    public bool Equals(Unit2<T> other) => X.Equals(other.X) && Y.Equals(other.Y);
    
    public override int GetHashCode() => HashCode.Combine(X, Y);
}