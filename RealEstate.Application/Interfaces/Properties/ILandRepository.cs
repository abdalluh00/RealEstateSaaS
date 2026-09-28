using RealEstate.Application.DTOs.Properties.Land;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces.Properties
{
    public interface ILandRepository : IGenericRepository<LandProperty>
    {
        // ── Queries ───────────────────────────────────────
        Task<PagedResult<LandListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            ZoningType? zoningType = null,
            CancellationToken ct = default);

        Task<LandDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Validation ────────────────────────────────────
        Task<bool> IsAvailableAsync(
            Guid id,
            CancellationToken ct = default);
    }
}