using NatLib.Core.Unification;

namespace NatLib.Core.Utils;

/// <summary>
/// Class, used for generating structural units. Every operation consumes Span&gt;char&lt;,
/// this Span SHOULD HAVE LENGTH of PreferableWidth from instance of StringStructuralConfiguration.
/// </summary>
public static class StringStructuralUtils
{
    public static void WriteFixedStringNext(Span<char> source, ReadOnlySpan<char> str, int width, char empty)
    {
        var strLen = Math.Min(str.Length, width);
        str[..strLen].CopyTo(source);
        if (str.Length < width)
            source[str.Length..width].Fill(empty);
        else
            source[^3..].Fill('.');
    }

    public static void WriteTopBorder(Span<char> source)
    {
        var (left, center, right, width) = StringStructuralConfiguration.Instance.DeconstructTop();
        source[0] = left;
        source[1..(width - 1)].Fill(center);
        source[width - 1] = right;
    }

    public static void WriteMessageInBounds(Span<char> source, ReadOnlySpan<char> message)
    {
        var (side, center, width) = StringStructuralConfiguration.Instance.DeconstructMiddle();

        var strLen = Math.Min(message.Length, width - 4);

        message[..strLen].CopyTo(source[2..]);
        source.Slice(strLen + 2, width - strLen - 2).Fill(center);
        if (message.Length > width - 4) source[(width - 5)..^2].Fill('.');

        source[0] = side;
        source[1] = center;
        source[^1] = side;
    }

    public static void WriteSeparator(Span<char> source)
    {
        var (left, center, right, width) = StringStructuralConfiguration.Instance.DeconstructSeparator();
        source[0] = left;
        source[1..(width - 1)].Fill(center);
        source[width - 1] = right;
    }

    public static void WriteBottomBorder(Span<char> source)
    {
        var (left, center, right, width) = StringStructuralConfiguration.Instance.DeconstructBottom();
        source[0] = left;
        source[1..(width - 1)].Fill(center);
        source[width - 1] = right;
    }
}