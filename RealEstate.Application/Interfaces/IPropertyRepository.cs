using RealEstate.Application.DTOs.Properties;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces.Properties
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        // ── Validation ────────────────────────────────────
        Task<bool> ExistsAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> IsAvailableAsync(
            Guid id,
            CancellationToken ct = default);

        Task<bool> PropertyCodeExistsAsync(
            string code,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> IsOwnerHasPropertyAsync(
            Guid ownerId,
            CancellationToken ct = default);

        Task<bool> IsOwnerLinkedToAnyPropertyAsync(
            Guid ownerId,
            Guid companyId,
            CancellationToken ct = default);

        Task<int> CountByStatusAsync(
            Guid companyId,
            PropertyStatus status,
            CancellationToken ct = default);

        // ── Paged Lists ───────────────────────────────────
        Task<PagedResult<PropertyListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            string? city = null,
            CancellationToken ct = default);

        Task<PagedResult<PropertyListDto>> GetByAgentAsync(
            Guid agentId,
            Guid companyId,
            int page,
            int pageSize,
            CancellationToken ct = default);

        Task<PagedResult<PropertyListDto>> GetByOwnerAsync(
            Guid ownerId,
            Guid companyId,
            int page,
            int pageSize,
            CancellationToken ct = default);

        // ── Children (units inside building/compound) ─────
        Task<IReadOnlyList<PropertyListDto>> GetChildrenAsync(
            Guid parentPropertyId,
            Guid companyId,
            CancellationToken ct = default);

        // ── Featured ──────────────────────────────────────
        Task<IReadOnlyList<PropertyListDto>> GetFeaturedAsync(
            Guid companyId,
            int limit,
            CancellationToken ct = default);

        // ── Dashboard ─────────────────────────────────────
        Task<PropertyDashboardDto> GetDashboardStatsAsync(
            Guid companyId,
            int featuredLimit,
            CancellationToken ct = default);

        // ── Commands ──────────────────────────────────────
        Task<Property?> GetByIdForDeleteAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);
    }
}