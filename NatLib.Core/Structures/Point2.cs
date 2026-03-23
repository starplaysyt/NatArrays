using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct Point2 : IEquatable<Point2>
{
    public Vector2 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2(float x, float y)
    {
        Value = new Vector2(x, y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2(Vector2 value)
    {
        Value = value;
    }

    // --- Properties ---

    public float X
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.X = value;
    }

    public float Y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.Y = value;
    }

    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Length();
    }

    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.LengthSquared();
    }

    public Point2 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Vector2.Normalize(Value));
    }

    // --- Operators ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator +(Point2 a, Point2 b) => new(a.Value + b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator -(Point2 a, Point2 b) => new(a.Value - b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator *(Point2 a, float s) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator *(float s, Point2 a) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator *(Point2 a, Point2 b) => new(a.Value * b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator /(Point2 a, float s) => new(a.Value / s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator /(Point2 a, Point2 b) => new(a.Value / b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator -(Point2 a) => new(-a.Value);

    public static bool operator ==(Point2 a, Point2 b) => a.Value == b.Value;

    public static bool operator !=(Point2 a, Point2 b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Point2 p) => p.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Point2(Vector2 v) => new(v);

    // --- Methods ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Point2 a, Point2 b) => Vector2.Distance(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Point2 a, Point2 b) => Vector2.DistanceSquared(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Point2 a, Point2 b) => Vector2.Dot(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 Lerp(Point2 a, Point2 b, float t) => new(Vector2.Lerp(a.Value, b.Value, t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 Min(Point2 a, Point2 b) => new(Vector2.Min(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 Max(Point2 a, Point2 b) => new(Vector2.Max(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 Clamp(Point2 value, Point2 min, Point2 max) =>
        new(Vector2.Clamp(value.Value, min.Value, max.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 Reflect(Point2 direction, Point2 normal) =>
        new(Vector2.Reflect(direction.Value, normal.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 Abs(Point2 a) => new(Vector2.Abs(a.Value));

    // --- Constants ---

    public static readonly Point2 Zero = new(Vector2.Zero);
    public static readonly Point2 One = new(Vector2.One);
    public static readonly Point2 UnitX = new(Vector2.UnitX);
    public static readonly Point2 UnitY = new(Vector2.UnitY);

    // --- Equality ---

    public bool Equals(Point2 other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is Point2 o && Equals(o);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"Point2({X}, {Y})";
}