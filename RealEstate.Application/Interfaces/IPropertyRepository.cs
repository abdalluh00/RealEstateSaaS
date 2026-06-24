using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.ReadModels.PropertyModel;
using RealEstate.Shared.Common;

namespace RealEstate.Domain.Interfaces
{

    public interface IPropertyRepository : IGenericRepository<Property>
    {
        // ── Validation ────────────────────────────────────
        Task<bool> PropertyCodeExistsAsync(
            string code,
            Guid companyId,
            CancellationToken ct = default);
        Task<IEnumerable<PropertyListDto>> GetChildrenAsync(Guid parentPropertyId, Guid companyId, CancellationToken ct = default);
        Task<bool> IsAvailableAsync(Guid id, CancellationToken ct = default);
        Task<int> CountByStatusAsync(Guid companyId, PropertyStatus status, CancellationToken ct = default);
        Task<bool> IsOwnerHasPropertyAsync(Guid ownerId, CancellationToken ct = default);

        Task<bool> IsOwnerLinkedToAnyPropertyAsync(
            Guid ownerId,
            Guid companyId,
            CancellationToken ct = default);

        // ── Paged List (mixed all types) ──────────────────
        Task<PagedResult<PropertyListReadModel>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            string? city = null,
            CancellationToken ct = default);

        // ── Agent Properties (paged) ──────────────────────
        Task<PagedResult<PropertyListReadModel>> GetByAgentAsync(
            Guid agentId,
            Guid companyId,
            int page,
            int pageSize,
            CancellationToken ct = default);

        // ── Dashboard ─────────────────────────────────────
        Task<PropertyDashboardReadModel> GetDashboardStatsAsync(
            Guid companyId,
            CancellationToken ct = default);

        Task<IReadOnlyList<PropertyListReadModel>> GetFeaturedAsync(
            Guid companyId,
            int limit,
            CancellationToken ct = default);

        // ── Commands ──────────────────────────────────────
        Task<Property?> GetByIdForDeleteAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);
        Task<IEnumerable<PropertyListDto>> GetByOwnerAsync(Guid ownerId, Guid companyId, CancellationToken ct = default);

    }
}

