using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Properties.Apartments.DTOs;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Apartments.Queries.GetApartments
{
    [Authorize(Roles = "Owner,Admin,Agent")]
    public record GetApartmentsQuery(
         int Page = 1,
         int PageSize = 20,
         string? Search = null,
         PropertyPurpose? Purpose = null,
         PropertyStatus? PropertyStatus = null,
         Guid? ParentPropertyId = null)
         : IRequest<ApiResponse<PagedResult<ApartmentListItemDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
