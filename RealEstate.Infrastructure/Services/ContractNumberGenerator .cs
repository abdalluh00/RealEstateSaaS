using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Services
{
    public class ContractNumberGenerator : IContractNumberGenerator
    {
        private readonly AppDbContext _context;

        public ContractNumberGenerator(AppDbContext context) => _context = context;

        public async Task<string> GenerateAsync(
            Guid companyId,
            CancellationToken ct = default)
        {
            var year = DateTime.UtcNow.Year;

            var count = await _context.Contracts
                .CountAsync(x => x.CompanyId == companyId, ct);

            return $"CONT-{year}-{(count + 1):D4}";
        }
    }
}