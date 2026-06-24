using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;

namespace RealEstate.Infrastructure.Repositories
{
    public class LandRepository : GenericRepository<LandProperty>, ILandRepository
    {
        public LandRepository(AppDbContext context) : base(context) { }

        public async Task<LandProperty?> GetByIdAsync(Guid id, Guid companyId) =>
            await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId);

        public async Task<LandProperty?> GetByIdWithDetailsAsync(Guid id, Guid companyId) =>
            await _dbSet
                .AsNoTracking()
                .Include(x => x.Owner)
                .Include(x => x.Agent)
                .Include(x => x.ParentProperty)
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId);

        public async Task<PagedResult<LandProperty>> GetPagedAsync(Guid companyId, int page, int pageSize)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LandProperty>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> ExistsAsync(Guid id, Guid companyId) =>
            await _dbSet.AnyAsync(x => x.Id == id && x.CompanyId == companyId);

        public async Task<bool> HasChildrenAsync(Guid landId, Guid companyId) =>
            await _context.Properties
                .AnyAsync(x => x.ParentPropertyId == landId && x.CompanyId == companyId);
    }
}