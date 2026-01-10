using System.Reflection;
using System.Runtime.CompilerServices;
using NatLib.Core.Enums;

namespace NatLib.Core.Operations;

public static class StringExtensions
{
    #region Fix Span Methods
    private static void FixSpan(Span<char> dst, (string Source, char Character) state)
    {
        var span = state.Source.AsSpan();
        var copy = Math.Min(span.Length, dst.Length); 
            
        span[..copy].CopyTo(dst); // Copying existed part.

        if (copy < dst.Length) // Filling the rest. Do nothing when.
            dst[copy..].Fill(state.Character);  
    }

    private static void FixSpan(Span<char> dst, (string Source, char Character) state, Alignment alignment)
    {
        switch (alignment)
        {
            case Alignment.Begin:
                FixSpan(dst, state);
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

    #region Fix String Methods
    public static string FixRight(this string str, int length, char character = ' ')
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (str.Length == length) return str;
        if (length == 0) return string.Empty;
        
        return string.Create(length, (Source: str, Character: character), FixSpanRight);
    }
    
    public static string Fix(this string str, int length, char character = ' ')
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (str.Length == length) return str;
        if (length == 0) return string.Empty;

        return string.Create(length, (Source: str, Character: character), FixSpan);
    }

    public static string FixCenter(this string str, int length, char character = ' ')
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (str.Length == length) return str;
        if (length == 0) return string.Empty;
        
        return string.Create(length, (Source: str, Character: character), FixSpanCenter);
    }

    public static string Fix(this string str, int length, char character, Alignment alignment)
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (str.Length == length) return str;
        if (length == 0) return string.Empty;
        
        return string.Create(
            length, 
            (Source: str, Character: character), 
            alignment switch {
            Alignment.Begin => FixSpan,
            Alignment.Center => FixSpanCenter,
            Alignment.End => FixSpanRight,
            _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
        });
    }
    #endregion
    
    #region Generate Method 
    public static string Generate(char character, int length)
    {
        return length switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(length)),
            0 => string.Empty,
            _ => string.Create(length, character, static (dst, ch) => dst.Fill(ch))
        };
    }
    #endregion

    #region Wrap Join Span Method
    private static void WrapJoinSpan(Span<char> dst, (string[] Array, int[] Lengths, char Separator, Alignment alignment) state)
    {
        var array = state.Array.AsSpan();
        var lengths = state.Lengths.AsSpan();
        var separator = state.Separator;
        var alignment = state.alignment;

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
    public static string WrapJoin(string[] arr, int[] lengths, char separator, Alignment alignment = Alignment.Begin)
    {
        ArgumentNullException.ThrowIfNull(arr);
        ArgumentNullException.ThrowIfNull(lengths);
        
        if (arr.Length != lengths.Length)
            throw new InvalidOperationException("Lengths of string array and int array should be equal.");
        
        return string.Create(
            lengths.Sum() + (lengths.Length - 1) * 3 + 4, 
            (arr, lengths, separator, alignment), 
            WrapJoinSpan);
    }
    #endregion
    
    #region Reflection To Array

    public static string[] GetProperties(object obj, PropertyInfo[] properties)
    {
        var retArray = new string[properties.Length];
        for (var i = 0; i < properties.Length; i++)
            retArray[i] = (properties[i].GetValue(obj) ?? "Error").ToString() ?? "Error";
        return retArray;
    }
    
    #endregion
}