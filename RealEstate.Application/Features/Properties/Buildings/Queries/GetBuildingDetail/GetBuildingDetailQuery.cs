using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Buildings.Queries.GetBuildingDetail
{
    public sealed class GetBuildingDetailQuery
        : IRequest<ApiResponse<BuildingDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}