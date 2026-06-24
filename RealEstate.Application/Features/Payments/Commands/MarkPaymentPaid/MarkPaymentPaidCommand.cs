using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid
{
    public record MarkPaymentPaidCommand(
    Guid PaymentId,
    string Method,
    string? Reference
) : IRequest<ApiResponse<bool>>;
}
