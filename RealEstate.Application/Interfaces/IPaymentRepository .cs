using RealEstate.Application.DTOs.Payments;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using PaymentSummaryDto = RealEstate.Application.DTOs.Payments.PaymentSummaryDto;

namespace RealEstate.Application.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        // ── Queries ───────────────────────────────────────
        Task<IReadOnlyList<PaymentListDto>> GetByContractAsync(
            Guid contractId,
            Guid companyId,
            CancellationToken ct = default);

        Task<PaymentSummaryDto> GetContractSummaryAsync(
            Guid contractId,
            Guid companyId,
            CancellationToken ct = default);

        Task<PagedResult<PaymentListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PaymentStatus? status = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            CancellationToken ct = default);

        // ── Commands ──────────────────────────────────────
        Task<Payment?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Background job ────────────────────────────────
        Task<int> MarkOverdueAsync(CancellationToken ct = default);
    }
}