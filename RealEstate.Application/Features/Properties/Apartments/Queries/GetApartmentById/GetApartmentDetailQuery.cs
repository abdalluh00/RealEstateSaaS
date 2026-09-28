using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Apartment;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Apartments.Queries.GetApartmentById
{
    [Authorize(Roles = "Owner,Admin,Agent")]
    public sealed class GetApartmentDetailQuery
        : IRequest<ApiResponse<ApartmentDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
