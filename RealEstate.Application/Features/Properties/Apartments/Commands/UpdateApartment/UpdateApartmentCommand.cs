using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Properties.Apartments.DTOs;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Apartments.Commands.UpdateApartment
{
    [Authorize(Roles = "Owner,Admin")]
    public record UpdateApartmentCommand(Guid ApartmentId, ApartmentUpsertDto Apartment)
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
