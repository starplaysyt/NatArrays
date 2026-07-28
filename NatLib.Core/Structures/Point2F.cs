using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct Point2F : IEquatable<Point2F>
{
    public Vector2 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2F(float x, float y)
    {
        Value = new Vector2(x, y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2F(Vector2 value)
    {
        Value = value;
    }

    // --- Properties ---

    public float X
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.X;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => Value.X = value; }

    public float Y
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Y;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => Value.Y = value; }

    public float Length
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Length(); }

    public float LengthSquared
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.LengthSquared(); }

    public Point2F Normalized
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Vector2.Normalize(Value)); }

    // --- Operators ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator +(Point2F a, Point2F b) => new(a.Value + b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator -(Point2F a, Point2F b) => new(a.Value - b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator +(Point2F a, float s) => new(a.X + s, a.Y + s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator -(Point2F a, float s) => new(a.X - s, a.Y - s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator *(Point2F a, float s) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator *(float s, Point2F a) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator *(Point2F a, Point2F b) => new(a.Value * b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator /(Point2F a, float s) => new(a.Value / s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator /(Point2F a, Point2F b) => new(a.Value / b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F operator -(Point2F a) => new(-a.Value);

    public static bool operator ==(Point2F a, Point2F b) => a.Value == b.Value;

    public static bool operator !=(Point2F a, Point2F b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Point2F p) => p.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Point2F(Vector2 v) => new(v);

    // --- Methods ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out float x, out float y) => (x, y) = (Value.X, Value.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Point2F a, Point2F b) => Vector2.Distance(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Point2F a, Point2F b) => Vector2.DistanceSquared(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Point2F a, Point2F b) => Vector2.Dot(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F Lerp(Point2F a, Point2F b, float t) => new(Vector2.Lerp(a.Value, b.Value, t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F Min(Point2F a, Point2F b) => new(Vector2.Min(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F Max(Point2F a, Point2F b) => new(Vector2.Max(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F Clamp(Point2F value, Point2F min, Point2F max) =>
        new(Vector2.Clamp(value.Value, min.Value, max.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F Reflect(Point2F direction, Point2F normal) =>
        new(Vector2.Reflect(direction.Value, normal.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2F Abs(Point2F a) => new(Vector2.Abs(a.Value));

    // --- Constants ---

    public static readonly Point2F Zero = new(Vector2.Zero);
    public static readonly Point2F One = new(Vector2.One);
    public static readonly Point2F UnitX = new(Vector2.UnitX);
    public static readonly Point2F UnitY = new(Vector2.UnitY);

    // --- Equality ---

    public bool Equals(Point2F other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is Point2F o && Equals(o);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"Point2F({X}, {Y})";
}