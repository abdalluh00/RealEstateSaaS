using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Land;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Lands.Queries.GetPagedLands
{
    public sealed class GetPagedLandsQuery
        : IRequest<ApiResponse<PagedResult<LandListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public PropertyStatus? Status { get; init; }
        public PropertyPurpose? Purpose { get; init; }
        public ZoningType? ZoningType { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}