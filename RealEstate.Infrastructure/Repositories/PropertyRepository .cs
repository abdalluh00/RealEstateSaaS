using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Domain.ReadModels.PropertyModel;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(AppDbContext context) : base(context) { }

        // ── Validation ────────────────────────────────────

        public async Task<bool> PropertyCodeExistsAsync(
            string code,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(x => x.PropertyCode == code && x.CompanyId == companyId, ct);

        public async Task<bool> IsOwnerLinkedToAnyPropertyAsync(
            Guid ownerId,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(x => x.OwnerId == ownerId && x.CompanyId == companyId, ct);

        // ── Paged List ────────────────────────────────────

        public async Task<PagedResult<PropertyListReadModel>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            string? city = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(x => x.PropertyStatus == status.Value);

            if (purpose.HasValue)
                query = query.Where(x => x.Purpose == purpose.Value);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(x => x.City == city);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<PropertyListReadModel>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PropertyListReadModel
                {
                    Id = x.Id,
                    PropertyCode = x.PropertyCode,
                    Title = x.Title,
                    Purpose = x.Purpose,
                    PropertyStatus = x.PropertyStatus,
                    Price = x.Price,
                    Area = x.Area,
                    City = x.City,
                    District = x.District,
                    IsFeatured = x.IsFeatured,
                    IsPublished = x.IsPublished,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(ct);

            return PagedResult<PropertyListReadModel>.Create(items, totalCount, page, pageSize);
        }

        // ── Agent Properties ──────────────────────────────

        public async Task<PagedResult<PropertyListReadModel>> GetByAgentAsync(
            Guid agentId,
            Guid companyId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.AgentId == agentId && x.CompanyId == companyId);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<PropertyListReadModel>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PropertyListReadModel
                {
                    Id = x.Id,
                    PropertyCode = x.PropertyCode,
                    Title = x.Title,
                    Purpose = x.Purpose,
                    PropertyStatus = x.PropertyStatus,
                    Price = x.Price,
                    Area = x.Area,
                    City = x.City,
                    District = x.District,
                    IsFeatured = x.IsFeatured,
                    IsPublished = x.IsPublished,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(ct);

            return PagedResult<PropertyListReadModel>.Create(items, totalCount, page, pageSize);
        }

        // ── Dashboard ─────────────────────────────────────

        public async Task<PropertyDashboardReadModel> GetDashboardStatsAsync(
            Guid companyId,
            CancellationToken ct = default)
        {
            // Single query — grouped aggregation in SQL
            // No subtype joins — only Properties table
            var stats = await _dbSet
                .Where(x => x.CompanyId == companyId)
                .GroupBy(_ => 1)                          // group all into one row
                .Select(g => new PropertyDashboardReadModel
                {
                    TotalProperties = g.Count(),
                    Available = g.Count(x => x.PropertyStatus == PropertyStatus.Available),
                    Rented = g.Count(x => x.PropertyStatus == PropertyStatus.Rented),
                    Sold = g.Count(x => x.PropertyStatus == PropertyStatus.Sold),
                    Reserved = g.Count(x => x.PropertyStatus == PropertyStatus.Reserved),
                    Featured = g.Count(x => x.IsFeatured),
                    Published = g.Count(x => x.IsPublished)
                })
                .FirstOrDefaultAsync(ct);

            // If company has no properties yet return zeros
            return stats ?? new PropertyDashboardReadModel();
        }

        public async Task<IReadOnlyList<PropertyListReadModel>> GetFeaturedAsync(
            Guid companyId,
            int limit,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId
                         && x.IsFeatured
                         && x.IsPublished)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .Select(x => new PropertyListReadModel
                {
                    Id = x.Id,
                    PropertyCode = x.PropertyCode,
                    Title = x.Title,
                    Purpose = x.Purpose,
                    PropertyStatus = x.PropertyStatus,
                    Price = x.Price,
                    Area = x.Area,
                    City = x.City,
                    District = x.District,
                    IsFeatured = x.IsFeatured,
                    IsPublished = x.IsPublished,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(ct);

        // ── Commands ──────────────────────────────────────

        public async Task<Property?> GetByIdForDeleteAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId, ct);
    }
}
