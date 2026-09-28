using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Land;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Lands.Queries.GetLandDetail
{
    public sealed class GetLandDetailQuery
        : IRequest<ApiResponse<LandDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}