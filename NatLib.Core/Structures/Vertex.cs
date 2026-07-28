using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NatLib.Core.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex : IEquatable<Vertex>
{
    public Point2F Position;
    public ColorFRGBA ColorFrgba;
    public Point2F TexCoord;

    // --- Size / Offset info ---

    public static readonly int SizeInBytes = Unsafe.SizeOf<Vertex>();

    public static readonly int PositionOffset = (int)Marshal.OffsetOf<Vertex>(nameof(Position));

    public static readonly int ColorOffset = (int)Marshal.OffsetOf<Vertex>(nameof(ColorFrgba));

    public static readonly int TexCoordOffset = (int)Marshal.OffsetOf<Vertex>(nameof(TexCoord));

    // --- Constructors ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex(Point2F position, ColorFRGBA colorFrgba, Point2F texCoord)
    {
        Position = position;
        ColorFrgba = colorFrgba;
        TexCoord = texCoord;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex(float x, float y, ColorFRGBA colorFrgba, float u, float v)
    {
        Position = new Point2F(x, y);
        ColorFrgba = colorFrgba;
        TexCoord = new Point2F(u, v);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex(Point2F position, ColorFRGBA colorFrgba)
    {
        Position = position;
        ColorFrgba = colorFrgba;
        TexCoord = Point2F.Zero;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex(Point2F position)
    {
        Position = position;
        ColorFrgba = ColorFRGBA.White;
        TexCoord = Point2F.Zero;
    }

    // --- With-methods (immutable modification) ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex WithPosition(Point2F position) => new(position, ColorFrgba, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex WithColor(ColorFRGBA colorFrgba) => new(Position, colorFrgba, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex WithTexCoord(Point2F texCoord) => new(Position, ColorFrgba, texCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex WithAlpha(float alpha) => new(Position, ColorFrgba.WithAlpha(alpha), TexCoord);

    // --- Transform ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Translate(Point2F offset) =>
        new(new Point2F(Position.Value + offset.Value), ColorFrgba, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Scale(float scale) =>
        new(new Point2F(Position.Value * scale), ColorFrgba, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Scale(Point2F scale) =>
        new(new Point2F(Position.Value * scale.Value), ColorFrgba, TexCoord);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Rotate(float radians)
    {
        var cos = MathF.Cos(radians);
        var sin = MathF.Sin(radians);
        var x = Position.X * cos - Position.Y * sin;
        var y = Position.X * sin + Position.Y * cos;
        return new Vertex(new Point2F(x, y), ColorFrgba, TexCoord);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vertex Transform(Matrix3x2 matrix) =>
        new(new Point2F(Vector2.Transform(Position.Value, matrix)), ColorFrgba, TexCoord);

    // --- Interpolation ---

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vertex Lerp(Vertex a, Vertex b, float t) =>
        new(
            Point2F.Lerp(a.Position, b.Position, t),
            ColorFRGBA.Lerp(a.ColorFrgba, b.ColorFrgba, t),
            Point2F.Lerp(a.TexCoord, b.TexCoord, t)
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
        a.Position == b.Position && a.ColorFrgba == b.ColorFrgba && a.TexCoord == b.TexCoord;

    public static bool operator !=(Vertex a, Vertex b) => !(a == b);

    public bool Equals(Vertex other) =>
        Position.Equals(other.Position) &&
        ColorFrgba.Equals(other.ColorFrgba) &&
        TexCoord.Equals(other.TexCoord);

    public override bool Equals(object? obj) => obj is Vertex o && Equals(o);

    public override int GetHashCode() => HashCode.Combine(
        Position.GetHashCode(),
        ColorFrgba.GetHashCode(),
        TexCoord.GetHashCode());

    public override string ToString() =>
        $"Vertex(Pos:{Position}, Col:{ColorFrgba}, UV:{TexCoord})";
}