using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.DTOs.Properties.Office;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;
using RealEstate.Application.Common.Extensions;

namespace RealEstate.Infrastructure.Repositories.Properties
{
    public class OfficeRepository : GenericRepository<OfficeProperty>, IOfficeRepository
    {
        public OfficeRepository(AppDbContext context) : base(context) { }

        private static readonly Expression<Func<OfficeProperty, OfficeListDto>> ToListDto =
            o => new OfficeListDto
            {
                Id = o.Id,
                PropertyCode = o.PropertyCode,
                Title = o.Title,
                Type = o.Type.ToArabicString(),
                Purpose = o.Purpose.ToArabicString(),
                Status = o.PropertyStatus.ToArabicString(),
                Price = o.Price,
                Area = o.Area,
                City = o.City,
                District = o.District,
                UnitNumber = o.UnitNumber,
                IsFeatured = o.IsFeatured,
                CreatedAt = o.CreatedAt,
                FloorNumber = o.FloorNumber,
                OfficesCount = o.OfficesCount,
                HasElevator = o.HasElevator,
                FurnishedStatus = o.FurnishedStatus.ToArabicString()
            };

        public async Task<PagedResult<OfficeListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            FurnishedStatus? furnishedStatus = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(o => o.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(o => o.PropertyStatus == status.Value);

            if (purpose.HasValue)
                query = query.Where(o => o.Purpose == purpose.Value);

            if (furnishedStatus.HasValue)
                query = query.Where(o => o.FurnishedStatus == furnishedStatus.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<OfficeListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(o => o.IsFeatured)
                .ThenByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<OfficeListDto>.Create(items, totalCount, page, pageSize);
        }

        public async Task<OfficeDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .Where(o => o.Id == id && o.CompanyId == companyId)
                .Select(o => new OfficeDetailDto
                {
                    Id = o.Id,
                    PropertyCode = o.PropertyCode,
                    Title = o.Title,
                    Type = o.Type.ToArabicString(),
                    Description = o.Description,
                    Purpose = o.Purpose.ToArabicString(),
                    Status = o.PropertyStatus.ToArabicString(),
                    Price = o.Price,
                    Area = o.Area,
                    City = o.City,
                    District = o.District,
                    Address = o.Address,
                    UnitNumber = o.UnitNumber,
                    Latitude = o.Latitude,
                    Longitude = o.Longitude,
                    ParkingSpots = o.ParkingSpots,
                    AgeInYears = o.AgeInYears,
                    FacingDirection = o.FacingDirection.ToArabicString(),
                    RegaLicenseNumber = o.RegaLicenseNumber,
                    DeedNumber = o.DeedNumber,
                    MunicipalityNumber = o.MunicipalityNumber,
                    IsFeatured = o.IsFeatured,
                    IsPublished = o.IsPublished,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    OwnerId = o.OwnerId,
                    OwnerName = o.Owner != null ? o.Owner.FullName : null,
                    OwnerPhone = o.Owner != null ? o.Owner.Phone : null,
                    AgentId = o.AgentId,
                    AgentName = o.Agent != null ? o.Agent.FullName : null,
                    AgentPhone = o.Agent != null ? o.Agent.Phone : null,
                    FloorNumber = o.FloorNumber,
                    Bathrooms = o.Bathrooms,
                    OfficesCount = o.OfficesCount,
                    MeetingRooms = o.MeetingRooms,
                    HasElevator = o.HasElevator,
                    HasCentralAC = o.HasCentralAC,
                    HasReceptionArea = o.HasReceptionArea,
                    HasKitchen = o.HasKitchen,
                    HasStorage = o.HasStorage,
                    HasCCTV = o.HasCCTV,
                    FurnishedStatus = o.FurnishedStatus.ToArabicString(),
                    Media = o.Media
                        .OrderBy(m => m.SortOrder)
                        .Select(m => new PropertyMediaDto
                        {
                            Id = m.Id,
                            MediaUrl = m.MediaUrl,
                            MediaType = m.MediaType.ToString(),
                            IsCover = m.IsCover,
                            SortOrder = m.SortOrder
                        }).ToList(),
                    Documents = o.Documents
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
                .AnyAsync(o => o.Id == id
                            && o.PropertyStatus == PropertyStatus.Available, ct);

        public async Task<bool> UnitNumberExistsAsync(
            string unitNumber,
            Guid parentPropertyId,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .AnyAsync(o => o.UnitNumber == unitNumber
                            && o.ParentPropertyId == parentPropertyId, ct);
    }
}