using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Features.Properties.Villas.DTOs;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Queries.GetVillaById
{
    [Authorize(Roles = "Owner,Admin,Agent")]
    public record GetVillaByIdQuery(Guid Id) : IRequest<ApiResponse<VillaDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
