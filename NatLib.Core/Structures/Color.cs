using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct Color : IEquatable<Color>
{
    public Vector4 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(float r, float g, float b, float a = 1f) => Value = new Vector4(r, g, b, a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(Vector4 value) => Value = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color(Vector3 rgb, float a = 1f) => Value = new Vector4(rgb, a);

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

    public Color Clamped
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Vector4.Clamp(Value, Vector4.Zero, Vector4.One));
    }

    public Color Opaque
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
    public static Color FromBytes(byte r, byte g, byte b, byte a = 255) =>
        new(r / 255f, g / 255f, b / 255f, a / 255f);

    public static Color FromHex(string hex)
    {
        hex = hex.TrimStart('#');
        byte r = Convert.ToByte(hex[0..2], 16);
        byte g = Convert.ToByte(hex[2..4], 16);
        byte b = Convert.ToByte(hex[4..6], 16);
        byte a = hex.Length >= 8 ? Convert.ToByte(hex[6..8], 16) : (byte)255;
        return FromBytes(r, g, b, a);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color FromHSV(float h, float s, float v)
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

        return new Color(r + m, g + m, b + m);
    }

    // --- Operators ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(Color a, Color b) => new(a.Value + b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(Color a, Color b) => new(a.Value - b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(Color a, Color b) => new(a.Value * b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(Color a, float s) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(float s, Color a) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator /(Color a, float s) => new(a.Value / s);

    public static bool operator ==(Color a, Color b) => a.Value == b.Value;
    public static bool operator !=(Color a, Color b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(Color c) => c.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Color(Vector4 v) => new(v);

    // --- Methods ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Lerp(Color a, Color b, float t) => new(Vector4.Lerp(a.Value, b.Value, t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Min(Color a, Color b) => new(Vector4.Min(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Max(Color a, Color b) => new(Vector4.Max(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithAlpha(float alpha) => new(Value.X, Value.Y, Value.Z, alpha);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithR(float r) => new(r, Value.Y, Value.Z, Value.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithG(float g) => new(Value.X, g, Value.Z, Value.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithB(float b) => new(Value.X, Value.Y, b, Value.W);

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

    public static readonly Color Transparent     = new(0, 0, 0, 0);
    public static readonly Color Black           = new(0, 0, 0);
    public static readonly Color White           = new(1, 1, 1);
    public static readonly Color Red             = new(1, 0, 0);
    public static readonly Color Green           = new(0, 1, 0);
    public static readonly Color Blue            = new(0, 0, 1);
    public static readonly Color Yellow          = new(1, 1, 0);
    public static readonly Color Cyan            = new(0, 1, 1);
    public static readonly Color Magenta         = new(1, 0, 1);
    public static readonly Color Gray            = new(0.5f, 0.5f, 0.5f);
    public static readonly Color Orange          = new(1f, 0.647f, 0f);
    public static readonly Color CornflowerBlue  = new(0.392f, 0.584f, 0.929f);

    // --- Equality ---

    public bool Equals(Color other) => Value.Equals(other.Value);
    public override bool Equals(object? obj) => obj is Color o && Equals(o);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => $"Color({R:F3}, {G:F3}, {B:F3}, {A:F3})";
}