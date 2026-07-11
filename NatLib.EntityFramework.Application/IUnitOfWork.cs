namespace NatLib.EntityFramework.Application;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default);

    public Task BeginTransactionAsync(CancellationToken ct = default);

    public Task CommitTransactionAsync(CancellationToken ct = default);

    public Task RollbackTransactionAsync(CancellationToken ct = default);
}