using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.PropertyMedia.Commands.SetCover
{
    public record SetCoverCommand(Guid MediaId) : IRequest<ApiResponse<bool>>;
}
