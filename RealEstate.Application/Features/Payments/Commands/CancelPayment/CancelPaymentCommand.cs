using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Payments.Commands.CancelPayment
{
    public record CancelPaymentCommand(Guid PaymentId) : IRequest<ApiResponse<bool>>;

}
