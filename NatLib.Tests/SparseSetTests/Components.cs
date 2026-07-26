using System.Runtime.InteropServices;

namespace NatLib.Tests.SparseSetTests;

[StructLayout(LayoutKind.Sequential)]
public struct Position
{
    public float X, Y, Z;

    public Position(float x, float y, float z)
    {
        X = x; Y = y; Z = z;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct BigComponent
{
    public long A, B, C, D, E, F, G, H; // 64 байта — ровно 1 cache line
}