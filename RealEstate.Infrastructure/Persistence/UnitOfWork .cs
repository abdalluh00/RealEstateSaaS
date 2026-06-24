using Microsoft.EntityFrameworkCore.Storage;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context) => _context = context;

        public async Task BeginTransactionAsync(CancellationToken ct = default) =>
            _transaction = await _context.Database.BeginTransactionAsync(ct);

        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
            await _transaction!.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            await _transaction!.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            await _context.SaveChangesAsync(ct);

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}