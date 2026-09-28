using MediatR;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Application.Features.Properties.Buildings.Queries.GetBuildingsPaged;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Building.Queries.GetBuildings
{
    public class GetBuildingsPagedHandler
        : IRequestHandler<GetBuildingsPagedQuery, ApiResponse<PagedResult<BuildingListDto>>>
    {
        private readonly IBuildingRepository _buildingRepo;

        public GetBuildingsPagedHandler(IBuildingRepository buildingRepo) =>
            _buildingRepo = buildingRepo;

        public async Task<ApiResponse<PagedResult<BuildingListDto>>> Handle(
            GetBuildingsPagedQuery request,
            CancellationToken ct)
        {
            var result = await _buildingRepo.GetPagedAsync(
                request.CompanyId,
                request.Page,
                request.PageSize,
                request.Status,
                request.Purpose,
                ct);

            return ApiResponse<PagedResult<BuildingListDto>>.Ok(result);
        }
    }
}