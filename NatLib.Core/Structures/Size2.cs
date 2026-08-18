using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatLib.Core.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct Size2 : IEquatable<Size2>
{
    public int Width;
    public int Height;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size2(int width, int height) =>
        (Width, Height) = (width, height);
    
    // --- Operators ---
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator +(Size2 a, Size2 b) => new(a.Width + b.Width, a.Height + b.Height);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator -(Size2 a, Size2 b) => new(a.Width - b.Width, a.Height - b.Height);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator +(Size2 a, int s) => new(a.Width + s, a.Height + s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator -(Size2 a, int s) => new(a.Width - s, a.Height - s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator *(int s, Size2 a) => new(a.Width * s, a.Height * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator *(Size2 a, Size2 b) => new(a.Width * b.Width, a.Height * b.Height);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator /(Size2 a, int s) => new(a.Width / s, a.Height / s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator /(Size2 a, Size2 b) => new(a.Width / b.Width, a.Height / b.Height);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator %(Size2 a, int s) => new(a.Width % s, a.Height % s);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator %(Size2 a, Size2 b) => new(a.Width % b.Width, a.Height % b.Height);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator -(Size2 a) => new(-a.Width, -a.Height);

    public static bool operator ==(Size2 a, Size2 b) => a.Width == b.Width && a.Height == b.Height;

    public static bool operator !=(Size2 a, Size2 b) => a.Width != b.Width || a.Height != b.Height;
    
    // --- Conversions ---
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Size2F(Size2 a) => new(a.Width, a.Height);
    
    // --- Methods ---
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out int width, out int height) => (width, height) = (Width, Height);
    
    // --- Constants ---
    
    public static readonly Size2 Zero = new(0, 0);
    public static readonly Size2 One = new(1, 1);
    public static readonly Size2 UnitWidth = new(1, 0);
    public static readonly Size2 UnitY = new(0, 1);
    
    // --- Equality ---
    
    public bool Equals(Size2 other) => this == other;

    public override bool Equals(object? obj) => obj is Size2 other && Equals(other);
    
    public override int GetHashCode() => HashCode.Combine(Width.GetHashCode(), Height.GetHashCode());
    
    public override string ToString() => $"Size2({Width}, {Height})";
}