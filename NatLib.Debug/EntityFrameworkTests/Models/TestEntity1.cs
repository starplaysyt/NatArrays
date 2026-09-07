using Microsoft.EntityFrameworkCore;
using NatLib.EntityFramework.Application;
using NatLib.EntityFramework.Domain;
using NatLib.EntityFramework.Infrastructure;

namespace NatLib.Debug.EntityFrameworkTests.Models;

public class TestEntity1 : DomainEntity<uint>
{
    public required string Name { get; set; }
    public required int Value { get; set; }
    
    public required uint? TestId { get; set; }
}

public record TestEntity1UpdateMap(
    string? Name,
    int? Value,
    NullableField<uint>? TestId);

public interface ITestEntity1Repository :
    IEntityRepository<TestEntity1, uint, TestEntity1UpdateMap>
{
}

public class TestEntity1Repository(AppDbContext context) :
    EntityRepository<TestEntity1, uint, TestEntity1UpdateMap>,
    ITestEntity1Repository
{
    protected override DbContext Context => context;
    public override DbSet<TestEntity1> DbSet => context.TestEntities;
} 