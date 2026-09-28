using MediatR;
using RealEstate.Application.DTOs.Payments;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Payments.Queries.GetContractPayments
{
    public sealed class GetContractPaymentsQueryHandler
        : IRequestHandler<GetContractPaymentsQuery,
            ApiResponse<IReadOnlyList<PaymentListDto>>>
    {
        private readonly IPaymentRepository _payments;

        public GetContractPaymentsQueryHandler(IPaymentRepository payments)
            => _payments = payments;

        public async Task<ApiResponse<IReadOnlyList<PaymentListDto>>> Handle(
            GetContractPaymentsQuery query,
            CancellationToken ct)
        {
            var result = await _payments.GetByContractAsync(
                query.ContractId, query.CompanyId, ct);

            return ApiResponse<IReadOnlyList<PaymentListDto>>.Ok(result);
        }
    }
}