namespace NatLib.Arrays;

/// <summary>
/// Defines the behavior of memory allocation.
/// </summary>
public enum AllocatingType : byte
{
    /// <summary>
    /// Leave newly allocated memory as it is.
    /// </summary>
    Nothing = 0,

    /// <summary>
    /// Filling newly allocated memory to zeroes using <c>NativeMemory.Clear()</c>.
    /// </summary>
    Zeroes = 1,

    /// <summary>
    /// Calling a constructor on each newly allocated cell (<c>new T()</c>).
    /// </summary>
    Constructor = 2
}