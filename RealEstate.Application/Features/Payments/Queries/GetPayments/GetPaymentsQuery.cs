using MediatR;
using RealEstate.Application.Features.Payments.DTO;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Payments.Queries.GetPayments
{
    public record GetPaymentsQuery(
    Guid ContractId
) : IRequest<ApiResponse<List<PaymentDto>>>;

   
}
