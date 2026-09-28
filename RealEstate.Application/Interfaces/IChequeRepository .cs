using RealEstate.Application.DTOs.Cheques;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces
{
    public interface IChequeRepository : IGenericRepository<Cheque>
    {
        // ── Queries ───────────────────────────────────────

        Task<PagedResult<ChequeListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            Guid? contractId = null,
            ChequeStatus? status = null,
            DateTime? from = null,
            DateTime? to = null,
            CancellationToken ct = default);

        Task<ChequeDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        Task<IReadOnlyList<ChequeListDto>> GetByContractAsync(
            Guid contractId,
            Guid companyId,
            CancellationToken ct = default);

        // ── Validation ────────────────────────────────────

        Task<bool> ExistsInCompanyAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> ChequeNumberExistsAsync(
            string chequeNumber,
            Guid companyId,
            Guid? excludeChequeId = null,
            CancellationToken ct = default);
    }
}