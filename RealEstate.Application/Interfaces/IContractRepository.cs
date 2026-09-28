using RealEstate.Application.DTOs.Contracts;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        // ── Queries ───────────────────────────────────────
        Task<PagedResult<ContractListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            ContractStatus? status = null,
            ContractType? contractType = null,
            Guid? agentId = null,
            Guid? clientId = null,
            Guid? propertyId = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            CancellationToken ct = default);

        Task<ContractDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Commands support ──────────────────────────────
        Task<Contract?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Validation ────────────────────────────────────
        Task<bool> HasOpenContractForPropertyAsync(
            Guid propertyId,
            Guid companyId,
            Guid? excludeContractId = null,
            CancellationToken ct = default);
    }
}