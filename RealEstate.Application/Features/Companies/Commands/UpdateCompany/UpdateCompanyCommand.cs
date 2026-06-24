using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Companies.Commands.UpdateCompany
{
    public record UpdateCompanyCommand(
    Guid Id,
    string Name,
    string Phone,
    string? Address,
    string? Logo,
    string SubscriptionPlan,
    DateTime SubscriptionExpiry,
    bool IsActive
) : IRequest<ApiResponse<bool>>;
}
