using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Properties.Apartment;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Application.Common.Extensions;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;

namespace RealEstate.Infrastructure.Repositories.Properties
{
    public class ApartmentRepository : GenericRepository<ApartmentProperty>, IApartmentRepository
    {
        public ApartmentRepository(AppDbContext context) : base(context) { }

        // ── Reusable list projection ──────────────────────
        private static readonly Expression<Func<ApartmentProperty, ApartmentListDto>> ToListDto =
            a => new ApartmentListDto
            {
                // ── Base fields ───────────────────────────
                Id = a.Id,
                PropertyCode = a.PropertyCode,
                Title = a.Title,
                Type = a.Type.ToArabicString(),
                Purpose = a.Purpose.ToArabicString(),
                Status = a.PropertyStatus.ToArabicString(),
                Price = a.Price,
                Area = a.Area,
                City = a.City,
                District = a.District,
                UnitNumber = a.UnitNumber,
                IsFeatured = a.IsFeatured,
                CreatedAt = a.CreatedAt,

                // ── Apartment specific ────────────────────
                Bedrooms = a.Bedrooms,
                Bathrooms = a.Bathrooms,
                FloorNumber = a.FloorNumber,
                HasElevator = a.HasElevator,
                FurnishedStatus = a.FurnishedStatus.ToString(),
            };

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<ApartmentListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            int? minBedrooms = null,
            int? maxBedrooms = null,
            FurnishedStatus? furnishedStatus = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(a => a.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(a => a.PropertyStatus == status.Value);

            if (purpose.HasValue)
                query = query.Where(a => a.Purpose == purpose.Value);

            if (minBedrooms.HasValue)
                query = query.Where(a => a.Bedrooms >= minBedrooms.Value);

            if (maxBedrooms.HasValue)
                query = query.Where(a => a.Bedrooms <= maxBedrooms.Value);

            if (furnishedStatus.HasValue)
                query = query.Where(a => a.FurnishedStatus == furnishedStatus.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<ApartmentListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(a => a.IsFeatured)
                .ThenByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<ApartmentListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<ApartmentDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .Where(a => a.Id == id && a.CompanyId == companyId)
                .Select(a => new ApartmentDetailDto
                {
                    // ── Base ──────────────────────────────
                    Id = a.Id,
                    PropertyCode = a.PropertyCode,
                    Title = a.Title,
                    Type = a.Type.ToArabicString(),
                    Description = a.Description,
                    Purpose = a.Purpose.ToArabicString(),
                    Status = a.PropertyStatus.ToArabicString(),
                    Price = a.Price,
                    Area = a.Area,
                    City = a.City,
                    District = a.District,
                    Address = a.Address,
                    UnitNumber = a.UnitNumber,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    ParkingSpots = a.ParkingSpots,
                    AgeInYears = a.AgeInYears,
                    FacingDirection = a.FacingDirection.ToString(),
                    RegaLicenseNumber = a.RegaLicenseNumber,
                    DeedNumber = a.DeedNumber,
                    MunicipalityNumber = a.MunicipalityNumber,
                    IsFeatured = a.IsFeatured,
                    IsPublished = a.IsPublished,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,

                    // ── Owner (single join, not two subqueries) ──
                    OwnerId = a.OwnerId,
                    OwnerName = a.Owner != null ? a.Owner.FullName : null,
                    OwnerPhone = a.Owner != null ? a.Owner.Phone : null,

                    // ── Agent (single join) ───────────────
                    AgentId = a.AgentId,
                    AgentName = a.Agent != null ? a.Agent.FullName : null,
                    AgentPhone = a.Agent != null ? a.Agent.Phone : null,

                    // ── Apartment specific ────────────────
                    Bedrooms = a.Bedrooms,
                    Bathrooms = a.Bathrooms,
                    FloorNumber = a.FloorNumber,
                    LivingRooms = a.LivingRooms,
                    HasMaidRoom = a.HasMaidRoom,
                    HasElevator = a.HasElevator,
                    HasCentralAC = a.HasCentralAC,
                    HasBalcony = a.HasBalcony,
                    HasStorage = a.HasStorage,
                    FurnishedStatus = a.FurnishedStatus.ToString(),

                    // ── Media (ordered, no extra roundtrip) ──
                    Media = a.Media
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

                    // ── Documents ─────────────────────────
                    Documents = a.Documents
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
                .AnyAsync(a => a.Id == id
                            && a.PropertyStatus == PropertyStatus.Available, ct);

        public async Task<bool> UnitNumberExistsAsync(
            string unitNumber,
            Guid parentPropertyId,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .AnyAsync(a => a.UnitNumber == unitNumber
                            && a.ParentPropertyId == parentPropertyId, ct);
    }
}