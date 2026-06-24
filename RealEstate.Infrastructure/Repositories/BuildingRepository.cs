using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;

namespace RealEstate.Infrastructure.Repositories
{
    public class BuildingRepository : GenericRepository<BuildingProperty>, IBuildingRepository
    {
        public BuildingRepository(AppDbContext context) : base(context) { }

        public async Task<BuildingProperty?> GetByIdAsync(Guid id, Guid companyId) =>
            await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId);

        public async Task<BuildingProperty?> GetByIdWithDetailsAsync(Guid id, Guid companyId) =>
            await _dbSet
                .AsNoTracking()
                .Include(x => x.Owner)
                .Include(x => x.Agent)
                .Include(x => x.ParentProperty)
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId);

        public async Task<PagedResult<BuildingProperty>> GetPagedAsync(Guid companyId, int page, int pageSize)
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

            return new PagedResult<BuildingProperty>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> ExistsAsync(Guid id, Guid companyId) =>
            await _dbSet.AnyAsync(x => x.Id == id && x.CompanyId == companyId);

        public async Task<bool> HasChildrenAsync(Guid buildingId, Guid companyId) =>
            await _context.BuildingProperties
                .AnyAsync(x => x.ParentPropertyId == buildingId && x.CompanyId == companyId);
    }
}
