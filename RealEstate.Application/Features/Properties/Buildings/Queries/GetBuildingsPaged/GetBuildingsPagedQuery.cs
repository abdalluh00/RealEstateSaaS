using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Buildings.Queries.GetPagedBuildings
{
    public sealed class GetPagedBuildingsQuery
        : IRequest<ApiResponse<PagedResult<BuildingListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public PropertyStatus? Status { get; init; }
        public PropertyPurpose? Purpose { get; init; }
        public bool? HasElevator { get; init; }
        public int? MinFloors { get; init; }
        public int? MaxFloors { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}