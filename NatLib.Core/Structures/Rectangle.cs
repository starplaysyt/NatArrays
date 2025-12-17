using System.Numerics;

namespace NatLib.Core.Structures;

public struct Rectangle<T> : IEquatable<Rectangle<T>> where T : INumber<T>
{
    private Unit2<T> _locationValue;
    private Unit2<T> _sizeValue;
    
    // Container properties contraction
    public T X { get => _locationValue.X; set => _locationValue.X = value; }
    public T Y { get => _locationValue.Y; set => _locationValue.Y = value; }
    public T Width { get => _sizeValue.X; set => _sizeValue.X = value; }
    public T Height { get => _sizeValue.Y; set => _sizeValue.Y = value; }
    
    // Zero and one static properties
    public static readonly Rectangle<T> Zero = new (T.Zero, T.Zero, T.Zero, T.Zero);
    public static readonly Rectangle<T> One = new (T.One, T.One, T.One, T.One);
    
    // Constructors
    public Rectangle() : this(T.Zero, T.Zero, T.Zero, T.Zero) { }
    
    public Rectangle(T x, T y, T width, T height) { 
        _locationValue = new Unit2<T>(x, y); 
        _sizeValue = new Unit2<T>(width, height); 
    }
    
    public Rectangle(Unit2<T> location, Unit2<T> size) { _locationValue = location;  _sizeValue = size; }
    
    // Conversion operators (DEPRECATED DUE TO INCOMPLETE UNIT4 STRUCT)
    // public static implicit operator Unit2<T>(Rectangle<T> point) => point._locationValue;
    //
    // public static implicit operator Rectangle<T>(Unit2<T> unit2) => new(unit2);
    
    // Default operators (I JUST CANNOT IMAGINE SITUATION WHEN SOMEONE WILL ADD ONE RECTANGLE TO ANOTHER, OR MULTIPLY
    // ONE ON ANOTHER.)
    // public static Rectangle<T> operator+(Rectangle<T> u1, Rectangle<T> u2) =>   
    //     new(u1._locationValue.X + u2._locationValue.X, u1._locationValue.Y + u2._locationValue.Y);
    //
    // public static Rectangle<T> operator-(Rectangle<T> u1, Rectangle<T> u2)
    //     => new(u1._locationValue.X - u2._locationValue.X, u1._locationValue.Y - u2._locationValue.Y);
    //
    // public static Rectangle<T> operator*(Rectangle<T> u1, Rectangle<T> u2)
    //     => new(u1._locationValue.X * u2._locationValue.X, u1._locationValue.Y * u2._locationValue.Y);
    //
    // public static Rectangle<T> operator/(Rectangle<T> u1, Rectangle<T> u2)
    //     => new(u1._locationValue.X / u2._locationValue.X, u1._locationValue.Y / u2._locationValue.Y);
    
    // Operators with T
    public static Rectangle<T> operator+(Rectangle<T> u1, T u2)
        => new(u1._locationValue.X + u2, u1._locationValue.Y + u2, u1.Width + u2, u1.Height + u2);
    
    public static Rectangle<T> operator-(Rectangle<T> u1, T u2)
        => new(u1._locationValue.X - u2, u1._locationValue.Y - u2,  u1.Width - u2, u1.Height - u2);
    
    public static Rectangle<T> operator*(Rectangle<T> u1, T u2)
        => new(u1._locationValue.X * u2, u1._locationValue.Y * u2,  u1.Width * u2, u1.Height * u2);
    
    public static Rectangle<T> operator/(Rectangle<T> u1, T u2)
        => new(u1._locationValue.X / u2, u1._locationValue.Y / u2,  u1.Width / u2, u1.Height / u2);

    // Compare operators 
    public static bool operator ==(Rectangle<T> left, Rectangle<T> right) => left.Equals(right);

    public static bool operator !=(Rectangle<T> left, Rectangle<T> right) => !left.Equals(right);
    
    // Operations with existing struct
    public static void Add(ref Rectangle<T> left, T value) { Unit2<T>.Add(ref left._locationValue, value); }
    public static void Subtract(ref Rectangle<T> left, T value) { Unit2<T>.Subtract(ref left._locationValue, value); }
    public static void Multiply(ref Rectangle<T> left, T value) { Unit2<T>.Multiply(ref left._locationValue, value); }
    public static void Divide(ref Rectangle<T> left, T value) { Unit2<T>.Divide(ref left._locationValue, value); }
    
    // Overrides
    public override string ToString() => _locationValue.ToString();
    
    public bool Equals(Rectangle<T> other) => _locationValue.Equals(other._locationValue) && _sizeValue.Equals(other._sizeValue);
    
    public override bool Equals(object? obj) => obj is Rectangle<T> other && Equals(other);
    
    public override int GetHashCode() => HashCode.Combine(_locationValue.GetHashCode(), _sizeValue.GetHashCode());
}