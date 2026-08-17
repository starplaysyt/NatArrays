using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatLib.Core.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct Point2 : IEquatable<Point2>
{
    public int X;
    public int Y;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2(int x, int y) =>
        (X, Y) = (x, y);
    
    // --- Operators ---
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator +(Point2 a, Point2 b) => new(a.X + b.X, a.Y + b.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator -(Point2 a, Point2 b) => new(a.X - b.X, a.Y - b.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator +(Point2 a, int s) => new(a.X + s, a.Y + s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator -(Point2 a, int s) => new(a.X - s, a.Y - s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator *(int s, Point2 a) => new(a.X * s, a.Y * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator *(Point2 a, Point2 b) => new(a.X * b.X, a.Y * b.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator /(Point2 a, int s) => new(a.X / s, a.Y / s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator /(Point2 a, Point2 b) => new(a.X / b.X, a.Y / b.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator %(Point2 a, int s) => new(a.X % s, a.Y % s);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator %(Point2 a, Point2 b) => new(a.X % b.X, a.Y % b.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2 operator -(Point2 a) => new(-a.X, -a.Y);

    public static bool operator ==(Point2 a, Point2 b) => a.X == b.X && a.Y == b.Y;

    public static bool operator !=(Point2 a, Point2 b) => a.X != b.X || a.Y != b.Y;
    
    // --- Conversions ---
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Point2F(Point2 a) => new(a.X, a.Y);
    
    // --- Methods ---
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
    
    // --- Constants ---
    
    public static readonly Point2 Zero = new(0, 0);
    public static readonly Point2 One = new(1, 1);
    public static readonly Point2 UnitX = new(1, 0);
    public static readonly Point2 UnitY = new(0, 1);
    
    // --- Equality ---
    
    public bool Equals(Point2 other) => this == other;

    public override bool Equals(object? obj) => obj is Point2 other && Equals(other);
    
    public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
    
    public override string ToString() => $"Point2({X}, {Y})";
}