using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NatLib.Core.Enums;

namespace NatLib.Core.Utils;

public static class StringUtils
{
    /// <summary>
    /// Returns new string that is fixed by length, and aligned to right.
    /// </summary>
    /// <param name="str"> The string, that will be fixed and aligned. </param>
    /// <param name="length"> The length, to what a string will be fixed. </param>
    /// <param name="character"> The character, that will be used to fill empty (generated) space. </param>
    /// <returns> The fixed and aligned string. </returns>
    public static string FixRight(string str, int length, char character = ' ')
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (str.Length == length) return str;
        if (length == 0) return string.Empty;

        return string.Create(length, (Source: str, Character: character), SpanCharUtils.FixSpanRight);
    }

    /// <summary>
    /// Returns new string that is fixed by length, and aligned to left.
    /// </summary>
    /// <param name="str"> The string, that will be fixed and aligned. </param>
    /// <param name="length"> The length, to what a string will be fixed. </param>
    /// <param name="character"> The character, that will be used to fill empty (generated) space. </param>
    /// <returns> The fixed and aligned string. </returns>
    public static string Fix(string str, int length, char character = ' ')
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (str.Length == length) return str;
        if (length == 0) return string.Empty;

        return string.Create(length, (Source: str, Character: character), SpanCharUtils.FixSpanLeft);
    }

    /// <summary>
    /// Returns new string that is fixed by length, and aligned to center.
    /// </summary>
    /// <param name="str"> The string, that will be fixed and aligned. </param>
    /// <param name="length"> The length, to what a string will be fixed. </param>
    /// <param name="character"> The character, that will be used to fill empty (generated) space. </param>
    /// <returns> The fixed and aligned string. </returns>
    public static string FixCenter(string str, int length, char character = ' ')
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (str.Length == length) return str;
        if (length == 0) return string.Empty;

        return string.Create(length, (Source: str, Character: character), SpanCharUtils.FixSpanCenter);
    }

    /// <summary>
    /// Returns new string that is fixed by length, and aligned to side, depended on alignment value.
    /// </summary>
    /// <param name="str"> The string, that will be fixed and aligned. </param>
    /// <param name="length"> The length, to what a string will be fixed. </param>
    /// <param name="character"> The character, that will be used to fill empty (generated) space. </param>
    /// <param name="alignment"> The alignment orientation. </param>
    /// <returns> The fixed and aligned string. </returns>
    /// <exception cref="ArgumentOutOfRangeException"> throws when length is negative.</exception>
    public static string Fix(string str, int length, char character, Alignment alignment)
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (str.Length == length) return str;
        if (length == 0) return string.Empty;

        return string.Create(
            length,
            (Source: str, Character: character),
            alignment switch
            {
                Alignment.Begin => SpanCharUtils.FixSpanLeft,
                Alignment.Center => SpanCharUtils.FixSpanCenter,
                Alignment.End => SpanCharUtils.FixSpanRight,
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
            });
    }

    /// <summary>
    /// Generates a new string with specified length, filled with specified character
    /// </summary>
    /// <param name="character"> The char value, the string will contain</param>
    /// <param name="length"> The length of desired string. </param>
    /// <returns> New string with specified length and content. </returns>
    /// <exception cref="ArgumentOutOfRangeException"> throws when length is negative. </exception>
    public static string Generate(char character, int length)
    {
        return length switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(length)),
            0 => string.Empty,
            _ => string.Create(length, character, static (dst, ch) => dst.Fill(ch))
        };
    }

    /// <summary>
    /// Generates a formatted string consisting of repeated character segments
    /// with specified lengths, separated by separator characters and enclosed
    /// by left and right boundary characters. <br/><br/>
    /// Simple example, what can describe the result it can give:
    /// [--------|-----|-----------|-------]
    /// </summary>
    /// <param name="leftSide"> The left-border character. </param>
    /// <param name="rightSide"> The right-border character. </param>
    /// <param name="middleFill"> The character, that will be placed in middle. </param>
    /// <param name="middleSeparator"> The character, that will be used as separator in the middle. </param>
    /// <param name="lengths"> The length array, that will define length of segments. </param>
    /// <returns> The generated strings. </returns>
    public static string GenerateJoin(char leftSide, char rightSide, char middleFill, char middleSeparator, int[] lengths)
    {
        return string.Create(
            1 + lengths.Sum(i => i + 3),
            (LeftSide: leftSide, RightSize: rightSide, MiddleFill: middleFill,
                MiddleSeparator: middleSeparator, RepeatLengths: lengths),
            SpanCharUtils.GenerateJoinSpan);
    }

    /// <summary>
    /// Generates a formatted string by joining input strings padded or trimmed
    /// to specified lengths, separated by a separator character and aligned
    /// according to the provided alignment option.
    /// </summary>
    /// <param name="array"> The array of strings, that will be placed to result string. </param>
    /// <param name="lengths"> The array of lengths, that will direct final length of given strings. </param>
    /// <param name="separator"> The character that will be used to separate given strings. </param>
    /// <param name="alignment"> The side of the strings alignment. </param>
    /// <returns> Generated string. </returns>
    /// <exception cref="InvalidOperationException"> Throws when lengths of array and lengths are not equal. </exception>
    public static string WrapJoin(string[] array, int[] lengths, char separator, Alignment alignment = Alignment.Begin)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentNullException.ThrowIfNull(lengths);

        if (array.Length != lengths.Length)
            throw new InvalidOperationException("Lengths of string array and int array should be equal.");

        return string.Create(
            lengths.Sum() + (lengths.Length - 1) * 3 + 4,
            (array, lengths, separator, alignment),
            SpanCharUtils.WrapJoinSpan);
    }
}