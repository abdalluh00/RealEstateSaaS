using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Apartments.Commands.DeleteApartment
{
    [Authorize(Roles = "Owner,Admin")]
    public record DeleteApartmentCommand(Guid ApartmentId)
           : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
