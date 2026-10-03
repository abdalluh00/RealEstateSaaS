using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Buildings.Queries.GetBuildingUnits
{
    public sealed class GetBuildingUnitsQuery
        : IRequest<ApiResponse<IReadOnlyList<PropertyListDto>>>, IAutoTenantRequest
    {
        public Guid BuildingId { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}