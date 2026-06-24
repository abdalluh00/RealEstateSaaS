using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Owners.Commands.UpdateOwner
{
    public record UpdateOwnerCommand(
    Guid Id,
    string FullName,
    string Phone,
    string? Email,
    string? IdNumber,
    string? Notes
) : IRequest<ApiResponse<bool>>;
}
