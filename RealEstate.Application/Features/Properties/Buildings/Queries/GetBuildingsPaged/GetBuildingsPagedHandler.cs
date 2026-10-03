using MediatR;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Buildings.Queries.GetPagedBuildings
{
    public sealed class GetPagedBuildingsQueryHandler
        : IRequestHandler<GetPagedBuildingsQuery,
            ApiResponse<PagedResult<BuildingListDto>>>
    {
        private readonly IBuildingRepository _buildings;

        public GetPagedBuildingsQueryHandler(IBuildingRepository buildings)
            => _buildings = buildings;

        public async Task<ApiResponse<PagedResult<BuildingListDto>>> Handle(
            GetPagedBuildingsQuery query,
            CancellationToken ct)
        {
            var result = await _buildings.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                status: query.Status,
                purpose: query.Purpose,
                ct: ct);

            return ApiResponse<PagedResult<BuildingListDto>>.Ok(result);
        }
    }
}