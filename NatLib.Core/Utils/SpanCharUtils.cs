using System.Diagnostics.CodeAnalysis;
using NatLib.Core.Enums;

namespace NatLib.Core.Utils;

public static class SpanCharUtils
{
    public static void FixSpan(Span<char> dst, (string Source, char Character) state, Alignment alignment)
    {
        switch (alignment)
        {
            case Alignment.Begin:
                FixSpanLeft(dst, state);
                return;
            case Alignment.Center:
                FixSpanCenter(dst, state);
                return;
            case Alignment.End:
                FixSpanRight(dst, state);
                return;
            default:
                return;
        }
    }

    public static void FixSpanLeft(Span<char> dst, (string Source, char Character) state)
    {
        var span = state.Source.AsSpan();
        var copy = Math.Min(span.Length, dst.Length);

        span[..copy].CopyTo(dst); // Copying existed part.

        if (copy < dst.Length) // Filling the rest. Do nothing when there is nothing to fill.
            dst[copy..].Fill(state.Character);
    }

    public static void FixSpanRight(Span<char> dst, (string Source, char Character) state)
    {
        var span = state.Source.AsSpan();
        var copy = Math.Min(span.Length, dst.Length);
        var offset = dst.Length - copy;

        if (copy < dst.Length)
            dst[..offset].Fill(state.Character); // Filling with padding.

        span[..copy].CopyTo(dst[offset..]); // Copying with padding.
    }

    public static void FixSpanCenter(Span<char> dst, (string Source, char Character) state)
    {
        var span = state.Source.AsSpan();
        var copy = Math.Min(span.Length, dst.Length);

        var totalPad = dst.Length - copy;
        var leftPad = totalPad / 2;
        var rightPad = totalPad - leftPad;

        dst[..leftPad].Fill(state.Character);

        span[..copy].CopyTo(dst[leftPad..]);

        dst.Slice(leftPad + copy, rightPad).Fill(state.Character);
    }

    public static void GenerateJoinSpan(Span<char> dst,
        (char LeftSide, char RightSide, char MiddleFill, char MiddleSeparator, int[] Lengths) state)
    {
        dst[0] = state.LeftSide;
        dst[^1] = state.RightSide;
        var lengths = state.Lengths;
        dst = dst[1..^1];

        var pos = 0;

        for (var i = 0; i < lengths.Length; i++)
        {
            var len = lengths[i];

            dst.Slice(pos, len + 2).Fill(state.MiddleFill);

            pos += len + 2;

            if (i < lengths.Length - 1) // on last element
            {
                dst[pos] = state.MiddleSeparator;
                pos++;
            }
        }
    }

    public static void WrapJoinSpan(Span<char> dst, (string[] Array, int[] Lengths, char Separator, Alignment Alignment) state)
    {
        var array = state.Array.AsSpan();
        var lengths = state.Lengths.AsSpan();
        var separator = state.Separator;
        var alignment = state.Alignment;

        var lastLocation = 0;
        for (var i = 0; i < array.Length; i++)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(lengths[i]);
            var localDst = dst.Slice(lastLocation, lengths[i] + 3);
            localDst[0] = separator;
            localDst[1] = ' ';
            var advanceDst = localDst.Slice(2, lengths[i]);
            FixSpan(advanceDst, (array[i], ' '), alignment);
            localDst[^1] = ' ';
            lastLocation += lengths[i] + 3;
        }

        dst[^2] = ' ';
        dst[^1] = separator;
    }

    public static int TryFormat<T>(T obj, Span<char> destination, [StringSyntax("StringFormat")] string format) where T : ISpanFormattable
        => obj.TryFormat(destination, out var charsWritten, format, null) ? charsWritten : -1;

    public static int TryCopy(string obj, Span<char> destination)
    {
        obj.TryCopyTo(destination);
        return Math.Min(obj.Length, destination.Length);
    }

    public static int TryCopy(ReadOnlySpan<char> obj, Span<char> destination)
    {
        // FIXED: In special circumstances function may fail to put obj to destination
        var length = Math.Min(obj.Length, destination.Length);
        obj[..length].CopyTo(destination);
        return length;
    }
}