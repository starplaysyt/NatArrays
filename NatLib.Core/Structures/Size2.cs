using System.Numerics;

namespace NatLib.Core.Structures;

public struct Size2<T> : IEquatable<Size2<T>> where T : INumber<T>
{
    private Unit2<T> _value;
    
    // Container properties contraction
    public T Width { get => _value.X; set => _value.X = value; }
    public T Height { get => _value.Y; set => _value.Y = value; }
    
    // Zero and one static properties
    public static readonly Size2<T> Zero = new (T.Zero, T.Zero);
    public static readonly Size2<T> One = new (T.One, T.One);
    
    // Constructors
    public Size2() : this(T.Zero, T.Zero) { }
    
    public Size2(T x, T y) { _value = new Unit2<T>(x, y); }
    
    public Size2(Unit2<T> u) { _value = u; }
    
    // Conversion operators
    public static implicit operator Unit2<T>(Size2<T> point) => point._value;
    
    public static implicit operator Size2<T>(Unit2<T> unit2) => new(unit2);
    
    // Default operators
    public static Size2<T> operator+(Size2<T> u1, Size2<T> u2) =>   
        new(u1._value.X + u2._value.X, u1._value.Y + u2._value.Y);
    
    public static Size2<T> operator-(Size2<T> u1, Size2<T> u2)
        => new(u1._value.X - u2._value.X, u1._value.Y - u2._value.Y);
    
    public static Size2<T> operator*(Size2<T> u1, Size2<T> u2)
        => new(u1._value.X * u2._value.X, u1._value.Y * u2._value.Y);
    
    public static Size2<T> operator/(Size2<T> u1, Size2<T> u2)
        => new(u1._value.X / u2._value.X, u1._value.Y / u2._value.Y);
    
    // Operators with T
    public static Size2<T> operator+(Size2<T> u1, T u2)
        => new(u1._value.X + u2, u1._value.Y + u2);
    
    public static Size2<T> operator-(Size2<T> u1, T u2)
        => new(u1._value.X - u2, u1._value.Y - u2);
    
    public static Size2<T> operator*(Size2<T> u1, T u2)
        => new(u1._value.X * u2, u1._value.Y * u2);
    
    public static Size2<T> operator/(Size2<T> u1, T u2)
        => new(u1._value.X / u2, u1._value.Y / u2);

    // Compare operators 
    public static bool operator ==(Size2<T> left, Size2<T> right) => left.Equals(right);

    public static bool operator !=(Size2<T> left, Size2<T> right) => !(left == right);
    
    // Operations with existing struct
    public static void Add(ref Size2<T> left, T value) { Unit2<T>.Add(ref left._value, value); }
    public static void Subtract(ref Size2<T> left, T value) { Unit2<T>.Subtract(ref left._value, value); }
    public static void Multiply(ref Size2<T> left, T value) { Unit2<T>.Multiply(ref left._value, value); }
    public static void Divide(ref Size2<T> left, T value) { Unit2<T>.Divide(ref left._value, value); }
    
    // Overrides
    public override string ToString() => _value.ToString();
    
    public bool Equals(Size2<T> other) => _value.Equals(other._value);
    
    public override bool Equals(object? obj) => obj is Size2<T> other && Equals(other);
    
    public override int GetHashCode() => _value.GetHashCode();
}