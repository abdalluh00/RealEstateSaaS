using MediatR;
using RealEstate.Application.Features.Payments.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Payments.Queries.GetPaymentSummary
{
    public class GetPaymentSummaryHandler
     : IRequestHandler<GetPaymentSummaryQuery, ApiResponse<PaymentSummaryDto>>
    {
        private readonly IPaymentRepository _repo;

        public GetPaymentSummaryHandler(IPaymentRepository repo) => _repo = repo;

        public async Task<ApiResponse<PaymentSummaryDto>> Handle(
            GetPaymentSummaryQuery request,
            CancellationToken ct)
        {
            var summary = await _repo.GetSummaryAsync(request.CompanyId);

            var result = new PaymentSummaryDto(
                summary.TotalExpected,
                summary.TotalCollected,
                summary.TotalOverdue,
                summary.TotalPending,
                summary.OverdueCount,
                summary.PendingCount
            );

            return ApiResponse<PaymentSummaryDto>.Ok(result);
        }
    }
}
