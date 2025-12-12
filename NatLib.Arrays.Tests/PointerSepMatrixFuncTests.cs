namespace NatArrays.Tests;

public class PointerSepMatrixFuncTests
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
}