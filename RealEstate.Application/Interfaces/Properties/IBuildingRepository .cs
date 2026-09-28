using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces.Properties
{
    public interface IBuildingRepository : IGenericRepository<BuildingProperty>
    {
        // ── Queries ───────────────────────────────────────
        Task<PagedResult<BuildingListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            CancellationToken ct = default);

        Task<BuildingDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Validation ────────────────────────────────────
        Task<bool> HasUnitsAsync(
            Guid buildingId,
            CancellationToken ct = default);

        Task<int> CountUnitsByStatusAsync(
            Guid buildingId,
            PropertyStatus status,
            CancellationToken ct = default);
    }
}