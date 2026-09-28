
using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Land;
using RealEstate.Application.DTOs.Properties.Office;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Offices.Query.GetOfficePages
{
    public sealed record GetOfficePagesQuery : IRequest<ApiResponse<PagedResult<OfficeListDto>>>, IAutoTenantRequest
    {
        
        public PropertyStatus? status { get; init; }
        public PropertyPurpose? purpose { get; init; }
        public FurnishedStatus? FurnishedStatus { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
