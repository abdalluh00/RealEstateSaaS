using MediatR;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.PropertyMedia.Queries.GetPropertyMedia
{
    public record GetPropertyMediaQuery(Guid PropertyId)
     : IRequest<ApiResponse<List<PropertyMediaDto>>>;
}
