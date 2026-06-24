using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Clients.Commands.DeleteClient
{
    public record DeleteClientCommand(Guid Id) : IRequest<ApiResponse<bool>>;

}
