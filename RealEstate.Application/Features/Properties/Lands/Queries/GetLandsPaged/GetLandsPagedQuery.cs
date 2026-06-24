using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Properties.Lands.Dtos;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Lands.Queries.GetLandsPaged
{
    public record GetLandsPagedQuery(int Page = 1, int PageSize = 10)
        : IRequest<ApiResponse<PagedResult<LandListItemDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
