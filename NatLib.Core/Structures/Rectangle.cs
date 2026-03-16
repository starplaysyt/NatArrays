using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct Rectangle : IEquatable<Rectangle>
{
    public Vector4 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rectangle(float x, float y, float width, float height) =>
        Value = new Vector4(x, y, width, height);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rectangle(Point2 position, Size2 size) =>
        Value = new Vector4(position.Value, size.Value.X, size.Value.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rectangle(Vector4 value) => Value = value;

    // --- Properties: components ---

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

    public float Width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Z;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.Z = value;
    }

    public float Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.W;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Value.W = value;
    }

    // --- Properties: edges ---

    public float Left
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.X;
    }

    public float Top
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Y;
    }

    public float Right
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.X + Value.Z;
    }

    public float Bottom
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Y + Value.W;
    }

    // --- Properties: composite ---

    public Point2 Position
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X, Value.Y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { Value.X = value.X; Value.Y = value.Y; }
    }

    public Size2 Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.Z, Value.W);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { Value.Z = value.Width; Value.W = value.Height; }
    }

    public float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Z * Value.W;
    }

    public float Perimeter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 2f * (Value.Z + Value.W);
    }

    // --- Properties: key points ---

    public Point2 TopLeft
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X, Value.Y);
    }

    public Point2 TopRight
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X + Value.Z, Value.Y);
    }

    public Point2 BottomLeft
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X, Value.Y + Value.W);
    }

    public Point2 BottomRight
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X + Value.Z, Value.Y + Value.W);
    }

    public Point2 Center
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Value.X + Value.Z * 0.5f, Value.Y + Value.W * 0.5f);
    }

    // --- Operators ---

    public static bool operator ==(Rectangle a, Rectangle b) => a.Value == b.Value;
    public static bool operator !=(Rectangle a, Rectangle b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(Rectangle r) => r.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rectangle(Vector4 v) => new(v);

    // --- Methods: tests ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Point2 point) =>
        point.X >= Left && point.X <= Right &&
        point.Y >= Top  && point.Y <= Bottom;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Rectangle other) =>
        other.Left >= Left && other.Right  <= Right &&
        other.Top  >= Top  && other.Bottom <= Bottom;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Intersects(Rectangle other) =>
        Left < other.Right  && Right  > other.Left &&
        Top  < other.Bottom && Bottom > other.Top;

    // --- Methods: geometry ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rectangle? Intersect(Rectangle a, Rectangle b)
    {
        float x1 = MathF.Max(a.Left, b.Left);
        float y1 = MathF.Max(a.Top,  b.Top);
        float x2 = MathF.Min(a.Right,  b.Right);
        float y2 = MathF.Min(a.Bottom, b.Bottom);

        if (x2 >= x1 && y2 >= y1)
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rectangle Union(Rectangle a, Rectangle b)
    {
        float x1 = MathF.Min(a.Left, b.Left);
        float y1 = MathF.Min(a.Top,  b.Top);
        float x2 = MathF.Max(a.Right,  b.Right);
        float y2 = MathF.Max(a.Bottom, b.Bottom);
        return new Rectangle(x1, y1, x2 - x1, y2 - y1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rectangle Inflate(float h, float v) =>
        new(Value.X - h, Value.Y - v, Value.Z + h * 2f, Value.W + v * 2f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rectangle Offset(float dx, float dy) =>
        new(Value.X + dx, Value.Y + dy, Value.Z, Value.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rectangle Offset(Point2 delta) =>
        new(Value.X + delta.X, Value.Y + delta.Y, Value.Z, Value.W);

    // --- Methods: factory ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rectangle FromCenter(Point2 center, Size2 size) =>
        new(center.X - size.Width * 0.5f, center.Y - size.Height * 0.5f,
            size.Width, size.Height);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rectangle FromCorners(Point2 a, Point2 b)
    {
        float x = MathF.Min(a.X, b.X);
        float y = MathF.Min(a.Y, b.Y);
        return new Rectangle(x, y, MathF.Abs(b.X - a.X), MathF.Abs(b.Y - a.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rectangle FromEdges(float left, float top, float right, float bottom) =>
        new(left, top, right - left, bottom - top);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rectangle Lerp(Rectangle a, Rectangle b, float t) =>
        new(Vector4.Lerp(a.Value, b.Value, t));

    // --- Constants ---

    public static readonly Rectangle Zero = new(0, 0, 0, 0);
    public static readonly Rectangle Unit = new(0, 0, 1, 1);

    // --- Equality ---

    public bool Equals(Rectangle other) => Value.Equals(other.Value);
    public override bool Equals(object? obj) => obj is Rectangle o && Equals(o);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => $"Rectangle({X}, {Y}, {Width}, {Height})";
}