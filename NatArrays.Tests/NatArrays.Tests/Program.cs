using Xunit;

namespace NatArrays.Tests;

public class Tests
{
    [Fact]
    public static void Allocate_ShouldAllocateMemory()
    {
        using var array = new PointerArray<int>();
        array.Allocate(10);

        Assert.True(array.IsAllocated);
        Assert.Equal(10, array.Length);
        Assert.Equal((ulong)(10 * sizeof(int)), array.ByteLength);
    }
    
    [Fact]
    public static void Indexer_ShouldReadAndWriteValues()
    {
        using var array = new PointerArray<int>();
        array.Allocate(3);

        array[0] = 42;
        array[1] = 7;
        array[2] = -5;

        Assert.Equal(42, array[0]);
        Assert.Equal(7, array[1]);
        Assert.Equal(-5, array[2]);
    }
    
    [Fact]
    public static void Indexer_ShouldThrowOnOutOfRange()
    {
        using var array = new PointerArray<int>();
        array.Allocate(2);

        Assert.Throws<IndexOutOfRangeException>(() => { var x = array[2]; });
        Assert.Throws<IndexOutOfRangeException>(() => array[-1] = 123);
    }
    
    [Fact]
    public static void Resize_ShouldPreserveDataAndChangeLength()
    {
        using var array = new PointerArray<int>();
        array.Allocate(3);
        array[0] = 1;
        array[1] = 2;
        array[2] = 3;

        array.Resize(5);
        Assert.Equal(5, array.Length);
        Assert.Equal(1, array[0]);
        Assert.Equal(2, array[1]);
        Assert.Equal(3, array[2]);
    }
    
    [Fact]
    public static void Deallocate_ShouldReleaseMemory()
    {
        var array = new PointerArray<int>();
        array.Allocate(10);
        Assert.True(array.IsAllocated);

        array.Deallocate();

        Assert.False(array.IsAllocated);
        Assert.Throws<ObjectDisposedException>(() => array[0] = 1);
    }
    
    [Fact]
    public static void DoubleDispose_ShouldNotCrash()
    {
        var array = new PointerArray<int>();
        array.Allocate(10);

        array.Dispose();
        array.Dispose(); // должно быть безопасно
    }
    
    [Fact]
    public void Finalizer_ShouldReleaseMemory()
    {
        WeakReference reference;
        void Create()
        {
            var array = new PointerArray<int>();
            array.Allocate(1000);
            reference = new WeakReference(array);
        }

        Create();
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Assert.False(reference.IsAlive);
    }
}

public class Program
{
    public static void Main()
    {
        Tests.Allocate_ShouldAllocateMemory();
        Tests.Indexer_ShouldReadAndWriteValues();
        Tests.Indexer_ShouldThrowOnOutOfRange();
        Tests.Resize_ShouldPreserveDataAndChangeLength();
        Tests.Deallocate_ShouldReleaseMemory();
    }
}
