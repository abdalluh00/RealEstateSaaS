using MediatR;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Companies.Commands.UpdateCompany
{
    public record UpdateCompanyMeCommand(
    Guid Id,
    string Name,
    string Phone,
    string? Address,
    string? Logo
) : IRequest<ApiResponse<bool>>;

    
}
