using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Owners.Commands.DeleteOwner
{
    public record DeleteOwnerCommand(Guid Id) : IRequest<ApiResponse<bool>>;

}
