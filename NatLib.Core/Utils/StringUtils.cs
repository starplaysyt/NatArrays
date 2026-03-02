using System.Reflection;
using NatLib.Core.Enums;

namespace NatLib.Core.Utils;

public static class StringUtils
{
    #region Fix Span Methods
    
    private static void FixSpan(Span<char> dst, (string Source, char Character) state, Alignment alignment)
    {
        switch (alignment)
        {
            case Alignment.Begin:
                FixSpanLeft(dst, state);
                break;
            case Alignment.Center:
                FixSpanCenter(dst, state);
                break;
            case Alignment.End:
                FixSpanRight(dst, state);
                break;
            default:
                return;
        }
    }
    
    private static void FixSpanLeft(Span<char> dst, (string Source, char Character) state)
    {
        var span = state.Source.AsSpan();
        var copy = Math.Min(span.Length, dst.Length); 
            
        span[..copy].CopyTo(dst); // Copying existed part.

        if (copy < dst.Length) // Filling the rest. Do nothing when there is nothing to fill.
            dst[copy..].Fill(state.Character);  
    }
    
    private static void FixSpanRight(Span<char> dst, (string Source, char Character) state)
    {
        var span = state.Source.AsSpan();
        var copy = Math.Min(span.Length, dst.Length);
        var offset = dst.Length - copy;
        
        if (copy < dst.Length)
            dst[..offset].Fill(state.Character); // Filling with padding.
        
        span[..copy].CopyTo(dst[offset..]); // Copying with padding.
    }
    
    private static void FixSpanCenter(Span<char> dst, (string Source, char Character) state)
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
    
    #endregion

    #region String Extensions (Fix Group)

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
        
        return string.Create(length, (Source: str, Character: character), FixSpanRight);
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

        return string.Create(length, (Source: str, Character: character), FixSpanLeft);
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
        
        return string.Create(length, (Source: str, Character: character), FixSpanCenter);
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
            alignment switch {
                Alignment.Begin => FixSpanLeft,
                Alignment.Center => FixSpanCenter,
                Alignment.End => FixSpanRight,
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
            });
    }

    #endregion
    
    #region Generate Method 
    
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
    /// </summary>
    /// <param name="dst"></param>
    /// <param name="state"></param>
    private static void GenerateJoinSpan(Span<char> dst,
        (char LeftSide, char RightSide, char MiddleFill, char MiddleSeparator, int[] Lengths) state)
    {
        dst[0] = state.LeftSide;
        dst[^1] = state.RightSide;
        var lengths = state.Lengths;
        dst = dst[1..^1];

        var pos = 0;
        
        for (var i = 0; i < lengths.Length; i++)
        {
            int len = lengths[i];
            
            dst.Slice(pos, len + 2).Fill(state.MiddleFill);

            pos += len + 2;

            if (i < lengths.Length - 1) // on last element
            {
                dst[pos] = state.MiddleSeparator;
                pos++;
            }
        }
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
            GenerateJoinSpan);
    }
    #endregion

    #region Wrap Join Span Method
    internal static void WrapJoinSpan(Span<char> dst, (string[] Array, int[] Lengths, char Separator, Alignment Alignment) state)
    {
        var array = state.Array.AsSpan();
        var lengths = state.Lengths.AsSpan();
        var separator = state.Separator;
        var alignment = state.Alignment;

        var lastLocation = 0;
        for (int i = 0; i < array.Length; i++)
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
    #endregion
    
    #region Wrap Join Methods
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
            WrapJoinSpan);
    }
    #endregion
    
    #region Reflection To Array

    public static string[] GetPropertiesStringValues(object obj, PropertyInfo[] properties)
    {
        var retArray = new string[properties.Length];
        for (var i = 0; i < properties.Length; i++)
            retArray[i] = (properties[i].GetValue(obj) ?? "Error").ToString() ?? "Error";
        return retArray;
    }
    
    #endregion
}