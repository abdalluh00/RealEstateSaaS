using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class MaintenanceMediaRepository
        : GenericRepository<MaintenanceMedia>, IMaintenanceMediaRepository
    {
        public MaintenanceMediaRepository(AppDbContext context) : base(context) { }

        public async Task<MaintenanceMedia?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(m => m.Id == id
                                       && m.CompanyId == companyId, ct);
    }
}