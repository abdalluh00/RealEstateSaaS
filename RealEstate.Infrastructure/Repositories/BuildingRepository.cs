using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Application.Common.Extensions;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;

namespace RealEstate.Infrastructure.Repositories.Properties
{
    public class BuildingRepository : GenericRepository<BuildingProperty>, IBuildingRepository
    {
        public BuildingRepository(AppDbContext context) : base(context) { }

        // ── Reusable list projection ──────────────────────
        private static readonly Expression<Func<BuildingProperty, BuildingListDto>> ToListDto =
            b => new BuildingListDto
            {
                Id = b.Id,
                PropertyCode = b.PropertyCode,
                Title = b.Title,
                Type = b.Type.ToArabicString(),
                Purpose = b.Purpose.ToArabicString(),
                Status = b.PropertyStatus.ToArabicString(),
                Price = b.Price,
                Area = b.Area,
                City = b.City,
                District = b.District,
                UnitNumber = b.UnitNumber,
                IsFeatured = b.IsFeatured,
                CreatedAt = b.CreatedAt,
                TotalFloors = b.TotalFloors,
                UnitsCount = b.UnitsCount,
                HasElevator = b.HasElevator
            };

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<BuildingListDto>> GetPagedAsync(Guid companyId, int page, int pageSize, PropertyStatus? status = null, PropertyPurpose? purpose = null, bool? hasElevator = null, int? minFloors = null, int? maxFloors = null, CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(b => b.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(b => b.PropertyStatus == status.Value);

            if (purpose.HasValue)
                query = query.Where(b => b.Purpose == purpose.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<BuildingListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(b => b.IsFeatured)
                .ThenByDescending(b => b.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<BuildingListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<BuildingDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .Where(b => b.Id == id && b.CompanyId == companyId)
                .Select(b => new BuildingDetailDto
                {
                    // ── Base ──────────────────────────────
                    Id = b.Id,
                    PropertyCode = b.PropertyCode,
                    Title = b.Title,
                    Type = b.Type.ToArabicString(),
                    Description = b.Description,
                    Purpose = b.Purpose.ToArabicString(),
                    Status = b.PropertyStatus.ToArabicString(),
                    Price = b.Price,
                    Area = b.Area,
                    City = b.City,
                    District = b.District,
                    Address = b.Address,
                    UnitNumber = b.UnitNumber,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude,
                    ParkingSpots = b.ParkingSpots,
                    AgeInYears = b.AgeInYears,
                    FacingDirection = b.FacingDirection.ToString(),
                    RegaLicenseNumber = b.RegaLicenseNumber,
                    DeedNumber = b.DeedNumber,
                    MunicipalityNumber = b.MunicipalityNumber,
                    IsFeatured = b.IsFeatured,
                    IsPublished = b.IsPublished,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt,

                    // ── Owner (navigation — single join) ──
                    OwnerId = b.OwnerId,
                    OwnerName = b.Owner != null ? b.Owner.FullName : null,
                    OwnerPhone = b.Owner != null ? b.Owner.Phone : null,

                    // ── Agent (navigation — single join) ──
                    AgentId = b.AgentId,
                    AgentName = b.Agent != null ? b.Agent.FullName : null,
                    AgentPhone = b.Agent != null ? b.Agent.Phone : null,

                    // ── Building specific ─────────────────
                    TotalFloors = b.TotalFloors,
                    UnitsCount = b.UnitsCount,
                    BasementFloors = b.BasementFloors,
                    HasElevator = b.HasElevator,
                    HasParkingFloor = b.HasParkingFloor,
                    HasMosque = b.HasMosque,
                    HasGuard = b.HasGuard,
                    HasGenerator = b.HasGenerator,
                    HasCCTV = b.HasCCTV,

                    // ── Units (children via ParentPropertyId) ──
                    // Uses _context.Properties (base table only — no subtype joins)
                    // Type comes from PropertyType enum — no discriminator needed
                    Units = _context.Properties
                        .Where(p => p.ParentPropertyId == b.Id)
                        .OrderBy(p => p.UnitNumber)
                        .Select(p => new PropertyListDto
                        {
                            Id = p.Id,
                            PropertyCode = p.PropertyCode,
                            Title = p.Title,
                            Type = p.Type.ToArabicString(),
                            Purpose = p.Purpose.ToArabicString(),
                            Status = p.PropertyStatus.ToArabicString(),
                            Price = p.Price,
                            Area = p.Area,
                            City = p.City,
                            District = p.District,
                            UnitNumber = p.UnitNumber,
                            IsFeatured = p.IsFeatured,
                            CreatedAt = p.CreatedAt
                        })
                        .ToList(),

                    // ── Media (navigation) ────────────────
                    Media = b.Media
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
                    Documents = b.Documents
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
        public async Task<bool> HasUnitsAsync(
            Guid buildingId,
            CancellationToken ct = default) =>
            await _context.Properties
                .AnyAsync(p => p.ParentPropertyId == buildingId, ct);

        public async Task<int> CountUnitsByStatusAsync(
            Guid buildingId,
            PropertyStatus status,
            CancellationToken ct = default) =>
            await _context.Properties
                .CountAsync(p => p.ParentPropertyId == buildingId
                              && p.PropertyStatus == status, ct);


    }
}