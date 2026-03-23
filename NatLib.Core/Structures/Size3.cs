using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct Size3 : IEquatable<Size3>
{
    public Vector3 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size3(float width, float height, float depth)
    {
        Value = new Vector3(width, height, depth);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size3(Vector3 value)
    {
        Value = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size3(Size2 wh, float depth)
    {
        Value = new Vector3(wh.Value, depth);
    }

    // --- Properties ---

    public float Width
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.X;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => Value.X = value; }

    public float Height
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Y;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => Value.Y = value; }

    public float Depth
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Z;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => Value.Z = value; }

    public float Volume
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.X * Value.Y * Value.Z; }

    public float Diagonal
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Length(); }

    public Size2 WidthHeight
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X, Value.Y); }

    public Size2 WidthDepth
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X, Value.Z); }

    public Size2 HeightDepth
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.Y, Value.Z); }

    // --- Operators ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator +(Size3 a, Size3 b) => new(a.Value + b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator -(Size3 a, Size3 b) => new(a.Value - b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator *(Size3 a, float s) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator *(float s, Size3 a) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator *(Size3 a, Size3 b) => new(a.Value * b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator /(Size3 a, float s) => new(a.Value / s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator /(Size3 a, Size3 b) => new(a.Value / b.Value);

    public static bool operator ==(Size3 a, Size3 b) => a.Value == b.Value;

    public static bool operator !=(Size3 a, Size3 b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Size3 s) => s.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Size3(Vector3 v) => new(v);

    // --- Methods ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 Lerp(Size3 a, Size3 b, float t) => new(Vector3.Lerp(a.Value, b.Value, t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 Min(Size3 a, Size3 b) => new(Vector3.Min(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 Max(Size3 a, Size3 b) => new(Vector3.Max(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 Clamp(Size3 value, Size3 min, Size3 max) =>
        new(Vector3.Clamp(value.Value, min.Value, max.Value));

    // --- Constants ---

    public static readonly Size3 Zero = new(0, 0, 0);
    public static readonly Size3 One = new(1, 1, 1);

    // --- Equality ---

    public bool Equals(Size3 other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is Size3 o && Equals(o);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"Size3({Width}, {Height}, {Depth})";
}