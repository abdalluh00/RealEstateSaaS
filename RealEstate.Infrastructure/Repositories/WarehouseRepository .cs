using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Domain.ReadModels;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Infrastructure.Repositories
{
    public class WarehouseRepository : GenericRepository<WarehouseProperty>, IWarehouseRepository
    {
        public WarehouseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<WarehouseProperty?> GetByIdForUpdateAsync(Guid id, Guid companyId, CancellationToken ct = default)
        {
            return await _context.WarehouseProperties
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId, ct);
        }

        public async Task<WarehouseDetailsDto?> GetDetailsByIdAsync(Guid id, Guid companyId, CancellationToken ct = default)
        {
            return await _context.WarehouseProperties
                .AsNoTracking()
                .Where(x => x.Id == id && x.CompanyId == companyId)
                .Select(x => new WarehouseDetailsDto
                {
                    Id = x.Id,
                    PropertyCode = x.PropertyCode,

                    ParentPropertyId = x.ParentPropertyId,
                    ParentPropertyTitle = x.ParentProperty != null ? x.ParentProperty.Title : null,

                    Title = x.Title,
                    Description = x.Description,

                    Purpose = x.Purpose,
                    PropertyStatus = x.PropertyStatus,

                    Price = x.Price,
                    Area = x.Area,

                    City = x.City,
                    District = x.District,
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,

                    ParkingSpots = x.ParkingSpots,
                    AgeInYears = x.AgeInYears,
                    FacingDirection = x.FacingDirection,
                    FurnishedStatus = x.FurnishedStatus,

                    RegaLicenseNumber = x.RegaLicenseNumber,
                    DeedNumber = x.DeedNumber,
                    MunicipalityNumber = x.MunicipalityNumber,

                    IsFeatured = x.IsFeatured,

                    CompanyId = x.CompanyId,

                    OwnerId = x.OwnerId,
                    OwnerName = x.Owner != null ? x.Owner.FullName : null,

                    AgentId = x.AgentId,
                    AgentName = x.Agent != null ? x.Agent.FullName : null,

                    CeilingHeight = x.CeilingHeight,
                    LoadingDocks = x.LoadingDocks,
                    ElectricityCapacity = x.ElectricityCapacity,
                    OfficeSpace = x.OfficeSpace,
                    SecurityRoom = x.SecurityRoom,

                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<PagedResult<WarehouseListItemDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            string? search = null,
            CancellationToken ct = default)
        {
            var query = _context.WarehouseProperties
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Title.Contains(search) ||
                    x.PropertyCode.Contains(search) ||
                    x.City.Contains(search) ||
                    x.District.Contains(search));
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new WarehouseListItemDto
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

                    ParentPropertyId = x.ParentPropertyId,
                    ParentPropertyTitle = x.ParentProperty != null ? x.ParentProperty.Title : null,

                    OwnerId = x.OwnerId,
                    OwnerName = x.Owner != null ? x.Owner.FullName : null,

                    AgentId = x.AgentId,
                    AgentName = x.Agent != null ? x.Agent.FullName : null,

                    CeilingHeight = x.CeilingHeight,
                    LoadingDocks = x.LoadingDocks,
                    OfficeSpace = x.OfficeSpace,
                    SecurityRoom = x.SecurityRoom,

                    IsFeatured = x.IsFeatured,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<WarehouseListItemDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
