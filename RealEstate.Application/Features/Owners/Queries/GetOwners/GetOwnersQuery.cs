using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Owner;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Owners.Queries.GetPagedOwners
{
    public sealed class GetPagedOwnersQuery
        : IRequest<ApiResponse<PagedResult<OwnerListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public bool? IsActive { get; init; }
        public OwnerType? OwnerType { get; init; }
        public string? Search { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}