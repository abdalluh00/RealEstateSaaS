using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Owner;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Owners.Queries.GetOwnerDetail
{
    public sealed class GetOwnerDetailQuery
        : IRequest<ApiResponse<OwnerDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}