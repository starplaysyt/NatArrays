using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core;

public struct Point2<T> : IEquatable<Point2<T>> where T : INumber<T>
{
    private Unit2<T> _value;
    
    // Container properties contraction
    public T X { get => _value.X; set => _value.X = value; }
    public T Y { get => _value.Y; set => _value.Y = value; }
    
    // Constructors
    public Point2() : this(T.Zero, T.Zero) { }
    
    public Point2(T x, T y) { _value = new Unit2<T>(x, y); }
    
    public Point2(Unit2<T> u) { _value = u; }
    
    // Conversion operators
    public static implicit operator Unit2<T>(Point2<T> point) => point._value;
    
    public static implicit operator Point2<T>(Unit2<T> unit2) => new(unit2);
    
    // Default operators
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2<T> operator+(Point2<T> u1, Point2<T> u2) => 
        new(u1._value.X + u2._value.X, u1._value.Y + u2._value.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2<T> operator-(Point2<T> u1, Point2<T> u2)
        => new(u1._value.X - u2._value.X, u1._value.Y - u2._value.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2<T> operator*(Point2<T> u1, Point2<T> u2)
        => new(u1._value.X * u2._value.X, u1._value.Y * u2._value.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2<T> operator/(Point2<T> u1, Point2<T> u2)
        => new(u1._value.X / u2._value.X, u1._value.Y / u2._value.Y);
    
    // Operators with T
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2<T> operator+(Point2<T> u1, T u2)
        => new(u1._value.X + u2, u1._value.Y + u2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2<T> operator-(Point2<T> u1, T u2)
        => new(u1._value.X - u2, u1._value.Y - u2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2<T> operator*(Point2<T> u1, T u2)
        => new(u1._value.X * u2, u1._value.Y * u2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2<T> operator/(Point2<T> u1, T u2)
        => new(u1._value.X / u2, u1._value.Y / u2);

    // Compare operators 
    public static bool operator ==(Point2<T> left, Point2<T> right) => left.Equals(right);

    public static bool operator !=(Point2<T> left, Point2<T> right) => !(left == right);
    
    // Operations with existing struct
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T value) { _value.Add(value); }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Subtract(T value) { _value.Subtract(value); }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Multiply(T value) { _value.Multiply(value); }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Divide(T value) { _value.Divide(value); }
    
    // Overrides
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => _value.ToString();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Point2<T> other) => _value.Equals(other._value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Point2<T> other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _value.GetHashCode();
    
    // Deconstructor
    public void Deconstruct(out T x, out T y) => (x, y) = (X, Y);
    
    // Type-specific operations
    
}