namespace NatLib.Arrays;

/// <summary>
/// Specifies how newly allocated or resized memory should be initialized:
/// <list type="bullet">
/// <item><description><see cref="Nothing"/> — leaves memory uninitialized.</description></item>
/// <item><description><see cref="Zeroes"/> — fills memory with zeroes using <see cref="System.Runtime.InteropServices.NativeMemory.Clear"/>.</description></item>
/// <item><description><see cref="Constructor"/> — calls the parameterless constructor (<c>new T()</c>) for each element.</description></item>
/// </list>
/// </summary>
/// <remarks>
/// This enumeration controls what happens to the contents of memory after it has been allocated or resized.
/// </remarks>
public enum InitializationMode : byte
{
    /// <summary>
    /// Leaves newly allocated memory uninitialized.
    /// </summary>
    Nothing = 0,

    /// <summary>
    /// Initializes newly allocated memory to zeroes using <see cref="System.Runtime.InteropServices.NativeMemory.Clear"/>.
    /// </summary>
    Zeroes = 1,

    /// <summary>
    /// Invokes the parameterless constructor (<c>new T()</c>) for each newly allocated element.
    /// </summary>
    Constructor = 2
}