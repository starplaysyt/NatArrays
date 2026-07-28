using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct Size3F : IEquatable<Size3F>
{
    public Vector3 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size3F(float width, float height, float depth)
    {
        Value = new Vector3(width, height, depth);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size3F(Vector3 value)
    {
        Value = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size3F(Size2F wh, float depth)
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

    public Size2F WidthHeight
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X, Value.Y); }

    public Size2F WidthDepth
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X, Value.Z); }

    public Size2F HeightDepth
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.Y, Value.Z); }

    // --- Operators ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F operator +(Size3F a, Size3F b) => new(a.Value + b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F operator -(Size3F a, Size3F b) => new(a.Value - b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F operator *(Size3F a, float s) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F operator *(float s, Size3F a) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F operator *(Size3F a, Size3F b) => new(a.Value * b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F operator /(Size3F a, float s) => new(a.Value / s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F operator /(Size3F a, Size3F b) => new(a.Value / b.Value);

    public static bool operator ==(Size3F a, Size3F b) => a.Value == b.Value;

    public static bool operator !=(Size3F a, Size3F b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Size3F s) => s.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Size3F(Vector3 v) => new(v);

    // --- Methods ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F Lerp(Size3F a, Size3F b, float t) => new(Vector3.Lerp(a.Value, b.Value, t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F Min(Size3F a, Size3F b) => new(Vector3.Min(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F Max(Size3F a, Size3F b) => new(Vector3.Max(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3F Clamp(Size3F value, Size3F min, Size3F max) =>
        new(Vector3.Clamp(value.Value, min.Value, max.Value));

    // --- Constants ---

    public static readonly Size3F Zero = new(0, 0, 0);
    public static readonly Size3F One = new(1, 1, 1);

    // --- Equality ---

    public bool Equals(Size3F other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is Size3F o && Equals(o);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"Size3F({Width}, {Height}, {Depth})";
}