using System.Numerics;
using System.Runtime.CompilerServices;

namespace NatLib.Core.Structures;

public struct RectangleF : IEquatable<RectangleF>
{
    public Vector4 Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RectangleF(float x, float y, float width, float height)
    {
        Value = new Vector4(x, y, width, height);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RectangleF(Point2F position, Size2F size)
    {
        Value = new Vector4(position.Value, size.Value.X, size.Value.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RectangleF(Vector4 value)
    {
        Value = value;
    }

    // --- Properties: components ---

    public float X
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.X;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => Value.X = value; }

    public float Y
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Y;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => Value.Y = value; }

    public float Width
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Z;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => Value.Z = value; }

    public float Height
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.W;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set => Value.W = value; }

    // --- Properties: edges ---

    public float Left
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.X; }

    public float Top
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Y; }

    public float Right
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.X + Value.Z; }

    public float Bottom
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Y + Value.W; }

    // --- Properties: composite ---

    public Point2F Position
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X, Value.Y);
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set
      { Value.X = value.X;
        Value.Y = value.Y; } }

    public Size2F Size
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.Z, Value.W);
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set
      { Value.Z = value.Width;
        Value.W = value.Height; } }

    public float Area
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Value.Z * Value.W; }

    public float Perimeter
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => 2f * (Value.Z + Value.W); }

    // --- Properties: key points ---

    public Point2F TopLeft
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X, Value.Y); }

    public Point2F TopRight
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X + Value.Z, Value.Y); }

    public Point2F BottomLeft
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X, Value.Y + Value.W); }

    public Point2F BottomRight
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X + Value.Z, Value.Y + Value.W); }

    public Point2F Center
    { [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => new(Value.X + Value.Z * 0.5f, Value.Y + Value.W * 0.5f); }

    // --- Operators ---

    public static bool operator ==(RectangleF a, RectangleF b) => a.Value == b.Value;

    public static bool operator !=(RectangleF a, RectangleF b) => a.Value != b.Value;

    // --- Conversions ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(RectangleF r) => r.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator RectangleF(Vector4 v) => new(v);

    // --- Methods: tests ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Point2F point) =>
        point.X >= Left && point.X <= Right &&
        point.Y >= Top && point.Y <= Bottom;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(RectangleF other) =>
        other.Left >= Left && other.Right <= Right &&
        other.Top >= Top && other.Bottom <= Bottom;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Intersects(RectangleF other) =>
        Left < other.Right && Right > other.Left &&
        Top < other.Bottom && Bottom > other.Top;

    // --- Methods: geometry ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RectangleF? Intersect(RectangleF a, RectangleF b)
    {
        var x1 = MathF.Max(a.Left, b.Left);
        var y1 = MathF.Max(a.Top, b.Top);
        var x2 = MathF.Min(a.Right, b.Right);
        var y2 = MathF.Min(a.Bottom, b.Bottom);

        if (x2 >= x1 && y2 >= y1)
            return new RectangleF(x1, y1, x2 - x1, y2 - y1);
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RectangleF Union(RectangleF a, RectangleF b)
    {
        var x1 = MathF.Min(a.Left, b.Left);
        var y1 = MathF.Min(a.Top, b.Top);
        var x2 = MathF.Max(a.Right, b.Right);
        var y2 = MathF.Max(a.Bottom, b.Bottom);
        return new RectangleF(x1, y1, x2 - x1, y2 - y1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RectangleF Inflate(float h, float v) =>
        new(Value.X - h, Value.Y - v, Value.Z + h * 2f, Value.W + v * 2f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RectangleF Offset(float dx, float dy) =>
        new(Value.X + dx, Value.Y + dy, Value.Z, Value.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RectangleF Offset(Point2F delta) =>
        new(Value.X + delta.X, Value.Y + delta.Y, Value.Z, Value.W);

    // --- Methods: factory ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RectangleF FromCenter(Point2F center, Size2F size) =>
        new(center.X - size.Width * 0.5f,
            center.Y - size.Height * 0.5f,
            size.Width,
            size.Height);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RectangleF FromCorners(Point2F a, Point2F b)
    {
        var x = MathF.Min(a.X, b.X);
        var y = MathF.Min(a.Y, b.Y);
        return new RectangleF(x, y, MathF.Abs(b.X - a.X), MathF.Abs(b.Y - a.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RectangleF FromEdges(float left, float top, float right, float bottom) =>
        new(left, top, right - left, bottom - top);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RectangleF Lerp(RectangleF a, RectangleF b, float t) =>
        new(Vector4.Lerp(a.Value, b.Value, t));

    // --- Constants ---

    public static readonly RectangleF Zero = new(0, 0, 0, 0);
    public static readonly RectangleF Unit = new(0, 0, 1, 1);

    // --- Equality ---

    public bool Equals(RectangleF other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is RectangleF o && Equals(o);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"RectangleF({X}, {Y}, {Width}, {Height})";
}