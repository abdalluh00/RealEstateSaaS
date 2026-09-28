using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Villa;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Villas.Queries.GetVillaById
{
    [Authorize(Roles = "Owner,Admin,Agent")]
    public record GetVillaByIdQuery(Guid Id) : IRequest<ApiResponse<VillaDetailDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
