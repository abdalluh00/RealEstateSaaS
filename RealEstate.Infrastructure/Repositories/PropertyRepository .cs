using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Properties;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(AppDbContext context) : base(context) { }

        // ── Reusable projection ───────────────────────────
        private static readonly Expression<Func<Property, PropertyListDto>> ToListDto =
            x => new PropertyListDto
            {
                Id = x.Id,
                PropertyCode = x.PropertyCode,
                Title = x.Title,
                Type = x.Type.ToString(),
                Purpose = x.Purpose.ToString(),
                Status = x.PropertyStatus.ToString(),
                Price = x.Price,
                Area = x.Area,
                City = x.City,
                District = x.District,
                UnitNumber = x.UnitNumber,
                IsFeatured = x.IsFeatured,
                CreatedAt = x.CreatedAt
            };

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

        public async Task<bool> ExistsAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(x => x.Id == id && x.CompanyId == companyId, ct);

        public async Task<bool> IsAvailableAsync(
            Guid id,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(x => x.Id == id && x.PropertyStatus == PropertyStatus.Available, ct);

        public async Task<bool> IsOwnerHasPropertyAsync(
            Guid ownerId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(x => x.OwnerId == ownerId, ct);

        public async Task<int> CountByStatusAsync(
            Guid companyId,
            PropertyStatus status,
            CancellationToken ct = default) =>
            await _dbSet
                .CountAsync(x => x.CompanyId == companyId && x.PropertyStatus == status, ct);

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<PropertyListDto>> GetPagedAsync(
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
                return PagedResult<PropertyListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<PropertyListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Agent Properties ──────────────────────────────
        public async Task<PagedResult<PropertyListDto>> GetByAgentAsync(
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
                return PagedResult<PropertyListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<PropertyListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Owner Properties ──────────────────────────────
        public async Task<PagedResult<PropertyListDto>> GetByOwnerAsync(
            Guid ownerId,
            Guid companyId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.OwnerId == ownerId && x.CompanyId == companyId);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<PropertyListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<PropertyListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Children ──────────────────────────────────────
        public async Task<IReadOnlyList<PropertyListDto>> GetChildrenAsync(
            Guid parentPropertyId,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.ParentPropertyId == parentPropertyId
                         && x.CompanyId == companyId)
                .OrderBy(x => x.UnitNumber)
                .Select(ToListDto)
                .ToListAsync(ct);

        // ── Featured ──────────────────────────────────────
        public async Task<IReadOnlyList<PropertyListDto>> GetFeaturedAsync(
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
                .Select(ToListDto)
                .ToListAsync(ct);

        // ── Dashboard ─────────────────────────────────────
        public async Task<PropertyDashboardDto> GetDashboardStatsAsync(
            Guid companyId,
            int featuredLimit,
            CancellationToken ct = default)
        {
            var statsTask = _dbSet
                .Where(x => x.CompanyId == companyId)
                .GroupBy(_ => 1)
                .Select(g => new
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

            var featuredTask = _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId
                         && x.IsFeatured
                         && x.IsPublished)
                .OrderByDescending(x => x.CreatedAt)
                .Take(featuredLimit)
                .Select(ToListDto)
                .ToListAsync(ct);

            await Task.WhenAll(statsTask, featuredTask);

            var stats = await statsTask;
            var featured = await featuredTask;

            return new PropertyDashboardDto
            {
                TotalProperties = stats?.TotalProperties ?? 0,
                Available = stats?.Available ?? 0,
                Rented = stats?.Rented ?? 0,
                Sold = stats?.Sold ?? 0,
                Reserved = stats?.Reserved ?? 0,
                Featured = stats?.Featured ?? 0,
                Published = stats?.Published ?? 0,
                TopFeatured = featured
            };
        }

        // ── Commands ──────────────────────────────────────
        public async Task<Property?> GetByIdForDeleteAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId, ct);
    }
}