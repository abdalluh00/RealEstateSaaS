using MediatR;
using RealEstate.Application.Features.Payments.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Payments.Queries.GetOverduePayments
{
    public class GetOverduePaymentsHandler
    : IRequestHandler<GetOverduePaymentsQuery, ApiResponse<List<OverduePaymentDto>>>
    {
        private readonly IPaymentRepository _repo;

        public GetOverduePaymentsHandler(IPaymentRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<OverduePaymentDto>>> Handle(
            GetOverduePaymentsQuery request,
            CancellationToken ct)
        {
            var payments = await _repo.GetOverdueAsync(request.CompanyId);

            var result = payments.Select(p => new OverduePaymentDto(
                p.Id,
                p.PropertyTitle,
                p.ClientName,
                p.ClientPhone,
                p.Amount,
                p.DueDate,
                p.DaysOverdue,
                p.ContractId
            )).ToList();

            return ApiResponse<List<OverduePaymentDto>>.Ok(result);
        }
    }
}
