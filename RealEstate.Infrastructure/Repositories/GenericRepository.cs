using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Common;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace RealEstate.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // ── Queryable ─────────────────────────────────────
        // Global query filter (IsDeleted) is applied automatically by EF
        public IQueryable<T> Query() =>
            _dbSet.AsQueryable();

        public IQueryable<T> QueryNoTracking() =>
            _dbSet.AsNoTracking();

        // ── Single ────────────────────────────────────────
        // IMPORTANT: Never use FindAsync — it hits the cache and bypasses
        // global query filters, so soft-deleted records can leak through
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _dbSet.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default) =>
            await _dbSet.FirstOrDefaultAsync(predicate, ct);

        public async Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default) =>
            await _dbSet.AnyAsync(predicate, ct);

        public async Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default) =>
            predicate is null
                ? await _dbSet.CountAsync(ct)
                : await _dbSet.CountAsync(predicate, ct);

        // ── Write ─────────────────────────────────────────
        public void Add(T entity) =>
            _dbSet.Add(entity);

        public void AddRange(IEnumerable<T> entities) =>
            _dbSet.AddRange(entities);

        public void Update(T entity) =>
            _dbSet.Update(entity);

        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            // UpdatedAt is stamped automatically in SaveChangesAsync
            _dbSet.Update(entity);
        }

        public void HardDelete(T entity) =>
            _dbSet.Remove(entity);
    }
}