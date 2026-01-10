using System;
using NatLib.Arrays;
using NatLib.Tests;
using Xunit;

public class PointerArrayTests
{
    [Fact]
    public void Allocate_SetsLengthAndAllocationFlag()
    {
        var arr = new PointerArray<int>();

        arr.Allocate(10);

        Assert.True(arr.IsAllocated);
        Assert.Equal(10, arr.Length);
        Assert.Equal((ulong)(10 * sizeof(int)), arr.ByteLength);

        arr.Deallocate();
    }

    [Fact]
    public void Allocate_Throws_WhenAlreadyAllocated()
    {
        var arr = new PointerArray<int>();
        arr.Allocate(5);

        Assert.Throws<InvalidOperationException>(() => arr.Allocate(3));

        arr.Deallocate();
    }

    [Fact]
    public void Allocate_Throws_WhenLengthIsNonPositive()
    {
        var arr = new PointerArray<int>();

        Assert.Throws<ArgumentException>(() => arr.Allocate(0));
        Assert.Throws<ArgumentException>(() => arr.Allocate(-1));
    }

    [Fact]
    public void Indexer_ReturnsCorrectValue()
    {
        var arr = new PointerArray<int>();
        arr.Allocate(3, InitializationMode.Zeroes);

        unsafe
        {
            arr.UnsafeSet(0, 10);
            arr.UnsafeSet(1, 20);
            arr.UnsafeSet(2, 30);
        }

        Assert.Equal(10, arr[0]);
        Assert.Equal(20, arr[1]);
        Assert.Equal(30, arr[2]);

        arr.Deallocate();
    }

    [Fact]
    public void Indexer_Throws_WhenNotAllocated()
    {
        var arr = new PointerArray<int>();

        Assert.Throws<InvalidOperationException>(() => _ = arr[0]);
    }

    [Fact]
    public void Indexer_Throws_WhenOutOfRange()
    {
        var arr = new PointerArray<int>();
        arr.Allocate(3);

        Assert.Throws<IndexOutOfRangeException>(() => _ = arr[-1]);
        Assert.Throws<IndexOutOfRangeException>(() => _ = arr[3]);

        arr.Deallocate();
    }

    [Fact]
    public void AsSpan_ReturnsCorrectSpan()
    {
        var arr = new PointerArray<int>();
        arr.Allocate(3, InitializationMode.Zeroes);

        var span = arr.AsSpan();
        span[0] = 1;
        span[1] = 2;
        span[2] = 3;

        Assert.Equal(1, arr[0]);
        Assert.Equal(2, arr[1]);
        Assert.Equal(3, arr[2]);

        arr.Deallocate();
    }

    [Fact]
    public void AsSpan_Throws_WhenNotAllocated()
    {
        var arr = new PointerArray<int>();

        Assert.Throws<InvalidOperationException>(() => arr.AsSpan());
    }

    [Fact]
    public void FromManaged_CopiesDataCorrectly()
    {
        var managed = new[] { 5, 6, 7 };
        var arr = new PointerArray<int>();

        arr.FromManaged(managed);

        Assert.True(arr.IsAllocated);
        Assert.Equal(3, arr.Length);
        Assert.Equal(5, arr[0]);
        Assert.Equal(6, arr[1]);
        Assert.Equal(7, arr[2]);

        arr.Deallocate();
    }

    [Fact]
    public void FromManaged_Throws_WhenAlreadyAllocated()
    {
        var arr = new PointerArray<int>();
        arr.Allocate(2);

        Assert.Throws<InvalidOperationException>(() => arr.FromManaged(new[] { 1, 2 }));

        arr.Deallocate();
    }

    [Fact]
    public void ToManaged_CreatesIndependentArray()
    {
        var arr = new PointerArray<int>();
        arr.Allocate(2);
        unsafe
        {
            arr.UnsafeSet(0, 10);
            arr.UnsafeSet(1, 20);
        }

        var managed = arr.ToManaged();

        Assert.Equal(new[] { 10, 20 }, managed);

        // проверяем независимость
        unsafe
        {
            arr.UnsafeSet(0, 99);
        }

        Assert.Equal(10, managed[0]);

        arr.Deallocate();
    }

    [Fact]
    public void ToManaged_Throws_WhenNotAllocated()
    {
        var arr = new PointerArray<int>();

        Assert.Throws<InvalidOperationException>(() => arr.ToManaged());
    }

    [Fact]
    public void Resize_IncreasesArrayAndInitializesNewPartWithZeroes()
    {
        var arr = new PointerArray<int>();
        arr.Allocate(2, InitializationMode.Zeroes);

        unsafe
        {
            arr.UnsafeSet(0, 1);
            arr.UnsafeSet(1, 2);
        }

        arr.Resize(4, InitializationMode.Zeroes);

        Assert.Equal(4, arr.Length);
        Assert.Equal(1, arr[0]);
        Assert.Equal(2, arr[1]);
        Assert.Equal(0, arr[2]);
        Assert.Equal(0, arr[3]);

        arr.Deallocate();
    }

    [Fact]
    public void Resize_Throws_WhenNotAllocated()
    {
        var arr = new PointerArray<int>();

        Assert.Throws<InvalidOperationException>(() => arr.Resize(10));
    }

    [Fact]
    public void Deallocate_ClearsPointerAndLength()
    {
        var arr = new PointerArray<int>();
        arr.Allocate(5);

        arr.Deallocate();

        Assert.False(arr.IsAllocated);
        Assert.Equal(0, arr.Length);
    }
}
