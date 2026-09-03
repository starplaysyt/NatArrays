using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NatLib.EntityFramework.Application;
using NatLib.EntityFramework.Domain;

namespace NatLib.EntityFramework.Infrastructure;

public abstract class EntityRepository<TEntity, TKey, TUpdateMap>
    : UnitOfWork, IEntityRepository<TEntity, TKey, TUpdateMap>
    where TEntity : DomainEntity<TKey>
    where TKey : unmanaged, IComparable<TKey>
{
    public abstract DbSet<TEntity> DbSet { get; }

    public virtual Action<UpdateSettersBuilder<TEntity>> BuildUpdateSetter(
        TUpdateMap updateMap) => UpdateSetterGenerator<TEntity, TKey, TUpdateMap>.Compiled(updateMap);

    public async Task<bool> AnyAsync(CancellationToken ct = default)
        => await DbSet.AsNoTracking().AnyAsync(ct);
    
    public async Task<bool> AnyAsync(TKey id, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().AnyAsync(b => b.Id.Equals(id), ct);

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking().AnyAsync(predicate, ct);

    public async Task<int> CountAsync(
        CancellationToken ct = default)
        => await DbSet.AsNoTracking().CountAsync(ct);

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking().CountAsync(predicate, ct);

    public async Task<TEntity?> GetAsync(
        TKey id,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(e => e.Id.Equals(id))
            .FirstOrDefaultAsync(ct);

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(predicate)
            .FirstOrDefaultAsync(ct);

    public async Task<TMap?> GetAsync<TMap>(
        TKey id,
        Expression<Func<TEntity, TMap>> selector,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(b => b.Id.Equals(id))
            .Select(selector)
            .FirstOrDefaultAsync(ct);
    
    public async Task<TMap?> GetAsync<TMap>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TMap>> selector,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(predicate)
            .Select(selector)
            .FirstOrDefaultAsync(ct);
    
    public async Task<List<TEntity>> GetAllAsync(
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .ToListAsync(ct);

    public async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(predicate)
            .ToListAsync(ct);

    public async Task<List<TMap>> GetAllAsync<TMap>(
        Expression<Func<TEntity, TMap>> selector,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(predicate)
            .Select(selector)
            .ToListAsync(ct);
    
    public async Task<List<TMap>> GetPagedAsync<TMap>(
        TKey lastId,
        int pageSize,
        Expression<Func<TEntity, TMap>> selector,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(e => e.Id.CompareTo(lastId) > 0)
            .OrderBy(e => e.Id)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(ct);

    public async Task<List<TMap>> GetPagedAsync<TMap>(
        TKey lastID,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TMap>> selector,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(predicate)
            .Where(e => e.Id.CompareTo(lastID) > 0)
            .OrderBy(e => e.Id)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(ct);

    public async Task CreateAsync(
        TEntity building,
        CancellationToken ct = default)
        => await DbSet.AddAsync(building, ct);

    public async Task<bool> UpdateAsync(
        TKey id,
        TUpdateMap updateMap,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(e => e.Id.Equals(id))
            .ExecuteUpdateAsync(BuildUpdateSetter(updateMap), ct) > 0;

    public async Task<int> UpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        TUpdateMap updateMap,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(predicate)
            .ExecuteUpdateAsync(BuildUpdateSetter(updateMap), ct);

    public async Task<bool> DeleteAsync(
        TKey id,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(b => b.Id.Equals(id))
            .ExecuteDeleteAsync(ct) > 0;
    
    public async Task<int> DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(predicate)
            .ExecuteDeleteAsync(ct);
}