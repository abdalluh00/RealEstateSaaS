using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Domain.ReadModels.PropertyModel;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
namespace RealEstate.Infrastructure.Repositories
{
    public class ApartmentRepository : GenericRepository<ApartmentProperty>, IApartmentRepository
    {
        public ApartmentRepository(AppDbContext context) : base(context) { }

        // ── Paged List ────────────────────────────────────

        public async Task<PagedResult<ApartmentListReadModel>> GetPagedAsync(
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
                .Where(x => x.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(x => x.PropertyStatus == status.Value);

            if (purpose.HasValue)
                query = query.Where(x => x.Purpose == purpose.Value);

            if (minBedrooms.HasValue)
                query = query.Where(x => x.Bedrooms >= minBedrooms.Value);

            if (maxBedrooms.HasValue)
                query = query.Where(x => x.Bedrooms <= maxBedrooms.Value);

            if (furnishedStatus.HasValue)
                query = query.Where(x => x.FurnishedStatus == furnishedStatus.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<ApartmentListReadModel>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ApartmentListReadModel
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
                    CreatedAt = x.CreatedAt,
                    Bedrooms = x.Bedrooms,
                    Bathrooms = x.Bathrooms,
                    FloorNumber = x.FloorNumber,
                    FurnishedStatus = x.FurnishedStatus,
                    HasElevator = x.HasElevator,
                    HasBalcony = x.HasBalcony
                })
                .ToListAsync(ct);

            return PagedResult<ApartmentListReadModel>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────

        public async Task<ApartmentDetailReadModel?> GetByIdWithDetailsAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.Id == id && x.CompanyId == companyId)
                .Select(x => new ApartmentDetailReadModel
                {
                    // ── Base ──────────────────────────────
                    Id = x.Id,
                    PropertyCode = x.PropertyCode,
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
                    IsFeatured = x.IsFeatured,
                    IsPublished = x.IsPublished,
                    DeedNumber = x.DeedNumber,
                    RegaLicenseNumber = x.RegaLicenseNumber,
                    MunicipalityNumber = x.MunicipalityNumber,

                    // ── Apartment-specific ────────────────
                    Bedrooms = x.Bedrooms,
                    Bathrooms = x.Bathrooms,
                    LivingRooms = x.LivingRooms,
                    FloorNumber = x.FloorNumber,
                    HasMaidRoom = x.HasMaidRoom,
                    HasElevator = x.HasElevator,
                    HasCentralAC = x.HasCentralAC,
                    HasBalcony = x.HasBalcony,
                    HasStorage = x.HasStorage,
                    FurnishedStatus = x.FurnishedStatus,

                    // ── Relations (no Include needed — projected directly) ──
                    OwnerId = x.OwnerId,
                    OwnerName = x.Owner != null
                        ? x.Owner.FullName
                        : null,
                    AgentId = x.AgentId,
                    AgentName = x.Agent != null
                        ? x.Agent.FullName
                        : null,

                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);

        // ── Commands ──────────────────────────────────────

        public async Task<ApartmentProperty?> GetByIdForUpdateAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId, ct);
        // Tracked — EF generates UPDATE on SaveChanges
        // No AsNoTracking — we need change tracking for the update command
    }
}
