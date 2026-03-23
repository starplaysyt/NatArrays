using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatLib.Core.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex : IEquatable<Vertex>
{
    public Point2 Position;
    public Color Color;
    public Point2 TexCoord;

    // --- Size / Offset info ---

    public static readonly int SizeInBytes = Unsafe.SizeOf<Vertex>();

    public static readonly int PositionOffset = (int)Marshal.OffsetOf<Vertex>(nameof(Position));

    public static readonly int ColorOffset = (int)Marshal.OffsetOf<Vertex>(nameof(Color));

    public static readonly int TexCoordOffset = (int)Marshal.OffsetOf<Vertex>(nameof(TexCoord));

    // --- Constructors ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex(Point2 position, Color color, Point2 texCoord)
    {
        Position = position;
        Color = color;
        TexCoord = texCoord;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex(float x, float y, Color color, float u, float v)
    {
        Position = new Point2(x, y);
        Color = color;
        TexCoord = new Point2(u, v);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex(Point2 position, Color color)
    {
        Position = position;
        Color = color;
        TexCoord = Point2.Zero;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex(Point2 position)
    {
        Position = position;
        Color = Color.White;
        TexCoord = Point2.Zero;
    }

    // --- With-methods (immutable modification) ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex WithPosition(Point2 position) => new(position, Color, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex WithColor(Color color) => new(Position, color, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex WithTexCoord(Point2 texCoord) => new(Position, Color, texCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex WithAlpha(float alpha) => new(Position, Color.WithAlpha(alpha), TexCoord);

    // --- Transform ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Translate(Point2 offset) =>
        new(new Point2(Position.Value + offset.Value), Color, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Scale(float scale) =>
        new(new Point2(Position.Value * scale), Color, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Scale(Point2 scale) =>
        new(new Point2(Position.Value * scale.Value), Color, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Rotate(float radians)
    {
        var cos = MathF.Cos(radians);
        var sin = MathF.Sin(radians);
        var x = Position.X * cos - Position.Y * sin;
        var y = Position.X * sin + Position.Y * cos;
        return new Vertex(new Point2(x, y), Color, TexCoord);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Transform(Matrix3x2 matrix) =>
        new(new Point2(Vector2.Transform(Position.Value, matrix)), Color, TexCoord);

    // --- Interpolation ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vertex Lerp(Vertex a, Vertex b, float t) =>
        new(
            Point2.Lerp(a.Position, b.Position, t),
            Color.Lerp(a.Color, b.Color, t),
            Point2.Lerp(a.TexCoord, b.TexCoord, t)
        );

    // --- Span interop (для GPU upload без копирования) ---

    /// <summary>
    /// Reinterpret Vertex[] as ReadOnlySpan&lt;byte&gt;
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> AsBytes(ReadOnlySpan<Vertex> vertices) =>
        MemoryMarshal.AsBytes(vertices);

    /// <summary>
    /// Reinterpret byte[] back as Span&lt;Vertex&gt;.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<Vertex> FromBytes(Span<byte> bytes) =>
        MemoryMarshal.Cast<byte, Vertex>(bytes);

    // --- Equality ---
    public static bool operator ==(Vertex a, Vertex b) =>
        a.Position == b.Position && a.Color == b.Color && a.TexCoord == b.TexCoord;

    public static bool operator !=(Vertex a, Vertex b) => !(a == b);

    public bool Equals(Vertex other) =>
        Position.Equals(other.Position) &&
        Color.Equals(other.Color) &&
        TexCoord.Equals(other.TexCoord);

    public override bool Equals(object? obj) => obj is Vertex o && Equals(o);

    public override int GetHashCode() => HashCode.Combine(
        Position.GetHashCode(),
        Color.GetHashCode(),
        TexCoord.GetHashCode());

    public override string ToString() =>
        $"Vertex(Pos:{Position}, Col:{Color}, UV:{TexCoord})";
}