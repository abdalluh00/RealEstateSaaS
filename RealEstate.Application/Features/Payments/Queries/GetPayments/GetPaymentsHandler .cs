using MediatR;
using RealEstate.Application.Features.Payments.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Payments.Queries.GetPayments
{
    public class GetPaymentsHandler : IRequestHandler<GetPaymentsQuery, ApiResponse<List<PaymentDto>>>
    {
        private readonly IPaymentRepository _repo;

        public GetPaymentsHandler(IPaymentRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<PaymentDto>>> Handle(
            GetPaymentsQuery request,
            CancellationToken ct)
        {
            var payments = await _repo.GetByContractAsync(request.ContractId);

            var result = payments.Select(p => new PaymentDto(
                p.Id, p.Amount, p.DueDate,
                p.PaidDate, p.Status,
                p.Method, p.Reference, p.DaysOverdue
            )).ToList();

            return ApiResponse<List<PaymentDto>>.Ok(result);
        }
    }
}
