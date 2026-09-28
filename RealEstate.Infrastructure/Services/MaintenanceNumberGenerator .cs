using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Services
{
    public class MaintenanceNumberGenerator : IMaintenanceNumberGenerator
    {
        private readonly AppDbContext _context;

        public MaintenanceNumberGenerator(AppDbContext context) => _context = context;

        public async Task<string> GenerateAsync(
            Guid companyId,
            CancellationToken ct = default)
        {
            var year = DateTime.UtcNow.Year;
            var count = await _context.MaintenanceRequests
                .CountAsync(x => x.CompanyId == companyId, ct);

            return $"MAINT-{year}-{(count + 1):D4}";
        }
    }
}