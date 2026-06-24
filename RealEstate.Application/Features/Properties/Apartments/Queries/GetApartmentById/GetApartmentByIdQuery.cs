using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Apartments.DTOs;
using RealEstate.Application.Features.Properties.Apartments.DTOs;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Apartments.Queries.GetApartmentById
{
    [Authorize(Roles = "Owner,Admin,Agent")]
    public record GetApartmentByIdQuery(Guid ApartmentId)
        : IRequest<ApiResponse<ApartmentDetailsDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
