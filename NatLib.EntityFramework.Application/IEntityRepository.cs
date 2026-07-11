using System.Linq.Expressions;
using NatLib.EntityFramework.Domain;

namespace NatLib.EntityFramework.Application;

public interface IEntityRepository<TEntity, in TKey, in TUpdateMap> : IUnitOfWork
    where TEntity : DomainEntity<TKey>
    where TKey : unmanaged, IComparable<TKey>
{
    public Task<bool> AnyAsync(
        CancellationToken ct = default);
    
    public Task<bool> AnyAsync(TKey id,
        CancellationToken ct = default);
    
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> id,
        CancellationToken ct = default);

    public Task<int> CountAsync(
        CancellationToken ct = default);

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default);

    public Task<TEntity?> GetAsync(
        TKey id,
        CancellationToken ct = default);

    public Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default);

    public Task<TMap?> GetAsync<TMap>(
        TKey id,
        Expression<Func<TEntity, TMap>> selector,
        CancellationToken ct = default);
    
    public Task<TMap?> GetAsync<TMap>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TMap>> selector,
        CancellationToken ct = default);

    public Task<List<TEntity>> GetAllAsync(
        CancellationToken ct = default);
    
    public Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default);
    
    public Task<List<TMap>> GetAllAsync<TMap>(
        Expression<Func<TEntity, TMap>> selector,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default);

    public Task<List<TMap>> GetPagedAsync<TMap>(
        TKey lastId,
        int pageSize,
        Expression<Func<TEntity, TMap>> selector,
        CancellationToken ct = default);


    public Task<List<TMap>> GetPagedAsync<TMap>(
        TKey lastID,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TMap>> selector,
        CancellationToken ct = default);

    public Task CreateAsync(
        TEntity building,
        CancellationToken ct = default);

    public Task<bool> UpdateAsync(
        TKey id,
        TUpdateMap updateMap,
        CancellationToken ct = default);

    public Task<int> UpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        TUpdateMap updateMap,
        CancellationToken ct = default);

    public Task<bool> DeleteAsync(
        TKey id,
        CancellationToken ct = default);

    public Task<int> DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default);
}