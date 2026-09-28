using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Building;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Buildings.Queries.GetBuildingById
{
    public record GetBuildingByIdQuery(Guid Id) : IRequest<ApiResponse<BuildingDetailDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
