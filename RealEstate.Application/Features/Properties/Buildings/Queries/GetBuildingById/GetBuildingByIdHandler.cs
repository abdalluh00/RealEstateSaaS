using MediatR;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Application.Features.Properties.Buildings.Queries.GetBuildingById;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Properties.Building.Queries.GetBuildingById
{
    public class GetBuildingByIdHandler
        : IRequestHandler<GetBuildingByIdQuery, ApiResponse<BuildingDetailDto>>
    {
        private readonly IBuildingRepository _buildingRepo;

        public GetBuildingByIdHandler(IBuildingRepository buildingRepo) =>
            _buildingRepo = buildingRepo;

        public async Task<ApiResponse<BuildingDetailDto>> Handle(
            GetBuildingByIdQuery request,
            CancellationToken ct)
        {
            var building = await _buildingRepo.GetDetailByIdAsync(
                request.Id,
                request.CompanyId,
                ct)
                ?? throw new NotFoundException("المبنى غير موجود");

            return ApiResponse<BuildingDetailDto>.Ok(building);
        }
    }
}