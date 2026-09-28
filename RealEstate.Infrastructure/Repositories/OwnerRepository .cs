using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Owner;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;
using RealEstate.Application.Common.Extensions;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(AppDbContext context) : base(context) { }

        // ── Reusable list projection ──────────────────────
        private static readonly Expression<Func<Owner, OwnerListDto>> ToListDto =
            o => new OwnerListDto
            {
                Id = o.Id,
                FullName = o.FullName,
                Phone = o.Phone,
                Email = o.Email,
                OwnerType = o.OwnerType.ToArabicString(),
                CompanyName = o.CompanyName,
                IsActive = o.IsActive,
                PropertyCount = o.Properties.Count(p => !p.IsDeleted),
                CreatedAt = o.CreatedAt
            };

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<OwnerListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            bool? isActive = null,
            OwnerType? ownerType = null,
            string? search = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(o => o.CompanyId == companyId);

            if (isActive.HasValue)
                query = query.Where(o => o.IsActive == isActive.Value);

            if (ownerType.HasValue)
                query = query.Where(o => o.OwnerType == ownerType.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(o =>
                    o.FullName.Contains(search) ||
                    o.Phone.Contains(search) ||
                    (o.CompanyName != null && o.CompanyName.Contains(search)));

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<OwnerListDto>.Empty(page, pageSize);

            var items = await query
                .OrderBy(o => o.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<OwnerListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<OwnerDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(o => o.Id == id && o.CompanyId == companyId)
                .Select(o => new OwnerDetailDto
                {
                    Id = o.Id,
                    FullName = o.FullName,
                    Phone = o.Phone,
                    Email = o.Email,
                    OwnerType = o.OwnerType.ToArabicString(),
                    CompanyName = o.CompanyName,
                    Nationality = o.Nationality,
                    IsActive = o.IsActive,
                    Notes = o.Notes,
                    CommissionRate = o.CommissionRate,
                    PropertyCount = o.Properties.Count(p => !p.IsDeleted),
                    NationalId = o.NationalId,
                    IBAN = o.IBAN,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);

        // ── Validation ────────────────────────────────────
        public async Task<bool> PhoneExistsAsync(
            string phone,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(o => o.Phone == phone
                            && o.CompanyId == companyId, ct);

        public async Task<bool> PhoneExistsForAnotherOwnerAsync(
            string phone,
            Guid ownerId,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(o => o.Phone == phone
                            && o.CompanyId == companyId
                            && o.Id != ownerId, ct);

        public async Task<bool> ExistsInCompanyAsync(
            Guid ownerId,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(o => o.Id == ownerId
                            && o.CompanyId == companyId, ct);
    }
}