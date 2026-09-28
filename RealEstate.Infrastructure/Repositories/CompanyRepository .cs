using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Companies;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Application.Common.Extensions;

namespace RealEstate.Infrastructure.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context) { }

        public async Task<CompanyDto?> GetDetailByIdAsync(
            Guid id,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Logo = c.Logo,
                    Address = c.Address,
                    Phone = c.Phone,
                    SubscriptionPlan = c.SubscriptionPlan.ToArabicString(),
                    SubscriptionExpiry = c.SubscriptionExpiry,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync(ct);

        public async Task<Company?> GetByIdForCommandAsync(
            Guid id,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<bool> IsActiveAsync(
            Guid id,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(c => c.Id == id && c.IsActive, ct);

        public async Task<bool> IsSubscriptionValidAsync(
            Guid id,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(c => c.Id == id
                            && c.IsActive
                            && c.SubscriptionExpiry > DateTime.UtcNow, ct);
    }
}