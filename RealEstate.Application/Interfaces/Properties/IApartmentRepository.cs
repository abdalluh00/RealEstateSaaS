// Application/Interfaces/Properties/IApartmentRepository.cs
using RealEstate.Application.DTOs.Properties.Apartment;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces.Properties
{
    public interface IApartmentRepository : IGenericRepository<ApartmentProperty>
    {
        // ── Queries ───────────────────────────────────────
        Task<PagedResult<ApartmentListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            int? minBedrooms = null,
            int? maxBedrooms = null,
            FurnishedStatus? furnishedStatus = null,
            CancellationToken ct = default);

        Task<ApartmentDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Validation ────────────────────────────────────
        Task<bool> IsAvailableAsync(
            Guid id,
            CancellationToken ct = default);

        Task<bool> UnitNumberExistsAsync(
            string unitNumber,
            Guid parentPropertyId,
            CancellationToken ct = default);
    }
}