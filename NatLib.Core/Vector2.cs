using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core;

public struct Vector2<T> : IEquatable<Vector2<T>> where T : INumber<T>, IRootFunctions<T>
{
    private Unit2<T> _value;
    
    // Container properties contraction
    public T X { get => _value.X; set => _value.X = value; }
    public T Y { get => _value.Y; set => _value.Y = value; }
    
    // Constructors
    public Vector2() : this(T.Zero, T.Zero) { }
    
    public Vector2(T x, T y) { _value = new Unit2<T>(x, y); }
    
    public Vector2(Unit2<T> u) { _value = u; }
    
    // Conversion operators
    public static implicit operator Unit2<T>(Vector2<T> point) => point._value;
    
    public static implicit operator Vector2<T>(Unit2<T> unit2) => new(unit2);
    
    // Default operators
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T> operator+(Vector2<T> u1, Vector2<T> u2) =>   
        new(u1._value.X + u2._value.X, u1._value.Y + u2._value.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T> operator-(Vector2<T> u1, Vector2<T> u2)
        => new(u1._value.X - u2._value.X, u1._value.Y - u2._value.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T> operator*(Vector2<T> u1, Vector2<T> u2)
        => new(u1._value.X * u2._value.X, u1._value.Y * u2._value.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T> operator/(Vector2<T> u1, Vector2<T> u2)
        => new(u1._value.X / u2._value.X, u1._value.Y / u2._value.Y);
    
    // Operators with T
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T> operator+(Vector2<T> u1, T u2)
        => new(u1._value.X + u2, u1._value.Y + u2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T> operator-(Vector2<T> u1, T u2)
        => new(u1._value.X - u2, u1._value.Y - u2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T> operator*(Vector2<T> u1, T u2)
        => new(u1._value.X * u2, u1._value.Y * u2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2<T> operator/(Vector2<T> u1, T u2)
        => new(u1._value.X / u2, u1._value.Y / u2);

    // Compare operators 
    public static bool operator ==(Vector2<T> left, Vector2<T> right) => left.Equals(right);

    public static bool operator !=(Vector2<T> left, Vector2<T> right) => !(left == right);
    
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
    public bool Equals(Vector2<T> other) => _value.Equals(other._value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Vector2<T> other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _value.GetHashCode();
    
    // Deconstructor
    public void Deconstruct(out T x, out T y) => (x, y) = (X, Y);
    
    // Type-specific operations
    
    public static Vector2<T> Zero => default;

    public T LengthSquared => _value.X * _value.X + _value.Y * _value.Y;

    public T Length => T.Sqrt(LengthSquared);
    
    public Vector2<T> Normalized() => this * (T.One / Length);

}