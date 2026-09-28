using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.DTOs.Properties.Villa;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;
using RealEstate.Application.Common.Extensions;


namespace RealEstate.Infrastructure.Repositories.Properties
{
    public class VillaRepository : GenericRepository<VillaProperty>, IVillaRepository
    {
        public VillaRepository(AppDbContext context) : base(context) { }

        private static readonly Expression<Func<VillaProperty, VillaListDto>> ToListDto =
            v => new VillaListDto
            {
                Id = v.Id,
                PropertyCode = v.PropertyCode,
                Title = v.Title,
                Type = v.Type.ToArabicString(),
                Purpose = v.Purpose.ToArabicString(),
                Status = v.PropertyStatus.ToArabicString(),
                Price = v.Price,
                Area = v.Area,
                City = v.City,
                District = v.District,
                UnitNumber = v.UnitNumber,
                IsFeatured = v.IsFeatured,
                CreatedAt = v.CreatedAt,
                Bedrooms = v.Bedrooms,
                Bathrooms = v.Bathrooms,
                HasPool = v.HasPool,
                HasMajlis = v.HasMajlis,
                FurnishedStatus = v.FurnishedStatus.ToArabicString()
            };

        public async Task<PagedResult<VillaListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            int? minBedrooms = null,
            int? maxBedrooms = null,
            FurnishedStatus? furnishedStatus = null,
            bool? hasPool = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(v => v.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(v => v.PropertyStatus == status.Value);

            if (purpose.HasValue)
                query = query.Where(v => v.Purpose == purpose.Value);

            if (minBedrooms.HasValue)
                query = query.Where(v => v.Bedrooms >= minBedrooms.Value);

            if (maxBedrooms.HasValue)
                query = query.Where(v => v.Bedrooms <= maxBedrooms.Value);

            if (furnishedStatus.HasValue)
                query = query.Where(v => v.FurnishedStatus == furnishedStatus.Value);

            if (hasPool.HasValue)
                query = query.Where(v => v.HasPool == hasPool.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<VillaListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(v => v.IsFeatured)
                .ThenByDescending(v => v.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<VillaListDto>.Create(items, totalCount, page, pageSize);
        }

        public async Task<VillaDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .Where(v => v.Id == id && v.CompanyId == companyId)
                .Select(v => new VillaDetailDto
                {
                    Id = v.Id,
                    PropertyCode = v.PropertyCode,
                    Title = v.Title,
                    Type = v.Type.ToArabicString(),
                    Description = v.Description,
                    Purpose = v.Purpose.ToArabicString(),
                    Status = v.PropertyStatus.ToArabicString(),
                    Price = v.Price,
                    Area = v.Area,
                    City = v.City,
                    District = v.District,
                    Address = v.Address,
                    UnitNumber = v.UnitNumber,
                    Latitude = v.Latitude,
                    Longitude = v.Longitude,
                    ParkingSpots = v.ParkingSpots,
                    AgeInYears = v.AgeInYears,
                    FacingDirection = v.FacingDirection.ToArabicString(),
                    RegaLicenseNumber = v.RegaLicenseNumber,
                    DeedNumber = v.DeedNumber,
                    MunicipalityNumber = v.MunicipalityNumber,
                    IsFeatured = v.IsFeatured,
                    IsPublished = v.IsPublished,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt,
                    OwnerId = v.OwnerId,
                    OwnerName = v.Owner != null ? v.Owner.FullName : null,
                    OwnerPhone = v.Owner != null ? v.Owner.Phone : null,
                    AgentId = v.AgentId,
                    AgentName = v.Agent != null ? v.Agent.FullName : null,
                    AgentPhone = v.Agent != null ? v.Agent.Phone : null,
                    Bedrooms = v.Bedrooms,
                    Bathrooms = v.Bathrooms,
                    Floors = v.Floors,
                    LivingRooms = v.LivingRooms,
                    HasMaidRoom = v.HasMaidRoom,
                    HasDriverRoom = v.HasDriverRoom,
                    HasPool = v.HasPool,
                    HasGarden = v.HasGarden,
                    GardenArea = v.GardenArea,
                    HasElevator = v.HasElevator,
                    HasMosque = v.HasMosque,
                    HasMajlis = v.HasMajlis,
                    HasStorage = v.HasStorage,
                    HasCCTV = v.HasCCTV,
                    HasGenerator = v.HasGenerator,
                    FurnishedStatus = v.FurnishedStatus.ToArabicString(),
                    Media = v.Media
                        .OrderBy(m => m.SortOrder)
                        .Select(m => new PropertyMediaDto
                        {
                            Id = m.Id,
                            MediaUrl = m.MediaUrl,
                            MediaType = m.MediaType.ToString(),
                            IsCover = m.IsCover,
                            SortOrder = m.SortOrder
                        }).ToList(),
                    Documents = v.Documents
                        .Select(d => new PropertyDocumentDto
                        {
                            Id = d.Id,
                            DocumentType = d.DocumentType.ToString(),
                            DocumentName = d.DocumentName,
                            FileUrl = d.FileUrl,
                            ExpiryDate = d.ExpiryDate
                        }).ToList()
                })
                .FirstOrDefaultAsync(ct);

        public async Task<bool> IsAvailableAsync(
            Guid id,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .AnyAsync(v => v.Id == id
                            && v.PropertyStatus == PropertyStatus.Available, ct);

        public async Task<bool> UnitNumberExistsAsync(
            string unitNumber,
            Guid parentPropertyId,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .AnyAsync(v => v.UnitNumber == unitNumber
                            && v.ParentPropertyId == parentPropertyId, ct);
    }
}