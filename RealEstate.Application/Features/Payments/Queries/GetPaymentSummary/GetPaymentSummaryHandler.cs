using MediatR;
using RealEstate.Application.DTOs.Payments;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Payments.Queries.GetContractPaymentSummary
{
    public sealed class GetContractPaymentSummaryQueryHandler
        : IRequestHandler<GetContractPaymentSummaryQuery, ApiResponse<PaymentSummaryDto>>
    {
        private readonly IPaymentRepository _payments;

        public GetContractPaymentSummaryQueryHandler(IPaymentRepository payments)
            => _payments = payments;

        public async Task<ApiResponse<PaymentSummaryDto>> Handle(
            GetContractPaymentSummaryQuery query,
            CancellationToken ct)
        {
            var result = await _payments.GetContractSummaryAsync(
                query.ContractId, query.CompanyId, ct);

            return ApiResponse<PaymentSummaryDto>.Ok(result);
        }
    }
}