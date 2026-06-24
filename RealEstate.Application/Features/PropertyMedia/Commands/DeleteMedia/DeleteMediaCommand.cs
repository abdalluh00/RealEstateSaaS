using MediatR;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.PropertyMedia.Commands.DeleteMedia
{
    public record DeleteMediaCommand(Guid Id) : IRequest<ApiResponse<bool>>;

}
