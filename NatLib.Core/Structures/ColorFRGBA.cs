using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct ColorFRGBA : IEquatable<ColorFRGBA>
{
    public Vector4 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorFRGBA(float r, float g, float b, float a = 1f) => Value = new Vector4(r, g, b, a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorFRGBA(Vector4 value) => Value = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorFRGBA(Vector3 rgb, float a = 1f) => Value = new Vector4(rgb, a);

    // --- Properties ---

    public float R
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.X;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.X = value;
    }

    public float G
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.Y = value;
    }

    public float B
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.Z = value;
    }

    public float A
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.W = value;
    }

    public float Luminance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 0.2126f * Value.X + 0.7152f * Value.Y + 0.0722f * Value.Z;
    }

    public ColorFRGBA Clamped
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Vector4.Clamp(Value, Vector4.Zero, Vector4.One));
    }

    public ColorFRGBA Opaque
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X, Value.Y, Value.Z);
    }

    public Vector3 RGB
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X, Value.Y, Value.Z);
    }

    // --- Factory ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA FromBytes(byte r, byte g, byte b, byte a = 255) =>
        new(r / 255f, g / 255f, b / 255f, a / 255f);

    public static ColorFRGBA FromHex(string hex)
    {
        hex = hex.TrimStart('#');
        var r = Convert.ToByte(hex[0..2], 16);
        var g = Convert.ToByte(hex[2..4], 16);
        var b = Convert.ToByte(hex[4..6], 16);
        var a = hex.Length >= 8 ? Convert.ToByte(hex[6..8], 16) : (byte)255;
        return FromBytes(r, g, b, a);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA FromHSV(float h, float s, float v)
    {
        var c = v * s;
        var x = c * (1f - MathF.Abs(h / 60f % 2f - 1f));
        var m = v - c;

        float r, g, b;
        switch (h)
        {
            case < 60: r = c; g = x; b = 0;
                break;
            case < 120: r = x; g = c; b = 0;
                break;
            case < 180: r = 0; g = c; b = x;
                break;
            case < 240: r = 0; g = x; b = c;
                break;
            case < 300: r = x; g = 0; b = c;
                break;
            default: r = c; g = 0; b = x;
                break;
        }

        return new ColorFRGBA(r + m, g + m, b + m);
    }

    // --- Operators ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA operator +(ColorFRGBA a, ColorFRGBA b) => new(a.Value + b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA operator -(ColorFRGBA a, ColorFRGBA b) => new(a.Value - b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA operator *(ColorFRGBA a, ColorFRGBA b) => new(a.Value * b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA operator *(ColorFRGBA a, float s) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA operator *(float s, ColorFRGBA a) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA operator /(ColorFRGBA a, float s) => new(a.Value / s);

    public static bool operator ==(ColorFRGBA a, ColorFRGBA b) => a.Value == b.Value;

    public static bool operator !=(ColorFRGBA a, ColorFRGBA b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(ColorFRGBA c) => c.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ColorFRGBA(Vector4 v) => new(v);

    // --- Methods ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA Lerp(ColorFRGBA a, ColorFRGBA b, float t) => new(Vector4.Lerp(a.Value, b.Value, t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA Min(ColorFRGBA a, ColorFRGBA b) => new(Vector4.Min(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ColorFRGBA Max(ColorFRGBA a, ColorFRGBA b) => new(Vector4.Max(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorFRGBA WithAlpha(float alpha) => new(Value.X, Value.Y, Value.Z, alpha);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorFRGBA WithR(float r) => new(r, Value.Y, Value.Z, Value.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorFRGBA WithG(float g) => new(Value.X, g, Value.Z, Value.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ColorFRGBA WithB(float b) => new(Value.X, Value.Y, b, Value.W);

    public (byte R, byte G, byte B, byte A) ToBytes()
    {
        var c = Clamped;
        return (
            (byte)(c.Value.X * 255f),
            (byte)(c.Value.Y * 255f),
            (byte)(c.Value.Z * 255f),
            (byte)(c.Value.W * 255f)
        );
    }

    public string ToHex()
    {
        var (r, g, b, a) = ToBytes();
        return a == 255 ? $"#{r:X2}{g:X2}{b:X2}" : $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }

    public uint ToRGBA()
    {
        var (r, g, b, a) = ToBytes();
        return (uint)(r << 24 | g << 16 | b << 8 | a);
    }

    // --- Predefined ---

    public static readonly ColorFRGBA Transparent = new(0, 0, 0, 0);
    public static readonly ColorFRGBA Black = new(0, 0, 0);
    public static readonly ColorFRGBA White = new(1, 1, 1);
    public static readonly ColorFRGBA Red = new(1, 0, 0);
    public static readonly ColorFRGBA Green = new(0, 1, 0);
    public static readonly ColorFRGBA Blue = new(0, 0, 1);
    public static readonly ColorFRGBA Yellow = new(1, 1, 0);
    public static readonly ColorFRGBA Cyan = new(0, 1, 1);
    public static readonly ColorFRGBA Magenta = new(1, 0, 1);
    public static readonly ColorFRGBA Gray = new(0.5f, 0.5f, 0.5f);
    public static readonly ColorFRGBA Orange = new(1f, 0.647f, 0f);
    public static readonly ColorFRGBA CornflowerBlue = new(0.392f, 0.584f, 0.929f);

    // --- Equality ---

    public bool Equals(ColorFRGBA other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is ColorFRGBA o && Equals(o);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"ColorFRGBA({R:F3}, {G:F3}, {B:F3}, {A:F3})";
}