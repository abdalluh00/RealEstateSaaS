using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.DTOs.Properties.Land;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;
using RealEstate.Application.Common.Extensions;


namespace RealEstate.Infrastructure.Repositories.Properties
{
    public class LandRepository : GenericRepository<LandProperty>, ILandRepository
    {
        public LandRepository(AppDbContext context) : base(context) { }

        // ── Reusable list projection ──────────────────────
        private static readonly Expression<Func<LandProperty, LandListDto>> ToListDto =
            l => new LandListDto
            {
                Id = l.Id,
                PropertyCode = l.PropertyCode,
                Title = l.Title,
                Type = l.Type.ToArabicString(),
                Purpose = l.Purpose.ToArabicString(),
                Status = l.PropertyStatus.ToArabicString(),
                Price = l.Price,
                Area = l.Area,
                City = l.City,
                District = l.District,
                UnitNumber = l.UnitNumber,
                IsFeatured = l.IsFeatured,
                CreatedAt = l.CreatedAt,

                // ── Land specific ─────────────────────────
                StreetWidth = l.StreetWidth,
                ZoningType = l.ZoningType.ToArabicString(),
                IsCornerLand = l.IsCornerLand
            };

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<LandListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            ZoningType? zoningType = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(l => l.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(l => l.PropertyStatus == status.Value);

            if (purpose.HasValue)
                query = query.Where(l => l.Purpose == purpose.Value);

            if (zoningType.HasValue)
                query = query.Where(l => l.ZoningType == zoningType.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<LandListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(l => l.IsFeatured)
                .ThenByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<LandListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<LandDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .Where(l => l.Id == id && l.CompanyId == companyId)
                .Select(l => new LandDetailDto
                {
                    // ── Base ──────────────────────────────
                    Id = l.Id,
                    PropertyCode = l.PropertyCode,
                    Title = l.Title,
                    Type = l.Type.ToArabicString(),
                    Description = l.Description,
                    Purpose = l.Purpose.ToArabicString(),
                    Status = l.PropertyStatus.ToArabicString(),
                    Price = l.Price,
                    Area = l.Area,
                    City = l.City,
                    District = l.District,
                    Address = l.Address,
                    UnitNumber = l.UnitNumber,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    ParkingSpots = l.ParkingSpots,
                    AgeInYears = l.AgeInYears,
                    FacingDirection = l.FacingDirection.ToArabicString(),
                    RegaLicenseNumber = l.RegaLicenseNumber,
                    DeedNumber = l.DeedNumber,
                    MunicipalityNumber = l.MunicipalityNumber,
                    IsFeatured = l.IsFeatured,
                    IsPublished = l.IsPublished,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,

                    // ── Owner (navigation — single join) ──
                    OwnerId = l.OwnerId,
                    OwnerName = l.Owner != null ? l.Owner.FullName : null,
                    OwnerPhone = l.Owner != null ? l.Owner.Phone : null,

                    // ── Agent (navigation — single join) ──
                    AgentId = l.AgentId,
                    AgentName = l.Agent != null ? l.Agent.FullName : null,
                    AgentPhone = l.Agent != null ? l.Agent.Phone : null,

                    // ── Land specific ─────────────────────
                    StreetWidth = l.StreetWidth,
                    NumberOfStreets = l.NumberOfStreets,
                    ZoningType = l.ZoningType.ToArabicString(),
                    LandShape = l.LandShape.ToArabicString(),
                    IsCornerLand = l.IsCornerLand,
                    IsWalled = l.IsWalled,
                    HasElectricity = l.HasElectricity,
                    HasWater = l.HasWater,
                    HasSewer = l.HasSewer,

                    // ── Media (navigation) ────────────────
                    Media = l.Media
                        .OrderBy(m => m.SortOrder)
                        .Select(m => new PropertyMediaDto
                        {
                            Id = m.Id,
                            MediaUrl = m.MediaUrl,
                            MediaType = m.MediaType.ToString(),
                            IsCover = m.IsCover,
                            SortOrder = m.SortOrder
                        })
                        .ToList(),

                    // ── Documents (navigation) ────────────
                    Documents = l.Documents
                        .Select(d => new PropertyDocumentDto
                        {
                            Id = d.Id,
                            DocumentType = d.DocumentType.ToString(),
                            DocumentName = d.DocumentName,
                            FileUrl = d.FileUrl,
                            ExpiryDate = d.ExpiryDate
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

        // ── Validation ────────────────────────────────────
        public async Task<bool> IsAvailableAsync(
            Guid id,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .AnyAsync(l => l.Id == id
                            && l.PropertyStatus == PropertyStatus.Available, ct);
    }
}