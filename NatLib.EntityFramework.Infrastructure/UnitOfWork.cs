using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NatLib.EntityFramework.Application;

namespace NatLib.EntityFramework.Infrastructure;

public abstract class UnitOfWork : IUnitOfWork
{
    protected abstract DbContext Context { get; }
    
    private IDbContextTransaction? _transaction;
    
    public Task<int> SaveChangesAsync(CancellationToken ct = default) 
        => Context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        _transaction = await Context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}