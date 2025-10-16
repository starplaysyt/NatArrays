using NatArrays;
using Xunit;

namespace NatArrays.Tests;

[Trait("Category", "PointerArray")]
public class PointerArrayFuncTests
{
    [Fact]
    public void Test_IsAllocatedProp()
    {
        var parray = new PointerArray<int>();
        
        Assert.False(parray.IsAllocated);
        
        parray.Allocate(5);
        
        Assert.True(parray.IsAllocated);
        
        parray.Deallocate();
        
        Assert.False(parray.IsAllocated);
        
        parray.Dispose();
    }

    [Fact]
    public void Test_ByThisIndex()
    {
        var parray = new PointerArray<int>();
        
        parray.Allocate(5);
        
        Assert.Throws<IndexOutOfRangeException>(() => parray[-1]);
        
        Assert.Throws<IndexOutOfRangeException>(() => parray[5]);
        
        parray.Dispose();
    }

    [Fact]
    public void Test_Allocate()
    {
        var parray = new PointerArray<int>();
        
        Assert.Throws<ArgumentException>(() => parray.Allocate(-1));
        Assert.Throws<ArgumentException>(() => parray.Allocate(0));

        parray.Allocate(5);
        
        Assert.Throws<InvalidOperationException>(() => parray.Allocate(5));
        
        parray.Deallocate();
    }
}