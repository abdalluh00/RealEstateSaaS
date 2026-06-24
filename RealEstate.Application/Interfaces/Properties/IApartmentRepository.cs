using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.ReadModels.PropertyModel;
using RealEstate.Shared.Common;

namespace RealEstate.Domain.Interfaces.Properties
{
    public interface IApartmentRepository : IGenericRepository<ApartmentProperty>
    {
        // ── Paged list with apartment-specific filters ────
        Task<PagedResult<ApartmentListReadModel>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            int? minBedrooms = null,
            int? maxBedrooms = null,
            FurnishedStatus? furnishedStatus = null,
            CancellationToken ct = default);

        // ── Detail — base + apartment fields + relations ──
        Task<ApartmentDetailReadModel?> GetByIdWithDetailsAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Commands support ──────────────────────────────
        Task<ApartmentProperty?> GetByIdForUpdateAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);
    }

}
