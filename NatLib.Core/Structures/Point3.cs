using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct Point3 : IEquatable<Point3>
{
    public Vector3 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point3(float x, float y, float z)
    {
        Value = new Vector3(x, y, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point3(Vector3 value)
    {
        Value = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point3(Point2 xy, float z)
    {
        Value = new Vector3(xy.Value, z);
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

    public float Z
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.Z = value;
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

    public Point3 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Vector3.Normalize(Value));
    }

    public Point2 XY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X, Value.Y);
    }

    public Point2 XZ
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X, Value.Z);
    }

    public Point2 YZ
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.Y, Value.Z);
    }

    // --- Operators ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator +(Point3 a, Point3 b) => new(a.Value + b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator -(Point3 a, Point3 b) => new(a.Value - b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator *(Point3 a, float s) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator *(float s, Point3 a) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator *(Point3 a, Point3 b) => new(a.Value * b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator /(Point3 a, float s) => new(a.Value / s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator /(Point3 a, Point3 b) => new(a.Value / b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator -(Point3 a) => new(-a.Value);

    public static bool operator ==(Point3 a, Point3 b) => a.Value == b.Value;

    public static bool operator !=(Point3 a, Point3 b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Point3 p) => p.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Point3(Vector3 v) => new(v);

    // --- Methods ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Point3 a, Point3 b) => Vector3.Distance(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Point3 a, Point3 b) => Vector3.DistanceSquared(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Point3 a, Point3 b) => Vector3.Dot(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 Cross(Point3 a, Point3 b) => new(Vector3.Cross(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 Lerp(Point3 a, Point3 b, float t) => new(Vector3.Lerp(a.Value, b.Value, t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 Min(Point3 a, Point3 b) => new(Vector3.Min(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 Max(Point3 a, Point3 b) => new(Vector3.Max(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 Clamp(Point3 value, Point3 min, Point3 max) =>
        new(Vector3.Clamp(value.Value, min.Value, max.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 Reflect(Point3 direction, Point3 normal) =>
        new(Vector3.Reflect(direction.Value, normal.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 Abs(Point3 a) => new(Vector3.Abs(a.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 Transform(Point3 position, Matrix4x4 matrix) =>
        new(Vector3.Transform(position.Value, matrix));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 TransformNormal(Point3 normal, Matrix4x4 matrix) =>
        new(Vector3.TransformNormal(normal.Value, matrix));

    // --- Constants ---

    public static readonly Point3 Zero = new(Vector3.Zero);
    public static readonly Point3 One = new(Vector3.One);
    public static readonly Point3 UnitX = new(Vector3.UnitX);
    public static readonly Point3 UnitY = new(Vector3.UnitY);
    public static readonly Point3 UnitZ = new(Vector3.UnitZ);

    // --- Equality ---

    public bool Equals(Point3 other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is Point3 o && Equals(o);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"Point3({X}, {Y}, {Z})";
}