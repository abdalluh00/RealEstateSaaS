using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Clients.Commands.UpdateClient
{
    public record UpdateClientCommand(
     Guid Id,
     string FullName,
     string Phone,
     string? Email,
     string LeadStatus,
     string? Source,
     string? Notes
 ) : IRequest<ApiResponse<bool>>;
}
