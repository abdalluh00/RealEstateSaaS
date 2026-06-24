using RealEstate.Domain.Common;
using System.Linq.Expressions;

namespace RealEstate.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // ── Single ────────────────────────────────────────
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);

        // ── Composable Queryable ──────────────────────────
        IQueryable<T> Query();            // tracked   — for commands that may modify
        IQueryable<T> QueryNoTracking();  // untracked — for read-only queries/projections

        // ── Write ─────────────────────────────────────────
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void SoftDelete(T entity);
        void HardDelete(T entity);        // use only for cascade cleanup
    }
}