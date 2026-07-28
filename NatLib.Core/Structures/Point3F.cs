using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct Point3F : IEquatable<Point3F>
{
    public Vector3 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point3F(float x, float y, float z)
    {
        Value = new Vector3(x, y, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point3F(Vector3 value)
    {
        Value = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point3F(Point2F xy, float z)
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

    public Point3F Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Vector3.Normalize(Value));
    }

    public Point2F XY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X, Value.Y);
    }

    public Point2F XZ
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X, Value.Z);
    }

    public Point2F YZ
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.Y, Value.Z);
    }

    // --- Operators ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F operator +(Point3F a, Point3F b) => new(a.Value + b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F operator -(Point3F a, Point3F b) => new(a.Value - b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F operator *(Point3F a, float s) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F operator *(float s, Point3F a) => new(a.Value * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F operator *(Point3F a, Point3F b) => new(a.Value * b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F operator /(Point3F a, float s) => new(a.Value / s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F operator /(Point3F a, Point3F b) => new(a.Value / b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F operator -(Point3F a) => new(-a.Value);

    public static bool operator ==(Point3F a, Point3F b) => a.Value == b.Value;

    public static bool operator !=(Point3F a, Point3F b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Point3F p) => p.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Point3F(Vector3 v) => new(v);

    // --- Methods ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Point3F a, Point3F b) => Vector3.Distance(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Point3F a, Point3F b) => Vector3.DistanceSquared(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Point3F a, Point3F b) => Vector3.Dot(a.Value, b.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F Cross(Point3F a, Point3F b) => new(Vector3.Cross(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F Lerp(Point3F a, Point3F b, float t) => new(Vector3.Lerp(a.Value, b.Value, t));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F Min(Point3F a, Point3F b) => new(Vector3.Min(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F Max(Point3F a, Point3F b) => new(Vector3.Max(a.Value, b.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F Clamp(Point3F value, Point3F min, Point3F max) =>
        new(Vector3.Clamp(value.Value, min.Value, max.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F Reflect(Point3F direction, Point3F normal) =>
        new(Vector3.Reflect(direction.Value, normal.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F Abs(Point3F a) => new(Vector3.Abs(a.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F Transform(Point3F position, Matrix4x4 matrix) =>
        new(Vector3.Transform(position.Value, matrix));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3F TransformNormal(Point3F normal, Matrix4x4 matrix) =>
        new(Vector3.TransformNormal(normal.Value, matrix));

    // --- Constants ---

    public static readonly Point3F Zero = new(Vector3.Zero);
    public static readonly Point3F One = new(Vector3.One);
    public static readonly Point3F UnitX = new(Vector3.UnitX);
    public static readonly Point3F UnitY = new(Vector3.UnitY);
    public static readonly Point3F UnitZ = new(Vector3.UnitZ);

    // --- Equality ---

    public bool Equals(Point3F other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is Point3F o && Equals(o);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"Point3F({X}, {Y}, {Z})";
}