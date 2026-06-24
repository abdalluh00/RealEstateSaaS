
using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Properties.Apartments.DTOs;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Apartments.Commands.CreateApartment
{
    [Authorize(Roles = "Owner,Admin")]
    public record CreateApartmentCommand(ApartmentUpsertDto Apartment)
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
