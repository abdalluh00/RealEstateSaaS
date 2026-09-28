using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
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

            // ── Single ────────────────────────────────────────
            public async Task<T?> GetByIdAsync(
                Guid id,
                CancellationToken ct = default) =>
                await _dbSet.FindAsync([id], ct);

            public async Task<T?> FirstOrDefaultAsync(
                Expression<Func<T, bool>> predicate,
                CancellationToken ct = default) =>
                await _dbSet
                    .AsNoTracking()
                    .FirstOrDefaultAsync(predicate, ct);

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

            // ── Composable Queryable ──────────────────────────
            public IQueryable<T> Query() =>
                _dbSet.AsQueryable();
            // tracked — for commands that may modify

            public IQueryable<T> QueryNoTracking() =>
                _dbSet.AsNoTracking().AsQueryable();
            // untracked — for read-only queries/projections

            // ── Write ─────────────────────────────────────────
            public void Add(T entity)
            {
                entity.CreatedAt = DateTime.UtcNow;
                _dbSet.Add(entity);
            }

            public void AddRange(IEnumerable<T> entities)
            {
                foreach (var entity in entities)
                    entity.CreatedAt = DateTime.UtcNow;

                _dbSet.AddRange(entities);
            }

            public void Update(T entity)
            {
                entity.UpdatedAt = DateTime.UtcNow;
                _dbSet.Update(entity);
            }

            public void SoftDelete(T entity)
            {
                entity.IsDeleted = true;
                entity.UpdatedAt = DateTime.UtcNow;
                _dbSet.Update(entity);
            }

            public void HardDelete(T entity) =>
                _dbSet.Remove(entity);
        }
    }
