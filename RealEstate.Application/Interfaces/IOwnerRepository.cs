using RealEstate.Application.DTOs.Owner;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        // ── Queries ───────────────────────────────────────
        Task<PagedResult<OwnerListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            bool? isActive = null,
            OwnerType? ownerType = null,
            string? search = null,
            CancellationToken ct = default);

        Task<OwnerDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Validation ────────────────────────────────────
        Task<bool> PhoneExistsAsync(
            string phone,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> PhoneExistsForAnotherOwnerAsync(
            string phone,
            Guid ownerId,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> ExistsInCompanyAsync(
            Guid ownerId,
            Guid companyId,
            CancellationToken ct = default);
    }
}