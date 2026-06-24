using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Infrastructure.Repositories
{
    public class OfficeRepository : GenericRepository<OfficeProperty>, IOfficeRepository
    {
        public OfficeRepository(AppDbContext context) : base(context) { }

        public async Task<OfficeProperty?> GetByIdForCompanyAsync(
            Guid officeId,
            Guid companyId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(
                    x => x.Id == officeId && x.CompanyId == companyId,
                    ct);
        }
    }
}
