using System.Numerics;

namespace NatLib.Arrays.Comparers;

public static class PointerArrayEqualityComparer
{
    public static bool Compare<T>(PointerArray<T> arr1, T[] arr2) where T : unmanaged, IEquatable<T>
    {
        if (!arr1.IsAllocated) return false;
        return arr1.AsSpan().SequenceEqual(arr2.AsSpan());
    }

    public static bool Compare<T>(PointerArray<T> arr1, PointerArray<T> arr2) where T : unmanaged, IEquatable<T>
    {
        if (!arr1.IsAllocated || !arr2.IsAllocated) return false;
        return arr1.AsSpan().SequenceEqual(arr2.AsSpan());
    }
}