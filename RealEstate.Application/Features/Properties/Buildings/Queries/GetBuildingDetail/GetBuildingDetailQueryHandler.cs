using MediatR;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Buildings.Queries.GetBuildingDetail
{
    public sealed class GetBuildingDetailQueryHandler
        : IRequestHandler<GetBuildingDetailQuery, ApiResponse<BuildingDetailDto>>
    {
        private readonly IBuildingRepository _buildings;

        public GetBuildingDetailQueryHandler(IBuildingRepository buildings)
            => _buildings = buildings;

        public async Task<ApiResponse<BuildingDetailDto>> Handle(
            GetBuildingDetailQuery query,
            CancellationToken ct)
        {
            var building = await _buildings.GetDetailByIdAsync(
                query.Id, query.CompanyId, ct);

            if (building is null)
                throw new NotFoundException("العمارة", query.Id);

            return ApiResponse<BuildingDetailDto>.Ok(building);
        }
    }
}