using MediatR;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Buildings.Queries.GetBuildingUnits
{
    public sealed class GetBuildingUnitsQueryHandler
        : IRequestHandler<GetBuildingUnitsQuery,
            ApiResponse<IReadOnlyList<PropertyListDto>>>
    {
        private readonly IPropertyRepository _properties;

        public GetBuildingUnitsQueryHandler(IPropertyRepository properties)
            => _properties = properties;

        public async Task<ApiResponse<IReadOnlyList<PropertyListDto>>> Handle(
            GetBuildingUnitsQuery query,
            CancellationToken ct)
        {
            var units = await _properties.GetChildrenAsync(
                query.BuildingId, query.CompanyId, ct);

            return ApiResponse<IReadOnlyList<PropertyListDto>>.Ok(units);
        }
    }
}