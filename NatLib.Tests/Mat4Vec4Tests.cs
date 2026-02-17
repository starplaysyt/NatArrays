using System;
using NatLib.Arrays;
using NatLib.Tests;
using Xunit;

public class Mat4Vec4Tests
{
    [Fact]
    public void Mat4Equals()
    {
        var mat1 = new Mat4F();
        mat1[0, 0] = 1;
        mat1[1, 1] = 2;
        mat1[2, 2] = 3;
        mat1[3, 3] = 4;
        
        var mat2 = new Mat4F();
        mat2[0, 0] = 1;
        mat2[1, 1] = 2;
        mat2[2, 2] = 3;
        mat2[3, 3] = 4;
        
        var mat3 = new Mat4F();
        mat3[0, 0] = 1;
        mat3[1, 1] = 2;
        mat3[2, 2] = 3;
        
        var mat4 = new Mat4F();

        var fillArr = new float[16];
        Array.Fill(fillArr, 0);
        var mat5 = new Mat4F(fillArr);
        
        Assert.True(mat1.Equals(mat2));
        Assert.True(mat2.Equals(mat1));
        
        Assert.False(mat1.Equals(mat3));
        Assert.False(mat3.Equals(mat1));
        
        Assert.True(mat5.Equals(mat4));
        Assert.True(mat4.Equals(mat5));
    }

    [Fact]
    public void Mat4GetRowAccessor()
    {
        var mat1 = new Mat4F();
        mat1[0, 0] = 1;
        mat1[0, 1] = 1;
        mat1[0, 2] = 1;
        mat1[0, 3] = 1;
        
        var row1 = new Vec4F(1, 1, 1, 1);

        Assert.True(mat1.GetRow(0).Equals(row1));
    }
}