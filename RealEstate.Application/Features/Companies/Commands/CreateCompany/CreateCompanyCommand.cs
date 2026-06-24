using MediatR;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Companies.Commands.CreateCompany
{
    public record CreateCompanyCommand(
    string Name,
    string Phone,
    string? Address,
    string? Logo,
    string SubscriptionPlan,
    DateTime SubscriptionExpiry
) : IRequest<ApiResponse<Guid>>;
}
