using MediatR;
using RealEstate.Application.Features.PropertyMedia.DTO;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.PropertyMedia.Queries.GetPropertyMedia
{
    public record GetPropertyMediaQuery(Guid PropertyId)
     : IRequest<ApiResponse<List<MediaDto>>>;
}
