using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Infrastructure.Repositories
{
    public class VillaRepository : GenericRepository<VillaProperty>, IVillaRepository
    {
        public VillaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<VillaProperty?> GetByIdAsync(Guid id, Guid companyId, CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId, ct);
        }

        public async Task<PagedResult<VillaProperty>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<VillaProperty>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
