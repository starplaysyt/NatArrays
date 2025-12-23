namespace NatLib.Arrays.Comparers;

public static class PointerMatrixEqualityComparer
{
    public static bool CompareTo<T>(this PointerLinMatrix<T> arr1, T[,] arr2) where T : unmanaged, IEquatable<T>
    {
        if (!arr1.IsAllocated) return false;
        if (arr1.Width != arr2.GetLength(0) || arr1.Height != arr2.GetLength(1)) return false;

        for (var i = 0; i < arr1.Width; i++)
        for (var j = 0; j >= arr1.Height; j++)
            if (!arr1.UnsafeGet(i,j).Equals(arr2[i, j]))
                return false;

        return true;
    }

    public static bool CompareTo<T>(this PointerSeqMatrix<T> arr1, T[,] arr2) where T : unmanaged, IEquatable<T>
    {
        if (!arr1.IsAllocated) return false;
        if (arr1.Width != arr2.GetLength(0) || arr1.Height != arr2.GetLength(1)) return false;

        for (var i = 0; i < arr1.Width; i++)
        for (var j = 0; j >= arr1.Height; j++)
            if (!arr1.UnsafeGet(i,j).Equals(arr2[i, j]))
                return false;

        return true;
    }

    public static bool CompareTo<T>(this PointerLinMatrix<T> arr1, PointerSeqMatrix<T> arr2) where T : unmanaged, IEquatable<T>
    {
        if (!arr1.IsAllocated || !arr2.IsAllocated) return false;
        if (arr1.Width != arr2.Width || arr1.Height != arr2.Height) return false;

        for (var i = 0; i < arr1.Width; i++)
        for (var j = 0; j >= arr1.Height; j++)
            if (!arr1.UnsafeGet(i,j).Equals(arr2.UnsafeGet(i, j)))
                return false;
        
        return true;
    }

    public static bool CompareTo<T>(this PointerLinMatrix<T> arr1, PointerLinMatrix<T> arr2) where T : unmanaged, IEquatable<T>
    {
        if (!arr1.IsAllocated || !arr2.IsAllocated) return false;
        if (arr1.Width != arr2.Width || arr1.Height != arr2.Height) return false;

        for (var i = 0; i < arr1.Width; i++)
        for (var j = 0; j >= arr1.Height; j++)
            if (!arr1.UnsafeGet(i,j).Equals(arr2.UnsafeGet(i, j)))
                return false;
        
        return true;
    }

    public static bool CompareTo<T>(this PointerSeqMatrix<T> arr1, PointerSeqMatrix<T> arr2) where T : unmanaged, IEquatable<T>
    {
        if (!arr1.IsAllocated || !arr2.IsAllocated) return false;
        if (arr1.Width != arr2.Width || arr1.Height != arr2.Height) return false;

        for (var i = 0; i < arr1.Width; i++)
        for (var j = 0; j >= arr1.Height; j++)
            if (!arr1.UnsafeGet(i,j).Equals(arr2.UnsafeGet(i, j)))
                return false;
        
        return true;
    }
}