using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Apartment;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Apartments.Queries.GetApartments
{
    [Authorize(Policy = $"{Policies.AgentAndUp}")]
    public sealed class GetPagedApartmentsQuery
         : IRequest<ApiResponse<PagedResult<ApartmentListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public PropertyStatus? Status { get; init; }
        public PropertyPurpose? Purpose { get; init; }
        public int? MinBedrooms { get; init; }
        public int? MaxBedrooms { get; init; }
        public FurnishedStatus? FurnishedStatus { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
