using MediatR;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Buildings.Queries.GetBuildingsPaged
{ 
    public record GetBuildingsPagedQuery
        (
       Guid CompanyId,
       int Page = 1,
       int PageSize = 20,
       PropertyStatus? Status = null,
       PropertyPurpose? Purpose = null
   ) : IRequest<ApiResponse<PagedResult<BuildingListDto>>>;
}
