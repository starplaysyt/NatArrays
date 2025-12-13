using System.Diagnostics;
using NatLib.Core;
using Xunit.Abstractions;

namespace NatLib.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private const int Iterations = 10_000_000;
    
    [Fact]
    public void Test1()
    {  
        var u1 = new Unit2<float>(1.0f, 2.0f);
        var u2 = new Unit2<float>(3.0f, 4.0f);

        var p1 = new Point2<float>(1.0f, 2.0f);
        var p2 = new Point2<float>(3.0f, 4.0f);

        // JIT warmup
        for (int i = 0; i < 100_000; i++)
        {
            _ = u1 + u2;
            _ = p1 + p2;
        }

        // -------------------------
        // Unit2 benchmark
        // -------------------------
        var swUnit = Stopwatch.StartNew();

        Unit2<float> unitResult = default;
        for (int i = 0; i < Iterations; i++)
        {
            unitResult = u1 + u2;
        }

        swUnit.Stop();

        // -------------------------
        // Point2 benchmark
        // -------------------------
        var swPoint = Stopwatch.StartNew();

        Point2<float> pointResult = default;
        for (int i = 0; i < Iterations; i++)
        {
            pointResult = p1 + p2;
        }

        swPoint.Stop();

        // Prevent dead-code elimination
        Assert.NotEqual(float.NaN, unitResult.X);
        Assert.NotEqual(float.NaN, pointResult.X);

        // Output results
        var unitTime = swUnit.Elapsed.TotalMilliseconds;
        var pointTime = swPoint.Elapsed.TotalMilliseconds;

        _testOutputHelper.WriteLine($"Unit2:  {unitTime:F2} ms");
        _testOutputHelper.WriteLine($"Point2: {pointTime:F2} ms");
    }
}