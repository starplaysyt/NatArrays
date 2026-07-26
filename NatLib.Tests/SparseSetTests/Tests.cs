using NatLib.Arrays;

namespace NatLib.Tests.SparseSetTests;

public class SparseSetCorrectnessTests : IDisposable
{
    private readonly SparseSet<Position> _set = new();

    public void Dispose()
    {
        _set.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void Add_SingleElement_CanRetrieve()
    {
        _set.Set(0, new Position(1, 2, 3));

        Assert.True(_set.Has(0));
        Assert.Equal(1, _set.Count);

        ref var pos = ref _set[0];
        Assert.Equal(1f, pos.X);
        Assert.Equal(2f, pos.Y);
        Assert.Equal(3f, pos.Z);
    }

    [Fact]
    public void Add_MultipleElements_AllAccessible()
    {
        for (var i = 0; i < 100; i++)
            _set.Set(i, new Position(i, i * 10, i * 100));

        Assert.Equal(100, _set.Count);

        for (var i = 0; i < 100; i++)
        {
            Assert.True(_set.Has(i));
            ref var p = ref _set[i];
            Assert.Equal(i, p.X);
        }
    }

    [Fact]
    public void Has_NonExistent_ReturnsFalse()
    {
        _set.Set(5, new Position(1, 1, 1));

        Assert.False(_set.Has(0));
        Assert.False(_set.Has(999));
        Assert.False(_set.Has(int.MaxValue / 2));
    }

    [Fact]
    public void Get_ReturnsRef_ModifiesInPlace()
    {
        _set.Set(42, new Position(0, 0, 0));

        ref var pos = ref _set[42];
        pos.X = 999f;

        Assert.Equal(999f, _set[42].X);
    }

    [Fact]
    public void Remove_SingleElement_BecomesEmpty()
    {
        _set.Set(7, new Position(1, 2, 3));
        var removed = _set.Remove(7);

        Assert.True(removed);
        Assert.Equal(0, _set.Count);
        Assert.False(_set.Has(7));
    }

    [Fact]
    public void Remove_NonExistent_ReturnsFalse()
    {
        _set.Set(1);
        Assert.False(_set.Remove(999));
        Assert.Equal(1, _set.Count);
    }

    [Fact]
    public void Remove_Middle_SwapAndPop_PreservesOthers()
    {
        _set.Set(10, new Position(10, 0, 0));
        _set.Set(20, new Position(20, 0, 0));
        _set.Set(30, new Position(30, 0, 0));
        _set.Set(40, new Position(40, 0, 0));
        _set.Set(50, new Position(50, 0, 0));

        _set.Remove(30);

        Assert.Equal(4, _set.Count);
        Assert.False(_set.Has(30));

        Assert.True(_set.Has(10));
        Assert.True(_set.Has(20));
        Assert.True(_set.Has(40));
        Assert.True(_set.Has(50));

        Assert.Equal(10f, _set[10].X);
        Assert.Equal(20f, _set[20].X);
        Assert.Equal(40f, _set[40].X);
        Assert.Equal(50f, _set[50].X);
    }

    [Fact]
    public void Remove_Last_NoSwapNeeded()
    {
        _set.Set(1, new Position(1, 0, 0));
        _set.Set(2, new Position(2, 0, 0));
        _set.Set(3, new Position(3, 0, 0));

        _set.Remove(3);

        Assert.Equal(2, _set.Count);
        Assert.True(_set.Has(1));
        Assert.True(_set.Has(2));
        Assert.False(_set.Has(3));
    }

    [Fact]
    public void Remove_All_ThenAddAgain()
    {
        _set.Set(1, new Position(1, 0, 0));
        _set.Set(2, new Position(2, 0, 0));

        _set.Remove(1);
        _set.Remove(2);

        Assert.Equal(0, _set.Count);

        // Переиспользование тех же ID
        _set.Set(1, new Position(100, 0, 0));
        Assert.Equal(1, _set.Count);
        Assert.Equal(100f, _set[1].X);
    }

    [Fact]
    public void GetOrAdd_New_AddsDefault()
    {
        ref var pos = ref _set.GetOrAdd(99);
        Assert.Equal(1, _set.Count);
        Assert.True(_set.Has(99));

        pos.X = 42f;
        Assert.Equal(42f, _set[99].X);
    }

    [Fact]
    public void GetOrAdd_Existing_ReturnsSameRef()
    {
        _set.Set(5, new Position(7, 8, 9));
        ref var pos = ref _set.GetOrAdd(5);

        Assert.Equal(1, _set.Count);
        Assert.Equal(7f, pos.X);
    }

    [Fact]
    public void GrowSparse_LargeEntityId_Works()
    {
        _set.Set(10_000, new Position(1, 2, 3));

        Assert.True(_set.Has(10_000));
        Assert.Equal(1f, _set[10_000].X);
    }

    [Fact]
    public void GrowDense_ManyElements_Works()
    {
        for (var i = 0; i < 1000; i++)
            _set.Set(i, new Position(i, 0, 0));

        Assert.Equal(1000, _set.Count);

        for (var i = 0; i < 1000; i++)
            Assert.Equal(i, _set[i].X);
    }

    [Fact]
    public void AsDataSpan_LinearIteration_MatchesCount()
    {
        for (var i = 0; i < 50; i++)
            _set.Set(i * 3, new Position(i, 0, 0));

        var data = _set.AsDataSpan();
        Assert.Equal(50, data.Length);

        float sum = 0;
        foreach (ref readonly var p in data)
            sum += p.X;

        Assert.Equal(1225f, sum);
    }

    [Fact]
    public void AsEntitySpan_ParallelToData()
    {
        _set.Set(100, new Position(100, 0, 0));
        _set.Set(200, new Position(200, 0, 0));
        _set.Set(300, new Position(300, 0, 0));

        var entities = _set.AsEntitySpan();
        var data = _set.AsDataSpan();

        Assert.Equal(3, entities.Length);
        Assert.Equal(3, data.Length);

        for (var i = 0; i < entities.Length; i++)
        {
            var eid = entities[i];
            Assert.Equal(eid, data[i].X);
        }
    }

    [Fact]
    public void Iteration_AfterRemove_NoDirtyData()
    {
        for (var i = 0; i < 10; i++)
            _set.Set(i, new Position(i, 0, 0));

        _set.Remove(3);
        _set.Remove(7);

        var data = _set.AsDataSpan();
        Assert.Equal(8, data.Length);

        foreach (ref readonly var p in data)
            Assert.InRange(p.X, 0f, 9f);
    }

    [Fact]
    public void StressTest_AddRemoveCycles()
    {
        var random = new Random(42);

        for (var cycle = 0; cycle < 100; cycle++)
        {
            for (var i = 0; i < 500; i++)
                _set.Set(cycle * 500 + i, new Position(i, cycle, 0));
            
            var toRemove = Enumerable.Range(cycle * 500, 500)
                .OrderBy(_ => random.Next())
                .Take(250)
                .ToList();

            foreach (var id in toRemove)
                _set.Remove(id);
        }

        var entities = _set.AsEntitySpan();
        var data = _set.AsDataSpan();

        Assert.Equal(_set.Count, entities.Length);
        Assert.Equal(_set.Count, data.Length);

        for (var i = 0; i < entities.Length; i++)
        {
            var eid = entities[i];
            Assert.True(_set.Has(eid));
            ref var fromGet = ref _set[eid];
            Assert.Equal(data[i].X, fromGet.X);
            Assert.Equal(data[i].Y, fromGet.Y);
        }
    }

    [Fact]
    public void Dispose_ThenDispose_NoCrash()
    {
        var set = new SparseSet<Position>();
        set.Set(1);
        set.Dispose();
        set.Dispose();
    }
}