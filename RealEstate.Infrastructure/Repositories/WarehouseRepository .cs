using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.DTOs.Properties.Warehouse;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;
using RealEstate.Application.Common.Extensions;
namespace RealEstate.Infrastructure.Repositories.Properties
{
    public class WarehouseRepository : GenericRepository<WarehouseProperty>, IWarehouseRepository
    {
        public WarehouseRepository(AppDbContext context) : base(context) { }

        private static readonly Expression<Func<WarehouseProperty, WarehouseListDto>> ToListDto =
            w => new WarehouseListDto
            {
                Id = w.Id,
                PropertyCode = w.PropertyCode,
                Title = w.Title,
                Type = w.Type.ToArabicString(),
                Purpose = w.Purpose.ToArabicString(),
                Status = w.PropertyStatus.ToArabicString(),
                Price = w.Price,
                Area = w.Area,
                City = w.City,
                District = w.District,
                UnitNumber = w.UnitNumber,
                IsFeatured = w.IsFeatured,
                CreatedAt = w.CreatedAt,
                CeilingHeight = w.CeilingHeight,
                LoadingDocks = w.LoadingDocks,
                ElectricityCapacity = w.ElectricityCapacity.ToArabicString(),
                HasColdStorage = w.HasColdStorage
            };

        public async Task<PagedResult<WarehouseListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            bool? hasColdStorage = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(w => w.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(w => w.PropertyStatus == status.Value);

            if (purpose.HasValue)
                query = query.Where(w => w.Purpose == purpose.Value);

            if (hasColdStorage.HasValue)
                query = query.Where(w => w.HasColdStorage == hasColdStorage.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<WarehouseListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(w => w.IsFeatured)
                .ThenByDescending(w => w.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<WarehouseListDto>.Create(items, totalCount, page, pageSize);
        }

        public async Task<WarehouseDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await QueryNoTracking()
                .Where(w => w.Id == id && w.CompanyId == companyId)
                .Select(w => new WarehouseDetailDto
                {
                    Id = w.Id,
                    PropertyCode = w.PropertyCode,
                    Title = w.Title,
                    Type = w.Type.ToArabicString(),
                    Description = w.Description,
                    Purpose = w.Purpose.ToArabicString(),
                    Status = w.PropertyStatus.ToArabicString(),
                    Price = w.Price,
                    Area = w.Area,
                    City = w.City,
                    District = w.District,
                    Address = w.Address,
                    UnitNumber = w.UnitNumber,
                    Latitude = w.Latitude,
                    Longitude = w.Longitude,
                    ParkingSpots = w.ParkingSpots,
                    AgeInYears = w.AgeInYears,
                    FacingDirection = w.FacingDirection.ToArabicString(),
                    RegaLicenseNumber = w.RegaLicenseNumber,
                    DeedNumber = w.DeedNumber,
                    MunicipalityNumber = w.MunicipalityNumber,
                    IsFeatured = w.IsFeatured,
                    IsPublished = w.IsPublished,
                    CreatedAt = w.CreatedAt,
                    UpdatedAt = w.UpdatedAt,
                    OwnerId = w.OwnerId,
                    OwnerName = w.Owner != null ? w.Owner.FullName : null,
                    OwnerPhone = w.Owner != null ? w.Owner.Phone : null,
                    AgentId = w.AgentId,
                    AgentName = w.Agent != null ? w.Agent.FullName : null,
                    AgentPhone = w.Agent != null ? w.Agent.Phone : null,
                    CeilingHeight = w.CeilingHeight,
                    LoadingDocks = w.LoadingDocks,
                    GateCount = w.GateCount,
                    ElectricityCapacity = w.ElectricityCapacity.ToArabicString(),
                    HasOfficeSpace = w.HasOfficeSpace,
                    HasSecurityRoom = w.HasSecurityRoom,
                    HasCCTV = w.HasCCTV,
                    HasFireSystem = w.HasFireSystem,
                    HasColdStorage = w.HasColdStorage,
                    HasMosanada = w.HasMosanada,
                    IsFenced = w.IsFenced,
                    HasTruckAccess = w.HasTruckAccess,
                    Media = w.Media
                        .OrderBy(m => m.SortOrder)
                        .Select(m => new PropertyMediaDto
                        {
                            Id = m.Id,
                            MediaUrl = m.MediaUrl,
                            MediaType = m.MediaType.ToString(),
                            IsCover = m.IsCover,
                            SortOrder = m.SortOrder
                        }).ToList(),
                    Documents = w.Documents
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
                .AnyAsync(w => w.Id == id
                            && w.PropertyStatus == PropertyStatus.Available, ct);
    }
}