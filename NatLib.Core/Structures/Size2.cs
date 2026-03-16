using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct Size2 : IEquatable<Size2>
{
    public Vector2 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size2(float width, float height) => Value = new Vector2(width, height);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size2(Vector2 value) => Value = value;

    // --- Properties ---

    public float Width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.X = value;
    }

    public float Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.Y = value;
    }

    public float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.X * Value.Y;
    }

    public float Diagonal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Length();
    }

    public float AspectRatio
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.X / Value.Y;
    }

    public Point2 Center
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value * 0.5f);
    }

    // --- Operators ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator +(Size2 a, Size2 b) => new(a.Value + b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator -(Size2 a, Size2 b) => new(a.Value - b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator *(Size2 a, float s) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator *(float s, Size2 a) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator *(Size2 a, Size2 b) => new(a.Value * b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator /(Size2 a, float s) => new(a.Value / s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator /(Size2 a, Size2 b) => new(a.Value / b.Value);

    public static bool operator ==(Size2 a, Size2 b) => a.Value == b.Value;
    public static bool operator !=(Size2 a, Size2 b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Size2 s) => s.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Size2(Vector2 v) => new(v);

    // --- Methods ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 Lerp(Size2 a, Size2 b, float t) => new(Vector2.Lerp(a.Value, b.Value, t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 Min(Size2 a, Size2 b) => new(Vector2.Min(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 Max(Size2 a, Size2 b) => new(Vector2.Max(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Point2 point) =>
        point.X >= 0 && point.X <= Width &&
        point.Y >= 0 && point.Y <= Height;

    // --- Constants ---

    public static readonly Size2 Zero = new(0, 0);
    public static readonly Size2 One  = new(1, 1);

    // --- Equality ---

    public bool Equals(Size2 other) => Value.Equals(other.Value);
    public override bool Equals(object? obj) => obj is Size2 o && Equals(o);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => $"Size2({Width}, {Height})";
}